using Flybilletter.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flybilletter.Model.DomeneModel;
using AutoMapper;
using DAL;
using System.Diagnostics.CodeAnalysis;

namespace Flybilletter.DAL.DBModel
{
    [ExcludeFromCodeCoverage]
    public class DBRute : IDBRute
    {
        public int ID { get; set; }
        public virtual DBFlyplass Fra { get; set; }
        public virtual DBFlyplass Til { get; set; }
        public double BasePris { get; set; }
        public TimeSpan Reisetid { get; set; }
        public virtual List<DBFlygning> Flygninger { get; set; }

        public Rute Hent(int ruteID)
        {
            using (var db = new DB())
            {
                return Mapper.Map<Rute>(db.Ruter.Find(ruteID));
            }
        }

        public List<Rute> HentAlle()
        {
            using (var db = new DB())
            {
                try
                {
                    var dbruter = db.Ruter.Include("Fra").Include("Til").ToList();
                    var ruter = new List<Rute>();
                    foreach (var dbrute in dbruter)
                    {
                        ruter.Add(Mapper.Map<Rute>(dbrute));
                    }
                    return ruter;
                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil("DBRute:HentAlle", e, "En feil oppsto da metoden prøvde å hente alle rutene.");
                    return null;
                }
            }
        }

        public bool Endre(Rute innrute)
        {
            using (var db = new DB())
            {
                var rute = db.Ruter.FirstOrDefault(r => r.ID == innrute.ID);
                var fraFlyplass = db.Flyplasser.FirstOrDefault(flyplass => flyplass.ID == innrute.Fra.ID);
                var tilFlyplass = db.Flyplasser.FirstOrDefault(flyplass => flyplass.ID == innrute.Til.ID);
                if (rute != null && fraFlyplass != null && tilFlyplass != null)
                {
                    rute.Fra = fraFlyplass;
                    rute.Til = tilFlyplass;
                    rute.Reisetid = innrute.Reisetid;
                    rute.BasePris = innrute.BasePris;

                    db.Endringer.Add(new DBEndring()
                    {
                        Endring = $"Endrer rute med nye verdier. Ny verdier: fra flyplass: {rute.Fra.ID}, til flyplass: {rute.Til.ID}, reisetid: {rute.Reisetid}, pris: {rute.BasePris} ",
                        Tidspunkt = DateTime.Now
                    });

                    try
                    {
                        db.SaveChanges();
                        return true;
                    }
                    catch (Exception e)
                    {
                        DALsetup.LogFeilTilFil("DBRute:Endre", e, "En feil oppsto da metoden prøvde å endre rute.");
                    }
                }
                return false;
            }
        }

        public bool LeggInn(Rute innrute)
        {
            using (var db = new DB())
            {
                try
                {

                    var rute = Mapper.Map<DBRute>(innrute);
                    var fraFlyplass = db.Flyplasser.FirstOrDefault(flyplass => flyplass.ID == innrute.Fra.ID);
                    var tilFlyplass = db.Flyplasser.FirstOrDefault(flyplass => flyplass.ID == innrute.Til.ID);

                    if (fraFlyplass.ID == tilFlyplass.ID || (fraFlyplass.ID != "OSL" && tilFlyplass.ID != "OSL")) return false;
                    if (fraFlyplass != null && tilFlyplass != null)
                    {
                        rute.Fra = fraFlyplass;
                        rute.Til = tilFlyplass;
                        db.Flyplasser.Attach(fraFlyplass);
                        db.Flyplasser.Attach(tilFlyplass);
                        db.Ruter.Add(rute);
                        db.Endringer.Add(new DBEndring()
                        {
                            Endring = $"Lager en ny rute: fra flyplass: {rute.Fra.ID}, til flyplass: {rute.Til.ID}, reisetid: {rute.Reisetid}, pris: {rute.BasePris} ",
                            Tidspunkt = DateTime.Now
                        });

                        db.SaveChanges();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil("DBRute:LeggInn", e, "En feil oppsto da metoden prøvde å lagre rute.");
                }
            }
            return false;
        }


        public bool Slett(int id)
        {
            using (var db = new DB())
            {
                var rute = db.Ruter.FirstOrDefault(r => r.ID == id);
                if (rute != null)
                {
                    db.Endringer.Add(new DBEndring()
                    {
                        Endring = $"Fjernet rute med ID: {rute.ID}",
                        Tidspunkt = DateTime.Now
                    });
                    db.Ruter.Remove(rute);

                    try
                    {
                        db.SaveChanges();
                        return true;
                    }
                    catch (Exception exception)
                    {
                        DALsetup.LogFeilTilFil("DBRute.Slett", exception, "En feil oppsto da metoden prøvde å slette rute.");
                    }
                }
                return false;
            }
        }
    }
}