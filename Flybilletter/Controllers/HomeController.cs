using Flybilletter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Flybilletter.Controllers
{
    public class HomeController : Controller
    {

        private DB db = new DB();

        public ActionResult Sok()
        {
            ViewBag.flyplasser = db.Flyplasser.ToList();

            var model = new SokViewModel()
            {
                Avreise = DateTime.Now.Date,
                Retur = DateTime.Now.Date.AddDays(1)

            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Sok(Flybilletter.Models.SokViewModel innSok)
        {
            bool sammeTilOgFra = innSok.Til.Equals(innSok.Fra);
            var fra = db.Flyplasser.Where(flyplass => flyplass.ID == innSok.Fra).First(); //Hvis du tweaket i HTML-koden fortjener du ikke feilmelding
            var til = db.Flyplasser.Where(flyplass => flyplass.ID == innSok.Til).First();

            FlygningerViewModel reiser = null;

            if (ModelState.IsValid && !sammeTilOgFra && fra != null && til != null)
            {
                reiser = new FlygningerViewModel()
                {
                    TurMuligheter = new List<Reise>(),
                    ReturMuligheter = new List<Reise>(),
                    TurRetur = innSok.Retur.Year >= DateTime.Now.Year
                };
                List<Flygning> fraListe = db.Flygninger.Include("Fly").Where(flygning => flygning.Rute.Fra.ID.Equals(fra.ID)).ToList(); //fly som drar fra reiseplass
                List<Flygning> tilListe = db.Flygninger.Include("Fly").Where(flygning => flygning.Rute.Til.ID.Equals(til.ID)).ToList(); //fly som ender opp i destinasjon
                List <Reise> turListe = new List<Reise>();
                List<Reise> returListe = new List<Reise>();
                foreach (Flygning fraFly in fraListe)
                {
                    if (fraFly.Rute.Til == til)
                    {
                        if (fraFly.AvgangsTid.Date == innSok.Avreise.Date)
                            turListe.Add(new Reise(fraFly));
                    }
                    else
                    {
                        foreach (Flygning tilFly in tilListe)
                        {
                            if (fraFly.Rute.Til == tilFly.Rute.Fra && fraFly.AvgangsTid.Date == innSok.Avreise.Date &&
                                (tilFly.AvgangsTid - fraFly.AnkomstTid) >= new TimeSpan(1, 0, 0))
                            {
                                turListe.Add(new Reise(fraFly, tilFly));
                                break;
                            }
                        }
                    }
                }

                List<Flygning> returFraListe = db.Flygninger.Where(flygning => flygning.Rute.Fra.ID.Equals(til.ID)).ToList();
                List<Flygning> returTilListe = db.Flygninger.Where(flygning => flygning.Rute.Til.ID.Equals(fra.ID)).ToList();

                foreach (Flygning fraFly in returFraListe)
                {
                    if (fraFly.Rute.Til == fra)
                    {
                        if (fraFly.AvgangsTid.Date == innSok.Retur.Date)
                            returListe.Add(new Reise(fraFly));
                    }
                    else
                    {
                        foreach (Flygning tilFly in returTilListe)
                        {
                            if (fraFly.Rute.Til == tilFly.Rute.Fra && fraFly.AvgangsTid.Date == innSok.Retur.Date &&
                                (tilFly.AvgangsTid - fraFly.AnkomstTid) >= new TimeSpan(1, 0, 0))
                            {
                                returListe.Add(new Reise(fraFly, tilFly));
                                break;
                            }
                        }
                    }
                }
                reiser.TurMuligheter.AddRange(turListe.ToList());
                reiser.ReturMuligheter.AddRange(returListe.ToList());

                Session["turListe"] = turListe;
                Session["returListe"] = returListe;
                Session["antallbilletter"] = innSok.AntallBilletter;

            }

            ViewBag.flyplasser = db.Flyplasser.ToList();
            return PartialView("_Flygninger", reiser);
        }



        [HttpPost]
        public ActionResult ValgtReise(string turIndeks, string returIndeks)
        {

            var turListe = (List<Reise>)Session["turListe"];
            var returListe = (List<Reise>)Session["returListe"];
            if (turListe == null || returListe == null) return RedirectToAction("Sok");

            int turIndeksInt = int.Parse(turIndeks);
            int returIndeksInt = -1;
            if (returIndeks != null) returIndeksInt = int.Parse(returIndeks);

            if (turIndeksInt < 0 || turIndeksInt >= turListe.Count) return RedirectToAction("Sok");
            if (returIndeksInt < -1 || returIndeksInt >= returListe.Count) return RedirectToAction("Sok");


            int antallBilletter = (int)Session["antallbilletter"];
            var kunde = new List<Kunde>();
            for (var i = 0; i < antallBilletter; i++)
            {
                kunde.Add(new Kunde());
            }

            var bestillingsdata = new BestillingViewModel()
            {
                Tur = turListe[turIndeksInt],
                Kunder = kunde
            };

            if (returIndeksInt >= 0) bestillingsdata.Retur = returListe[returIndeksInt];

            Session["GjeldendeBestilling"] = bestillingsdata;
            return View("BestillingDetaljer", bestillingsdata);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string Kunde(List<Kunde> Kunder)
        {
            //Hadde vi tatt høyde for kundehåndtering (som er oblig 2) hadde vi håndtert om kunden allerede eksisterte i databasen.
            //Ved kall på denne metoden vet vi at det umidelbart kommer et kall til generer referanse
            Session["KunderBestilling"] = Kunder;
            return "Success";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenererReferanse(BestillingViewModel kredittkortInformasjon)
        {
            if (!ModelState.IsValid)
            {
                return View("BetalingFeilet");
            } else
            {
                string CVCstring = ModelState["Kredittkort.CVC"].Value.AttemptedValue;
                string utlop = ModelState["Kredittkort.Utlop"].Value.AttemptedValue;
                string feilmelding;
                bool gyldig = VerifiserKredittkort(CVCstring, utlop, out feilmelding);
                if (!gyldig)
                {
                    ViewBag.Feilmelding = feilmelding;
                    return View("BetalingFeilet");
                }
            }

            // Generer referanse, lagre i database
            var kunder = (List<Kunde>)Session["KunderBestilling"];
            var dbKunder = DBKunde.LeggInn(kunder);

            //Denne inneholder informasjon om Tur- og Retur-property
            var gjeldende = (BestillingViewModel)Session["GjeldendeBestilling"];

            var bestilling = new Bestilling()
            {
                BestillingsTidspunkt = DateTime.Now,
                FlygningerTur = new List<Flygning>(),
                Passasjerer = dbKunder,
                Totalpris = gjeldende.Totalpris
            };



            do //Lag en unik UUID helt til det ikke finnes i databasen fra før.
            {
                bestilling.Referanse = Guid.NewGuid().ToString().ToUpper().Substring(0, 6);
            } while (db.Bestillinger.Where(best => best.Referanse == bestilling.Referanse).Any());


            //Vi må finne de orginale flygningene i databasen for å unngå exception om "Violation of PRIMARY KEY constraint"
            foreach (var flygning in gjeldende.Tur.Flygninger)
            {
                var dbFlygning = db.Flygninger.Find(flygning.ID);
                if (dbFlygning == null) throw new InvalidOperationException("Ugyldig flygning"); //Det skjedde en feil

                bestilling.FlygningerTur.Add(dbFlygning);
            }

            if (gjeldende.Retur != null)
            {
                bestilling.FlygningerRetur = new List<Flygning>();
                foreach (var flygning in gjeldende.Retur.Flygninger)
                {
                    var dbFlygning = db.Flygninger.Find(flygning.ID);
                    if (dbFlygning == null) throw new InvalidOperationException("Ugyldig flygning");

                    bestilling.FlygningerRetur.Add(dbFlygning);
                }
            }
            foreach(var kunde in dbKunder)
            {
                db.Kunder.Attach(kunde);
            }
            db.Bestillinger.Add(bestilling);

            db.SaveChanges();

            TempData["bestilling"] = bestilling;
            return RedirectToAction("Kvittering");
        }

        public ActionResult Kvittering()
        {
            var bestilling = (Bestilling)TempData["bestilling"];
            return View(bestilling);
        }

        [HttpGet]
        public ActionResult ReferanseSok(string referanse)
        {
            Bestilling bestilling = null;

            referanse = referanse.ToUpper().Trim();
            var regex = new Regex("^[A-Z0-9]{6}$");
            bool isMatch = regex.IsMatch(referanse);

            if (isMatch)
            {
                bestilling = db.Bestillinger.Include("Passasjerer.Poststed").Where(best => best.Referanse == referanse).First();
            }

            return View("BestillingInformasjon", bestilling);
        }

        [HttpGet]
        public string ReferanseEksisterer(string referanse)
        {
            string returString = "{{ \"exists\":\"{0}\", \"url\":\"{1}\" }}";
            if (referanse == null) return string.Format(returString, false, null);

            referanse = referanse.ToUpper().Trim();
            var regex = new Regex("^[A-Z0-9]{6}$");
            bool isMatch = regex.IsMatch(referanse);
            bool exists = false;
            string url = "";

            if (isMatch)
            {
                exists = db.Bestillinger.Where(best => best.Referanse == referanse).Any();
                if (exists) url = "/Home/ReferanseSok?referanse=" + referanse;
            }


            return string.Format(returString, exists, url);
        }

        [HttpGet]
        public string HentPoststed(string postnummer)
        {
            var regex = new Regex("^[0-9]{4}$");
            bool isMatch = regex.IsMatch(postnummer);
            DBPoststed poststed = null;
            if (isMatch)
            {
                poststed = db.Poststeder.FirstOrDefault(model => model.Postnr == postnummer);
            }
            
            return poststed == null ? "null" : poststed.Poststed;
        }


        public ActionResult ReferanseSammendrag(string referanse)
        {
            var bestilling = db.Bestillinger.First(best => best.Referanse.Equals(referanse));

            return View(bestilling);
        }

        private bool VerifiserKredittkort(string CVCstring, string utlop, out string feilmelding)
        {
            feilmelding = "";
            int cvc = 0;
            bool ok = int.TryParse(CVCstring, out cvc);
            if (!ok || cvc < 100 || cvc > 999)
            {
                feilmelding = "Ugyldig CVC. Må være mellom 100 og 999";
                return false;
            }

            
            var regex = new Regex(@"^([0-9]{2})\-([0-9]{2})$");
            Match match = regex.Match(utlop);
            bool resultat = false;
            if (match.Success)
            {
                int mnd = int.Parse(match.Groups[1].Value);
                int aar = int.Parse(match.Groups[2].Value);

                if (mnd > 0 && mnd <= 12) //Sjekk at mnd er OK
                {
                    resultat = true;
                    if (aar < DateTime.Now.Year - 2000) //Sjekk at vi ikke er i fortiden
                    {
                        feilmelding = "Ugyldig utløp. Kortet kan ikke være utløpt.";
                        resultat = false;
                    }
                    else if (aar == DateTime.Now.Year - 2000) //Om kortet går ut samme år som nå sjekker vi at mnd er OK
                    {
                        int currmnd = DateTime.Now.Month;
                        if (mnd < currmnd) 
                        {
                            feilmelding = "Ugyldig utløp. Kortet kan ikke være utløpt.";
                            resultat = false;
                        }
                    }
                }
            }
            
            return resultat;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}