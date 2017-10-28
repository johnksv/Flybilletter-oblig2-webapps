﻿using AutoMapper;
using DAL;
using Flybilletter.DAL.Interfaces;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flybilletter.DAL.DBModel
{
    public class DBFlygning : IDBFlygning
    {
        public int ID { get; set; }
        public virtual List<DBBestilling> Bestillinger { get; set; }
        public virtual DBRute Rute { get; set; }
        public virtual DBFly Fly { get; set; }
        public DateTime AvgangsTid { get; set; }
        public bool Kansellert { get; set; }

        public List<Flygning> HentFlygningerFra(Flyplass flyplass)
        {
            using (var db = new DB())
            {
                try
                {
                    return Mapper.Map<List<Flygning>>(db.Flygninger.Include("Fly").Where(flygning => flygning.Rute.Fra.ID == flyplass.ID).ToList());
                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e, "En feil oppsto da metoden prøvde å hente flygninger");
                    return null;
                }
            }
        }

        public List<Flygning> HentFlygningerTil(Flyplass flyplass)
        {
            using (var db = new DB())
            {
                try
                {
                    return Mapper.Map<List<Flygning>>(db.Flygninger.Include("Fly").Where(flygning => flygning.Rute.Til.ID == flyplass.ID).ToList());
                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e, "En feil oppsto da metoden prøvde å hente flygninger");
                    return null;
                }
            }
        }

        public Flygning Finn(int ID)
        {
            using (var db = new DB())
            {
                try
                {
                    return Mapper.Map<Flygning>(db.Flygninger.AsNoTracking().FirstOrDefault(fly => fly.ID == ID));
                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e, "En feil oppsto da metoden prøvde å finne flygning med ID " + ID);
                    return null;
                }
            }
        }

        public List<Flygning> HentAlle(DateTime dateTime)
        {
            using (var db = new DB())
            {

                var dbflygninger = db.Flygninger.Include("Fly").Where(flyg => flyg.AvgangsTid > dateTime).ToList();
                List<Flygning> flygninger = new List<Flygning>();
                foreach (var flygning in dbflygninger)
                {
                    flygninger.Add(Mapper.Map<Flygning>(flygning));
                }
                return flygninger;
            }
        }

        public Flygning HentEnFlygning(int id)
        {
            using (var db = new DB())
            {
                var dbflygning = db.Flygninger.Include("Fly").Where(item => item.ID == id).FirstOrDefault();
                return Mapper.Map<Flygning>(dbflygning);
            }
        }

        public bool OppdaterFlygning(Flygning flygning)
        {
            using (var db = new DB())
            {
                var dbflygning = db.Flygninger.Find(flygning.ID);
                dbflygning.AvgangsTid = flygning.AvgangsTid;

                try
                {
                    db.Endringer.Add(new DBEndring()
                    {
                        Tidspunkt = DateTime.Now,
                        Endring = "Oppdaterer flygning med ID " + flygning.ID
                    });
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e, "En feil oppsto da metoden prøvde å oppdatere flygning");
                    return false;
                }
                return true;
            }
        }

        public bool OppdaterStatus(int id)
        {
            using (var db = new DB())
            {
                DBFlygning dbflygning = db.Flygninger.Include("Bestillinger").Where(item => item.ID == id).FirstOrDefault();
                if (dbflygning != null)
                {
                    if (dbflygning.AvgangsTid < DateTime.Now && dbflygning.Bestillinger.Count() == 0)
                    {
                        return false;
                    }
                    dbflygning.Kansellert = !dbflygning.Kansellert;
                }
                try
                {
                    db.Endringer.Add(new DBEndring()
                    {
                        Tidspunkt = DateTime.Now,
                        Endring = "Endret status på flygning med ID: " + ID
                    });
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e, "En feil oppsto da metoden prøvde å oppdatere status på flygning");
                    return false;
                }
            }
        }

        public bool LeggInnFlygning(Flygning flygning)
        {
            using (var db = new DB())
            {
                var dbflygning = Mapper.Map<DBFlygning>(flygning);
                try
                {
                    dbflygning.Rute = db.Ruter.Find(dbflygning.Rute.ID);
                    db.Flyplasser.Find(dbflygning.Rute.Fra.ID);
                    db.Flyplasser.Find(dbflygning.Rute.Til.ID);
                    dbflygning.Fly = db.Fly.Find(dbflygning.Fly.ID);
                    db.Flygninger.Add(dbflygning);
                    db.Endringer.Add(new DBEndring()
                    {
                        Endring = "La til flygning ny flygning mellom: " + dbflygning.Rute.Fra.ID + " - " + dbflygning.Rute.Til.ID,
                        Tidspunkt = DateTime.Now
                    });
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil("DBFlygning:LeggInnFlygning", e.InnerException, "Feil under lagring til databasen");
                    return false;
                }
            }
        }

        public bool Endre(int id, DateTime nyAvgangstid)
        {
            using (var db = new DB())
            {
                try
                {
                    var eksisterendeFlygning = db.Flygninger.Find(id);
                    if (eksisterendeFlygning != null)
                    {
                        eksisterendeFlygning.AvgangsTid = nyAvgangstid;
                        db.Endringer.Add(new DBEndring()
                        {
                            Endring = $"Endret avgangstid på flygning {id}. Ny avgangstid {nyAvgangstid}.",
                            Tidspunkt = DateTime.Now
                        });

                        db.SaveChanges();
                        return true;
                    }

                    db.Endringer.Add(new DBEndring()
                    {
                        Endring = $"Prøvde å endre en flygning som ikke eksisterte: {id}",
                        Tidspunkt = DateTime.Now
                    });
                    db.SaveChanges();

                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil("DBFlygning:Endre", e.InnerException, $"Feil under lagring til databasen, med id: {id}");
                }

                return false;
            }
        }
    }
}
