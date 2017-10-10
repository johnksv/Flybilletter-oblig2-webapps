using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flybilletter.Models
{

    public class DBPoststed
    {
        [Key]
        public string Postnr { get; set; }
        public string Poststed { get; set; }

        public virtual List<DBKunde> Kunder { get; set; }
    }

    public class DBKunde
    {
        public int ID { get; set; }

        public string Fornavn { get; set; }

        public string Etternavn { get; set; }

        public DateTime Fodselsdag { get; set; }

        public string Adresse { get; set; }

        public string Tlf { get; set; }

        public string EPost { get; set; }

        public DBPoststed Poststed { get; set; }



        public static List<Kunde> hentAlle()
        {
            List<Kunde> kunder = null;
            using (var db = new DB())
            {
                kunder = db.Kunder.Select(kunde => new Kunde()
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
                }).ToList();
            }
            return kunder;
        }

        public static List<DBKunde> LeggInn(IEnumerable<Kunde> kunder)
        {
            var dbKunder = new List<DBKunde>(kunder.Count());
            foreach (var kunde in kunder)
            {
                dbKunder.Add(LeggInn(kunde));
            }
            return dbKunder;
        }

            public static DBKunde LeggInn(Kunde innKunde)
        {
            using (var db = new DB())
            {
                var nyKunde = new DBKunde()
                {
                    Fornavn = innKunde.Fornavn,
                    Etternavn = innKunde.Etternavn,
                    Adresse = innKunde.Adresse,
                    Fodselsdag = innKunde.Fodselsdag,
                    EPost = innKunde.EPost,
                    Tlf = innKunde.Tlf
                };

                var poststed = db.Poststeder.Find(innKunde.Postnummer);
                if(poststed == null) //Antar at posten har byttet postnummer på denne plassen
                {
                    poststed = new DBPoststed()
                    {
                        Postnr= innKunde.Postnummer,
                        Poststed = ""
                    };
                    
                }else
                {
                    db.Poststeder.Attach(poststed);
                }

                
                nyKunde.Poststed = poststed;
                
                db.Kunder.Add(nyKunde);
                db.SaveChanges();
                return nyKunde;
            }
        }
  
        public Kunde HentKundePaaID(int kundeID)
        {
            using (var db = new DB())
            {
                var dbKunde = db.Kunder.Where(kund => kund.ID == kundeID).FirstOrDefault();
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