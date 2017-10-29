using AutoMapper;
using DAL;
using Flybilletter.DAL.Interfaces;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;

namespace Flybilletter.DAL.DBModel
{
    [ExcludeFromCodeCoverage]
    public class DBFlyplass : IDBFlyplass
    {
        [Required]
        public string ID { get; set; }
        [Required]
        public string Navn { get; set; }
        [Required]
        public string By { get; set; }
        [Required]
        public string Land { get; set; }
        public virtual List<DBRute> Ruter { get; set; }

        public List<Flyplass> HentAlle()
        {
            using (var db = new DB())
            {
                try
                {
                    return Mapper.Map<List<Flyplass>>(db.Flyplasser.ToList());
                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil("DBFlyplass:HentAlle", e, "En feil oppsto da metoden prøvde å hente alle flyplasser fra databasen");
                    return null;
                }
            }
        }

        public Flyplass Hent(string tilFlyplassID)
        {
            using (var db = new DB())
            {
                try
                {
                    return Mapper.Map<Flyplass>(db.Flyplasser.Find(tilFlyplassID));
                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil("DBFlyplass:Hent", e, "En feil oppsto da metoden prøvde å hente flyplass med ID " + tilFlyplassID);
                    return null;
                }
            }
        }

        public bool LeggInn(Flyplass flyplass)
        {
            using (var db = new DB())
            {
                try
                {

                    var dbflyplass = Mapper.Map<DBFlyplass>(flyplass);
                    if (db.Flyplasser.FirstOrDefault(fly => fly.ID == dbflyplass.ID) == null)
                    {
                        db.Flyplasser.Add(dbflyplass);

                        db.Endringer.Add(new DBEndring()
                        {
                            Tidspunkt = DateTime.Now,
                            Endring = $"Legg til flyplass med ID {dbflyplass.ID}, nye verdier: {dbflyplass.Navn}, {dbflyplass.By}, {dbflyplass.Land}"
                        });

                        db.SaveChanges();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil("DBFlyplass:LeggInn", e, "En feil oppsto da metoden prøvde å legge inn flyplass");

                }
            }
            return false;
        }

        public bool Endre(Flyplass item)
        {
            using (var db = new DB())
            {
                try
                {

                    var dbflyplass = db.Flyplasser.Find(item.ID);
                        
                    if (dbflyplass != null)
                    {
                        dbflyplass.Navn = item.Navn;
                        dbflyplass.By = item.By;
                        dbflyplass.Land = item.Land;


                        db.Endringer.Add(new DBEndring()
                        {
                            Endring = $"Endret flyplass {item.ID}. Nye verdier: {item.Navn}, {item.By}, {item.Land}.",
                            Tidspunkt = DateTime.Now
                        });

                        db.SaveChanges();
                        return true;
                    }
                    db.Endringer.Add(new DBEndring()
                    {
                        Endring = $"Prøvde å endre en flyplass som ikke eksisterte: {item.ID}",
                        Tidspunkt = DateTime.Now
                    });
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil("DBFlyplass:Endre", e, "En feil oppsto da metoden prøvde å legge inn flyplass");

                }
            }
            return false;
        }
    }
}