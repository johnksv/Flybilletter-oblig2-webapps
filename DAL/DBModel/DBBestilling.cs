using AutoMapper;
using DAL;
using Flybilletter.DAL.Interfaces;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;

namespace Flybilletter.DAL.DBModel
{
    public class DBBestilling : IDBBestilling
    {
        [Key]
        public string Referanse { get; set; } //ID til bestillingen
        public virtual List<DBKunde> Passasjerer { get; set; } //Passasjerer knyttet til en bestilling
        public virtual List<DBFlygning> FlygningerTur { get; set; }
        public virtual List<DBFlygning> FlygningerRetur { get; set; }
        public DateTime Bestillingstidspunkt { get; set; }
        public double Totalpris { get; set; }

        public Bestilling FinnBestilling(string referanse)
        {
            Bestilling bestilling = null;
            if (referanse == null) return bestilling;

            referanse = referanse.ToUpper().Trim();
            var regex = new Regex("^[A-Z0-9]{6}$");
            bool isMatch = regex.IsMatch(referanse);
            try
            {
                if (isMatch)
                {
                    using (var db = new DB())
                    {
                        var dbbestilling = db.Bestillinger.Include("Passasjerer.Postnummer.Postnr").Where(best => best.Referanse == referanse).FirstOrDefault();
                        bestilling = Mapper.Map<Bestilling>(dbbestilling);
                    }
                }
            }
            catch (Exception e)
            {
                DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e, "En feil oppsto når metoden prøvde å finne bestilling med referanse " + referanse);
            }

            return bestilling;
        }

        public void LeggInn(Bestilling bestilling)
        {
            using (var db = new DB())
            {
                var dbbestilling = Mapper.Map<DBBestilling>(bestilling);
                var a = db.Entry(dbbestilling);

                List<DBKunde> kunder = new List<DBKunde>();
                DBKunde dbKunde = new DBKunde();
                foreach (var kunde in bestilling.Passasjerer)
                {
                    kunder.Add(dbKunde.LeggInn(kunde));
                    db.Kunder.Attach(kunder.Last());
                }
                dbbestilling.Passasjerer = kunder;

                //MÅ mappes automatisk for å få riktig entity framework relasjon.... attach, samt mye annet ville ikke fungere...
                List<DBFlygning> flygninger = new List<DBFlygning>();
                foreach (var flygning in dbbestilling.FlygningerTur)
                {
                    flygninger.Add(db.Flygninger.Find(flygning.ID));
                }
                dbbestilling.FlygningerTur = flygninger;


                flygninger = new List<DBFlygning>();
                foreach (var flygning in dbbestilling.FlygningerRetur)
                {
                    flygninger.Add(db.Flygninger.Find(flygning.ID));
                }
                dbbestilling.FlygningerRetur = flygninger;

                try
                {
                    db.Bestillinger.Add(dbbestilling);
                    var endring = new DBEndring()
                    {
                        Tidspunkt = DateTime.Now,
                        Endring = "La til bestilling: " + dbbestilling.Referanse
                    };
                    db.Endringer.Add(endring);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e, "En feil oppsto når metoden prøvde å legge inn bestillingen i databasen.");
                }

            }
        }

        public bool EksistererReferanse(string referanse)
        {
            using (var db = new DB())
            {
                try
                {
                    return db.Bestillinger.Where(best => best.Referanse == referanse).Any();
                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e, "En feil oppsto når metoden prøvde å finne ut om referansen " + referanse + " eksisterer.");
                }
                return false;
            }
        }

        public List<Bestilling> HentAlle()
        {
            using (var db = new DB())
            {
                var dbbestillinger = db.Bestillinger.ToList();
                var bestillinger = new List<Bestilling>();
                foreach (var best in dbbestillinger)
                {
                    bestillinger.Add(Mapper.Map<Bestilling>(best));
                }

                return bestillinger;
            }
        }

        public bool Slett(string referanse)
        {
            using (var db = new DB())
            {
                var dbbestilling = db.Bestillinger.Include("FlygningerTur").Include("FlygningerRetur").FirstOrDefault(best => best.Referanse == referanse);
                if (dbbestilling != null)
                {
                    db.Bestillinger.Remove(dbbestilling);

                    db.Endringer.Add(new DBEndring()
                    {
                        Endring = "Slett bestilling med referanse " + referanse,
                        Tidspunkt = DateTime.Now
                    });


                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        DALsetup.LogFeilTilFil("DBBestilling:Slett", e, "En feil oppsto da metoden prøvde å slette bestilling med referanse" + referanse);
                        return false;
                    }

                    return true;
                }
                return false;

            }
        }
    }
}