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

        public virtual DBPostnummer Postnummer { get; set; }

        public virtual List<DBBestilling> Bestillinger { get; set; }



        public static List<Kunde> hentAlle()
        {
            List<Kunde> kunder = null;
            using (var db = new DB())
            {
                if (db.Kunder.Any())
                {
                    kunder = new List<Kunde>();
                    db.Kunder.ToList().ForEach(kunde =>
                    {
                        kunder.Add(Mapper.Map<Kunde>(kunde));
                    });
                }
            }
            return kunder;
        }

        public static bool LeggInn(IEnumerable<Kunde> kunder)
        {
            try
            {
                foreach (var kunde in kunder)
                {
                    LeggInn(kunde);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static DBKunde LeggInn(Kunde innKunde)
        {
            using (var db = new DB())
            {
                DBKunde eksisterendeKunde = db.Kunder.Where(k => k.Etternavn == innKunde.Etternavn && k.Fornavn == innKunde.Fornavn && k.Tlf == innKunde.Tlf).FirstOrDefault();
                if (eksisterendeKunde != null)
                {
                    return eksisterendeKunde;
                }

                DBKunde kunde = Mapper.Map<DBKunde>(innKunde);
                db.Kunder.Add(kunde);

                var poststed = db.Poststeder.Find(innKunde.Postnummer.Postnr);
                if (poststed == null)
                {
                    db.Poststeder.Attach(kunde.Postnummer);
                }else
                {
                    kunde.Postnummer = poststed;
                    db.Poststeder.Attach(kunde.Postnummer);
                }

                db.SaveChanges();
                return kunde;
            }
        }

        public static Kunde HentKunde(int kundeID)
        {
            using (var db = new DB())
            {
                return Mapper.Map<Kunde>(db.Kunder.Find(kundeID));
            }

        }
    }

}