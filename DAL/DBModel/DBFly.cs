using AutoMapper;
using Flybilletter.DAL.Interfaces;
using System.Collections.Generic;
using Flybilletter.Model.DomeneModel;
using System;
using DAL;

namespace Flybilletter.DAL.DBModel
{
    public class DBFly : IDBFly
    {
        public int ID { get; set; }
        public string Modell { get; set; } //Modell-navn til flyet
        public int AntallSeter { get; set; }
        public virtual List<DBFlygning> Flygninger { get; set; }

        public Fly Hent(int ID)
        {
            using (var db = new DB())
            {
                try
                {
                    return Mapper.Map<Fly>(db.Fly.Find(ID));
                } catch (Exception e)
                {
                    DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e, "En feil oppsto da metoden prøvde å hente fly med ID "+ ID);
                    return null;
                }
            }
        }

        public List<Fly> HentAlle()
        {
            using (var db = new DB())
            {
                try
                {
                    return Mapper.Map<List<Fly>>(db.Fly);
                } catch (Exception e)
                {
                    DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e, "En feil oppsto da metoden prøvde å hente alle fly");
                    return null;
                }
            }
        }

        public bool Oppdater(Fly fly)
        {
            using (var db = new DB())
            {
                try
                {
                    // DB oppdater
                    var dbflyentitet = db.Fly.Find(fly.ID);
                    dbflyentitet.Modell = fly.Modell;
                    dbflyentitet.AntallSeter = fly.AntallSeter;

                    db.Endringer.Add(new DBEndring()
                    {
                        Endring = "Oppdaterer fly med id " + ID + ". Flymodell er " + dbflyentitet.Modell + ", antall seter er " + dbflyentitet.AntallSeter,
                        Tidspunkt = DateTime.Now
                    });

                    db.SaveChanges();
                    return true;
                } catch (Exception e)
                {
                    DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e, "En feil oppsto da metoden prøvde å oppdatere fly");
                    return false;
                }
            }
        }

        public bool Slett(int ID)
        {
            using (var db = new DB())
            {
                var fly = db.Fly.Find(ID);

                if (fly != null)
                {
                    db.Fly.Remove(fly);
                    db.Endringer.Add(new DBEndring()
                    {
                        Endring = "Slett fly med id " + ID,
                        Tidspunkt = DateTime.Now
                    });
                    try
                    {
                        db.SaveChanges();
                        return true;
                    }
                    catch (Exception e)
                    {
                        DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e, "En feil oppsto da metoden prøvde å slette fly med ID " + ID);
                        return false;
                    }
                }
                return false;
            }
        }

        public bool LeggTil(Fly fly)
        {
            using (var db = new DB())
            {
                try
                {
                    DBFly dbFly = Mapper.Map<DBFly>(fly);
                    db.Fly.Add(dbFly);
                    db.Endringer.Add(new DBEndring()
                    {
                        Tidspunkt = DateTime.Now,
                        Endring = "Legg til fly med ID: " + fly.ID
                    });
                    
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e, "En feil oppsto da metoden prøvde å legge til et nytt fly");
                    return false;
                }

            }
        }
    }
}