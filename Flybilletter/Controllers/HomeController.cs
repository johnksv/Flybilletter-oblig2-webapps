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

        private IBLLFlyplass bllflyplass;
        private IBLLFlygning bllflygning;
        private IBLLBestilling bllbestilling;
        private IBLLKunde bllkunde;

        public HomeController()
        {
            bllflyplass = new BLLFlyplass();
            bllflygning = new BLLFlygning();
            bllbestilling = new BLLBestilling();
            bllkunde = new BLLKunde();
        }

        public HomeController(BLLFlyplass bllflyplassStub, BLLFlygning bllflygningStub, BLLBestilling bllbestillingStub, BLLKunde bllkundeStub)
        {
            this.bllflyplass = bllflyplassStub;
            this.bllflygning = bllflygningStub;
            this.bllbestilling = bllbestillingStub;
            this.bllkunde = bllkundeStub;
        }

        public ActionResult Sok()
        {
            
            ViewBag.flyplasser = bllflyplass.HentAlle();

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
            bool sammeTilOgFra = innSok.Til == innSok.Fra;

            FlygningerViewModel reiser = null;

            if (ModelState.IsValid && !sammeTilOgFra)
            {

                bllflygning.FinnReisemuligheter(innSok, out reiser, out List<Reise> flygningerTur, out List<Reise> flygningerRetur);

                Session["turListe"] = flygningerTur;
                Session["returListe"] = flygningerRetur;
                Session["antallbilletter"] = innSok.AntallBilletter;

            }

            ViewBag.flyplasser = bllflyplass.HentAlle();
            return PartialView("_Flygninger", reiser);
        }


        [HttpPost]
        public ActionResult ValgtReise(string turIndeks, string returIndeks)
        {

            var turListe = (List<Reise>)Session["turListe"];
            var returListe = (List<Reise>)Session["returListe"];
            if (turListe == null || returListe == null) return RedirectToAction("Sok");

            int turIndeksInt = int.Parse(turIndeks);
            int returIndeksInt = -1; //Hvis returindeks er -1 antar vi at man kun skal ha en-veis-tur
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
            //Ved kall på denne metoden vet vi at det umidelbart kommer et kall til generer referanse
            
            if (ModelState.IsValid)
            {
                Session["KunderBestilling"] = Kunder;
                return "success";
            }
            return "En eller flere kunder har ugyldig state. Sjekk informasjonen på nytt.";
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
                string CVCstring = kredittkortInformasjon.Kredittkort.CVC + "";
                string utlop = kredittkortInformasjon.Kredittkort.Utlop;
                string feilmelding;
                bool gyldig = bllbestilling.VerifiserKredittkort(CVCstring, utlop, out feilmelding);
                if (!gyldig)
                {
                    ViewBag.Feilmelding = feilmelding;
                    return View("BetalingFeilet");
                }
            }

            var kunder = (List<Kunde>)Session["KunderBestilling"];
            //Denne inneholder informasjon om Tur- og Retur-property
            var gjeldende = (BestillingViewModel)Session["GjeldendeBestilling"];

            TempData["bestilling"] = bllbestilling.LeggInn(kunder, gjeldende);
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
            Bestilling bestilling = bllbestilling.FinnBestilling(referanse);
            return View("BestillingInformasjon", bestilling);
        }

        [HttpGet]
        public string ReferanseEksisterer(string referanse)
        {
            return bllbestilling.EksistererReferanse("/Home/ReferanseSok?referanse=", referanse);
        }

        public bool SlettBestilling(string referanse)
        {
            return bllbestilling.SlettSomKunde(referanse);
        }


        [HttpGet]
        public string HentPoststed(string postnummer)
        {
            return bllkunde.HentPoststed(postnummer);
        }

        
    }
}