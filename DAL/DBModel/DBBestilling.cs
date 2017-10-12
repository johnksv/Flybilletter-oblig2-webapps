using AutoMapper;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Flybilletter.DAL.DBModel
{
    public class DBBestilling
    {
        public int ID { get; set; }
        public string Referanse { get; set; } //ID til bestillingen
        public virtual List<DBKunde> Passasjerer { get; set; } //Passasjerer knyttet til en bestilling
        public virtual List<DBFlygning> FlygningerTur { get; set; }
        public virtual List<DBFlygning> FlygningerRetur { get; set; }
        public DateTime BestillingsTidspunkt { get; set; }
        public double Totalpris { get; set; }

        public static Bestilling FinnBestilling(string referanse)
        {
            Bestilling bestilling = null;

            referanse = referanse.ToUpper().Trim();
            var regex = new Regex("^[A-Z0-9]{6}$");
            bool isMatch = regex.IsMatch(referanse);

            if (isMatch)
            {
                using (var db = new DB())
                {

                    DBBestilling dbbestilling = db.Bestillinger.Include("Passasjerer.Poststed").Where(best => best.Referanse == referanse).FirstOrDefault();


                    if (dbbestilling != null)
                    {
                        bestilling = Mapper.Map<Bestilling>(dbbestilling);
                        /* bestilling = new Bestilling()
                        {
                            ID = dbbestilling.ID,
                            BestillingsTidspunkt = dbbestilling.BestillingsTidspunkt,
                            Referanse = dbbestilling.Referanse,
                            Totalpris = dbbestilling.Totalpris
                            //TODO: dbbestilling.FlygningerRetur må gjøres om fra List<DBFlygning> til List<Flygning>, samt List<DBKunde> til List<Kunde>
                            //Kanskje denne er mulig https://stackoverflow.com/questions/1909268/convert-a-list-of-objects-from-one-type-to-another-using-lambda-expression
                            //FlygningerRetur = dbbestilling.FlygningerRetur,
                            //FlygningerTur = dbbestilling.FlygningerTur,
                            //Passasjerer = dbbestilling.Passasjerer,
                        }; */
                    }
                }
            }

            return bestilling;
        }

        public static void LeggInn(Bestilling bestilling)
        {
            using (var db = new DB())
            {
                var dbbestilling = Mapper.Map<DBBestilling>(bestilling);

                foreach (var kunde in bestilling.Passasjerer)
                {
                    DBKunde.LeggInnEllerHentDeretterAttach(kunde);
                }
                db.Bestillinger.Add(dbbestilling);
                db.SaveChanges();
            }
        }

        public static bool EksistererReferanse(string referanse)
        {
            using(var db = new DB())
            {
                return db.Bestillinger.Where(best => best.Referanse == referanse).Any();
            }
        }
    }
}