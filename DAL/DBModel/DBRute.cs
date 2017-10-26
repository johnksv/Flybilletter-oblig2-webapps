using Flybilletter.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flybilletter.Model.DomeneModel;
using AutoMapper;
using DAL;

namespace Flybilletter.DAL.DBModel
{
    public class DBRute : IDBRute
    {
        public int ID { get; set; }
        public virtual DBFlyplass Fra { get; set; }
        public virtual DBFlyplass Til { get; set; }
        public double BasePris { get; set; } //faktor for å regne ut total pris for turen
        public TimeSpan Reisetid { get; set; }
        public virtual List<DBFlygning> Flygninger { get; set; }

        public List<Rute> HentAlle()
        {
            using (var db = new DB())
            {
                var dbruter = db.Ruter.Include("Fra").Include("Til").ToList();
                var ruter = new List<Rute>();
                foreach (var dbrute in dbruter)
                {
                    ruter.Add(Mapper.Map<Rute>(dbrute));
                }
                return ruter;
            }
        }

        public bool LagreRute(Rute innrute)
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

                    try
                    {
                        db.SaveChanges();
                        return true;
                    }
                    catch (Exception e)
                    {
                        DALsetup.LogFeilTilFil("DBRute:LagreRute", e);
                    }
                }
                return false;
            }
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
                        DALsetup.LogFeilTilFil("DBRute:Slett", exception);
                    }
                }
                return false;
            }
        }
    }
}