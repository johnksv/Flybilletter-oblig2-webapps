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
    public class DBKunde : IDBKunde
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
            catch (Exception e)
            {
                DALsetup.LogFeilTilFil("DBKunde:LeggInn", e, "En feil oppsto da metoden prøvde å legge inn kunder.");
                return false;
            }

            return true;
        }

        public DBKunde LeggInn(Kunde innKunde)
        {
            using (var db = new DB())
            {
                try
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
                        kunde.Postnummer = new DBPostnummer
                        {
                            Postnr = innKunde.Postnr,
                            Poststed = ""
                        };
                    }
                    else
                    {
                        kunde.Postnummer = poststed;
                        db.Poststeder.Attach(poststed);
                    }
                    var endring = new DBEndring()
                    {
                        Tidspunkt = DateTime.Now,
                        Endring = $"La til ny kunde: {kunde.Fornavn} {kunde.Etternavn}, {kunde.Adresse}, {kunde.Postnummer.Postnr} {kunde.Postnummer.Poststed}"
                    };
                    db.Endringer.Add(endring);
                    db.SaveChanges();

                    return kunde;
                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil("DBKunde:LeggInn", e, "En feil oppsto da metoden prøvde å legge inn ny kunde.");
                    return null;
                }
            }
        }

        public List<Kunde> HentAlle()
        {
            using (var db = new DB())
            {
                try
                {
                    var dbkunder = db.Kunder.Include("Postnummer").ToList();
                    var kunder = new List<Kunde>();
                    foreach (var kunde in dbkunder)
                    {
                        var domenekunde = Mapper.Map<Kunde>(kunde);

                        domenekunde.Postnr = kunde.Postnummer.Postnr;
                        domenekunde.Poststed = kunde.Postnummer.Poststed;

                        kunder.Add(domenekunde);

                    }
                    return kunder;
                }catch(Exception e)
                {
                    DALsetup.LogFeilTilFil("DBKunde:HentAlle", e, "En feil oppsto da metoden prøvde å hente alle kundene.");
                    return null;
                }
            }
        }

        public Kunde Hent(int id)
        {
            using (var db = new DB())
            {
                try
                {
                    var dbkunde = db.Kunder.Include("Postnummer").Where(kunde => kunde.ID == id).FirstOrDefault();
                    Kunde kund = Mapper.Map<Kunde>(dbkunde);
                    kund.Postnr = dbkunde.Postnummer.Postnr;
                    kund.Poststed = dbkunde.Postnummer.Poststed;
                    return kund;
                }catch(Exception e)
                {
                    DALsetup.LogFeilTilFil("DBKunde:Hent", e, "En feil oppsto da metoden prøvde å hente en kunde.");
                    return null;
                }

            }
        }

        public bool Endre(Kunde kunde)
        {
            using (var db = new DB())
            {
                try
                {
                    // DB oppdater
                    var dbkundeEntitet = db.Kunder.Find(kunde.ID);
                    dbkundeEntitet.Fornavn = kunde.Fornavn;
                    dbkundeEntitet.Etternavn = kunde.Etternavn;
                    dbkundeEntitet.Fodselsdag = kunde.Fodselsdag;
                    dbkundeEntitet.Tlf = kunde.Tlf;
                    dbkundeEntitet.Adresse = kunde.Adresse;
                    dbkundeEntitet.EPost = kunde.EPost;
                    dbkundeEntitet.Postnummer = db.Poststeder.Find(kunde.Postnr);
                    db.Endringer.Add(new DBEndring()
                    {
                        Tidspunkt = DateTime.Now,
                        Endring = $"Endret på kunde {kunde.ID}, nye verdier: {kunde.Fornavn} {kunde.Etternavn}, {kunde.Adresse}, {kunde.Postnr} {kunde.Poststed}"
                    });
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil("DBKunde:Endre", e, $"En feil oppsto da metoden prøvde å oppdatere kunde.");
                    return false;
                }
            }
        }

        public bool Slett(int id)
        {
            using (var db = new DB())
            {
                try
                {
                    var kunde = db.Kunder.Include("Postnummer").Where(kund => kund.ID == id).FirstOrDefault();
                    if (kunde != null)
                    {
                        
                        if (kunde.Bestillinger.Count() == 0)
                        {

                            db.Kunder.Remove(kunde);
                            db.Endringer.Add(new DBEndring()
                            {
                                Tidspunkt = DateTime.Now,
                                Endring = $"Slettet kunde {kunde.Fornavn} {kunde.Etternavn}, ID {kunde.ID}."
                            });
                            db.SaveChanges();
                            return true;
                        }

                        db.Endringer.Add(new DBEndring()
                        {
                            Tidspunkt = DateTime.Now,
                            Endring = $"Prøvde å slette kunde {kunde.Fornavn} {kunde.Etternavn}, ID {kunde.ID}, men hadde bestillinger tilknyttet."
                        });
                        db.SaveChanges();

                    }

                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil("DBKunde:Slett", e, "En feil oppsto da metoden prøvde å slette kunde.");
                }
                return false;

            }
        }
    }

}