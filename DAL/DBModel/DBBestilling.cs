using AutoMapper;
using DAL;
using Flybilletter.DAL.Interfaces;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Flybilletter.DAL.DBModel
{
    public class DBBestilling : IDBBestilling
    {
        public int ID { get; set; }
        public string Referanse { get; set; } //ID til bestillingen
        public virtual List<DBKunde> Passasjerer { get; set; } //Passasjerer knyttet til en bestilling
        public virtual List<DBFlygning> FlygningerTur { get; set; }
        public virtual List<DBFlygning> FlygningerRetur { get; set; }
        public DateTime BestillingsTidspunkt { get; set; }
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
                        bestilling = Mapper.Map<Bestilling>(db.Bestillinger.Include("Passasjerer.Postnummer").Where(best => best.Referanse == referanse).FirstOrDefault());
                    }
                }
            }
            catch (Exception e)
            {
                DALsetup.logFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
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
                    DALsetup.logFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
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
                    DALsetup.logFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
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
    }
}