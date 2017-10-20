﻿using AutoMapper;
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

        public DBPostnummer Postnummer { get; set; }

        public virtual List<DBBestilling> Bestillinger { get; set; }


        public bool LeggInn(IEnumerable<Kunde> kunder)
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

        public DBKunde LeggInn(Kunde innKunde)
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

                var poststed = db.Poststeder.Find(innKunde.Postnr);
                
                if (poststed == null) //Hvis postnummeret finnes attacher vi det med databasen.
                {
                    kunde.Postnummer = new DBPostnummer {
                        Postnr = innKunde.Postnr,
                        Poststed = ""
                    };
                    
                }else
                {
                    kunde.Postnummer = poststed;
                    db.Poststeder.Attach(poststed);
                }

                db.SaveChanges();
                return kunde;
            }
        }
    }

}