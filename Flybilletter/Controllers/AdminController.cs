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
    public class AdminController : Controller
    {
        private IBLLBestilling bllbestilling;
        private IBLLKunde bllkunde;

        public AdminController()
        {
            bllbestilling = new BLLBestilling();
            bllkunde = new BLLKunde();
        }

        public AdminController(IBLLBestilling ibllbestilling, IBLLKunde ibllkunde)
        {
            bllbestilling = ibllbestilling;
            bllkunde = ibllkunde;
        }

        [HttpGet]
        public string LoginAttempt(string username, string password)
        {
            string returString = "{{ \"gyldig\":\"{0}\", \"url\":\"{1}\" }}";
            string url = "";
            bool gyldig = false;
            BLLAdmin bllAdmin = new BLLAdmin();

            if (bllAdmin.IsPassordGyldig(username, password))
            {
                // Login er gyldig, session variabel settes
                Session["Admin"] = true;
                gyldig = true;
                url = "/Home/Sok";
            }

            return string.Format(returString, gyldig, url); ;
        }

        [HttpGet]
        public ActionResult Bestillinger()
        {
            if (ErAdmin())
            {
                List<Bestilling> bestillinger = bllbestilling.HentAlle();
                return View("BestillingListe", bestillinger);
            }
            return RedirectToAction("Sok", "Home");
        }

        public ActionResult SeDetaljerBestilling(string referanse)
        {
            if (ErAdmin())
            {
                return RedirectToAction("ReferanseSok", "Home", new { referanse = referanse });
            }
            return RedirectToAction("Sok", "Home");
        }

        public ActionResult SlettBestilling(string referanse)
        {
            if (ErAdmin())
            {
               bllbestilling.Slett(referanse);
               return RedirectToAction("Bestillinger");
            }
            return RedirectToAction("Sok", "Home");
        }

        public ActionResult Kunder()
        {
            if (ErAdmin())
            {
                List<Kunde> kunder = bllkunde.HentAlle();
                return View("KundeListe", kunder);
            }
            return RedirectToAction("Sok", "Home");
        }
        public ActionResult LagKunde()
        {
            if (ErAdmin())
            {
                return View("LagKunde");
            }
            return RedirectToAction("Sok", "Home");
        }

        [HttpPost]
        public ActionResult LagreKunde(Kunde kunde)
        {
            if (bllkunde.LeggInn(kunde))
                return Kunder();
            else return RedirectToAction("Sok", "Home");
        }

        public ActionResult EndreKunde()
        {
            if (ErAdmin())
            {

            }
            return RedirectToAction("Sok", "Home");
        }
        public ActionResult DetaljerKunde()
        {
            if (ErAdmin())
            {

            }
            return RedirectToAction("Sok", "Home");
        }

        private bool ErAdmin()
        {
            return Session["Admin"] != null && (bool)Session["Admin"] == true;
        }

    }
}