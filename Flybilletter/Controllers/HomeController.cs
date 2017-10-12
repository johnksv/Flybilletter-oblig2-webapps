using BLL;
using Flybilletter.Model.DomeneModel;
using Flybilletter.Model.ViewModel;
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

        public ActionResult Sok()
        {
            ViewBag.flyplasser = BLLFlyplass.HentAlle();

            var model = new SokViewModel()
            {
                Avreise = DateTime.Now.Date,
                Retur = DateTime.Now.Date.AddDays(1)

            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Sok(SokViewModel innSok)
        {
            bool sammeTilOgFra = innSok.Til.Equals(innSok.Fra);

            FlygningerViewModel reiser = null;

            if (ModelState.IsValid && !sammeTilOgFra)
            {
                string fraFlyplass = innSok.Fra;
                string tilFlyplass = innSok.Til;

                List<Reise> flygningerTur = BLLFlygning.FinnReiseforslag(fraFlyplass, tilFlyplass, innSok.Avreise);
                List<Reise> flygningerRetur = BLLFlygning.FinnReiseforslag(tilFlyplass, fraFlyplass, innSok.Retur);

                reiser = new FlygningerViewModel()
                {
                    TurMuligheter = flygningerTur,
                    ReturMuligheter = flygningerRetur,
                    TurRetur = innSok.Retur >= innSok.Avreise
                };

                Session["turListe"] = flygningerTur;
                Session["returListe"] = flygningerRetur;
                Session["antallbilletter"] = innSok.AntallBilletter;

            }

            ViewBag.flyplasser = BLLFlyplass.HentAlle();
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
            }
            else
            {
                string CVCstring = ModelState["Kredittkort.CVC"].Value.AttemptedValue;
                string utlop = ModelState["Kredittkort.Utlop"].Value.AttemptedValue;
                string feilmelding;
                bool gyldig = BLLBestilling.VerifiserKredittkort(CVCstring, utlop, out feilmelding);
                if (!gyldig)
                {
                    ViewBag.Feilmelding = feilmelding;
                    return View("BetalingFeilet");
                }
            }


            var kunder = (List<Kunde>)Session["KunderBestilling"];
            //Denne inneholder informasjon om Tur- og Retur-property
            var gjeldende = (BestillingViewModel)Session["GjeldendeBestilling"];

            TempData["bestilling"] = BLLBestilling.LeggInn(kunder, gjeldende);
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
            Bestilling bestilling = BLLBestilling.FinnBestilling(referanse);
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
                exists = BLLBestilling.EksistererReferanse(referanse);
                if (exists) url = "/Home/ReferanseSok?referanse=" + referanse;
            }


            return string.Format(returString, exists, url);
        }

        [HttpGet]
        public string HentPoststed(string postnummer)
        {
            return BLLKunde.HentPoststed(postnummer);
        }
    }
}