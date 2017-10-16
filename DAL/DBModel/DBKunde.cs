using AutoMapper;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flybilletter.DAL.DBModel
{

    public class DBKunde
    {
        public int ID { get; set; }

        public string Fornavn { get; set; }

        public string Etternavn { get; set; }

        public DateTime Fodselsdag { get; set; }

        public string Adresse { get; set; }

        public string Tlf { get; set; }

        public string EPost { get; set; }

        public virtual DBPoststed Poststed { get; set; }

        public virtual List<DBBestilling> Bestillinger { get; set; }



        public static List<Kunde> hentAlle()
        {
            List<Kunde> kunder = null;
            using (var db = new DB())
            {
                /*kunder = db.Kunder.Select(kunde => new Kunde()
                {
                    ID = kunde.ID,
                    Fornavn = kunde.Fornavn,
                    Etternavn = kunde.Etternavn,
                    Adresse = kunde.Adresse,
                    Postnummer = kunde.Poststed.Postnr,
                    Poststed = kunde.Poststed.Poststed,
                    Tlf = kunde.Tlf,
                    EPost = kunde.EPost,
                    Fodselsdag = kunde.Fodselsdag
                }).ToList(); */
                if (db.Kunder.Any())
                {
                    kunder = new List<Kunde>();
                    db.Kunder.ToList().ForEach(kunde => {
                        kunder.Add(Mapper.Map<Kunde>(kunde));
                    });
                } 
            }
            return kunder;
        }

        public static bool LeggInn(IEnumerable<Kunde> kunder)
        {
            bool result = true;
            foreach (var kunde in kunder)
            {
                bool current = LeggInn(kunde);
                result = result == true ? current : false;
            }

            return result;
        }

        private static DBKunde KundeTilDBKunde(Kunde innKunde, out bool postnummerEksisterte)
        {
            DBKunde dbKunde = null;

            using (var db = new DB())
            {
                dbKunde = new DBKunde()
                {
                    Fornavn = innKunde.Fornavn,
                    Etternavn = innKunde.Etternavn,
                    Adresse = innKunde.Adresse,
                    Fodselsdag = innKunde.Fodselsdag,
                    EPost = innKunde.EPost,
                    Tlf = innKunde.Tlf
                };

                var poststed = db.Poststeder.Find(innKunde.Postnummer);

                postnummerEksisterte = poststed != null;

                if (!postnummerEksisterte) //Antar at posten har byttet postnummer på denne plassen
                {
                    postnummerEksisterte = false;
                    poststed = new DBPoststed()
                    {
                        Postnr = innKunde.Postnummer,
                        Poststed = ""
                    };

                }

                dbKunde.Poststed = poststed;
            }
            return dbKunde;
        }

        public static bool LeggInn(Kunde innKunde)
        {
            try
            {
                using (var db = new DB())
                {
                    var dbKunde = KundeTilDBKunde(innKunde, out bool postnummerIkkeEksisterte);

                    if (postnummerIkkeEksisterte)
                    {
                        db.Poststeder.Attach(dbKunde.Poststed);
                    }

                    db.Kunder.Add(dbKunde);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        internal static void LeggInnEllerHentDeretterAttach(Kunde innKunde)
        {
            using (var db = new DB())
            {
                DBKunde kunde = db.Kunder.Find(innKunde.ID);
                if (kunde == null)
                {
                    LeggInn(innKunde);
                    kunde = db.Kunder.Find(innKunde.ID);
                }

                db.Kunder.Attach(kunde);
            }
            
        }

        public static Kunde HentKunde(int kundeID)
        {
            using (var db = new DB())
            {
                var dbKunde = db.Kunder.Find(kundeID);
                if (dbKunde == null)
                {
                    return null;
                }

                var kunde = new Kunde()
                {
                    ID = dbKunde.ID,
                    Fornavn = dbKunde.Fornavn,
                    Etternavn = dbKunde.Etternavn,
                    Adresse = dbKunde.Adresse,
                    Postnummer = dbKunde.Poststed.Postnr,
                    Poststed = dbKunde.Poststed.Poststed
                };

                return kunde;
            }

        }
    }

}