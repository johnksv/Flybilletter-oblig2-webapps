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
        private IBLLFly bllfly;
        private IBLLKunde bllkunde;
        private IBLLFlygning bllflygning;
        private IBLLFlyplass bllflyplass;
        private IBLLRute bllrute;

        public AdminController() : this(new BLLBestilling(), new BLLFly(), new BLLKunde(), new BLLFlyplass(), new BLLFlygning(), new BLLRute())
        {
        }

        public AdminController(IBLLBestilling bestillingstub, IBLLFly flystub, IBLLKunde kundestub, IBLLFlyplass flyplasstub, IBLLFlygning flygningstub, IBLLRute rutestub)

        {
            bllbestilling = bestillingstub;
            bllfly = flystub;
            bllkunde = kundestub;
            bllflygning = flygningstub;
            bllflyplass = flyplasstub;
            bllrute = rutestub;
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


        public ActionResult Fly()
        {
            if (ErAdmin())
            {
                List<Fly> fly = bllfly.HentAlle();
                return View("ListFly", fly);
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

        public ActionResult RedigerFly(int ID)
        {
            if (ErAdmin())
            {
                var fly = bllfly.Hent(ID);
                if (fly != null) return View("RedigerFly", fly);
            }
            return RedirectToAction("Sok", "Home");
        }

        [HttpPost]
        public ActionResult RedigerFly(Fly fly)
        {
            if (ErAdmin())
            {
                if (bllfly.Oppdater(fly))
                {
                    List<Fly> flyListe = bllfly.HentAlle();
                    return RedirectToAction("Fly", flyListe);
                }
            }
            return RedirectToAction("Sok", "Home");
        }

        public ActionResult SlettFly(int ID)
        {
            if (ErAdmin())
            {
                var fly = bllfly.Hent(ID);
                if (fly != null) return View("SlettFly", fly);
            }
            return RedirectToAction("Sok", "Home");
        }

        [HttpPost]
        public bool SlettFly(int ID, Fly fly)
        {
            if (ErAdmin())
            {
                return bllfly.Slett(ID);
            }
            return false;
        }

        public ActionResult LagFly()
        {
            if (ErAdmin())
            {
                return View();
            }
            return RedirectToAction("Sok", "Home");
        }

        [HttpPost]
        public ActionResult LagFly(Fly fly)
        {
            if (ErAdmin())
            {
                if (bllfly.LeggTil(fly))
                {
                    List<Fly> alleFly = bllfly.HentAlle();
                    return RedirectToAction("Fly");
                }
            }
            return RedirectToAction("Sok", "Home");
        }

        public bool SlettBestilling(string referanse)
        {
            if (ErAdmin())
            {
                return bllbestilling.Slett(referanse);

            }
            return false;
        }

        public ActionResult Flyplasser()
        {
            if (ErAdmin())
            {
                var model = bllflyplass.HentAlle();
                return View("ListFlyplasser", model);
            }
            return RedirectToAction("Sok", "Home");
        }

        public ActionResult NyFlyplass()
        {
            if (ErAdmin())
            {
                return View("NyFlyplass");
            }
            return RedirectToAction("Sok", "Home");
        }

        [HttpPost]
        public ActionResult NyFlyplass(Flyplass flyplass)
        {
            if (ErAdmin())
            {
                if (ModelState.IsValid)
                {
                    bllflyplass.LeggInn(flyplass);
                }   
                return RedirectToAction("Flyplasser");
            }
            return RedirectToAction("Sok", "Home");
        }

        public ActionResult EndreFlyplass(string id)
        {
            if (ErAdmin())
            {
                var model = bllflyplass.Hent(id);
                if(model != null)
                {
                    return View("EndreFlyplass", model);
                }
                ViewBag.Feilmelding = "Fant ikke flyplass med ID " + id + " i databasen";
                //TODO: redirect til feilmelding
            }
            return RedirectToAction("Sok", "Home");
        }

        [HttpPost]
        public ActionResult EndreFlyplass(Flyplass flyplass)
        {
            if (ErAdmin())
            {
            }
            return RedirectToAction("Sok", "Home");
        }

        public ActionResult Ruter()
        {
            if (ErAdmin())
            {
                var model = bllrute.HentAlle();
                return View("ListRuter", model);
            }
            return RedirectToAction("Sok", "Home");
        }

        public ActionResult LagRute()
        {
            if (ErAdmin())
            {
                return View();
            }
            return RedirectToAction("Sok", "Home");
        }
        [HttpPost]
        public ActionResult LagreRute(Rute rute)
        {
            if (ErAdmin())
            {
                if (ModelState.IsValid) 
                {

                }
                return View(rute);
            }
            return RedirectToAction("Sok", "Home");
        }

        public ActionResult SlettRute(int id)
        {
            if (ErAdmin())
            {
                if (bllrute.Slett(id))
                {
                    return RedirectToAction("Ruter");
                }
                ViewBag.Feilmelding = "Klarte ikke slette rute fra i databasen. Mulig den har flygninger relatert til seg.";

            }
            return RedirectToAction("Sok", "Home");
        }
      

        public ActionResult Kunder()
        {
            if (ErAdmin())
            {
                List<Kunde> kunder = bllkunde.HentAlle();
                return View("ListKunder", kunder);
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
                return RedirectToAction("Kunder");
            else return RedirectToAction("Sok", "Home");
        }

        public ActionResult EndreKunde(int id)
        {
            if (ErAdmin())
            {
                var kunde = bllkunde.HentEnKunde(id);
                return View("RedigerKunde", kunde);
            }
            return RedirectToAction("Sok", "Home");
        }

        public ActionResult OppdaterKunde(Kunde kunde)
        {
            if (ErAdmin())
            {
                bllkunde.Oppdater(kunde);
                return RedirectToAction("Kunder");
            }
            return RedirectToAction("Sok", "Home");
        }

        public ActionResult SlettKunde(int id)
        {
            if (ErAdmin())
            {
                bllkunde.SlettKunde(id);
            }
            return RedirectToAction("Kunder");
        }

        public ActionResult Flygninger()
        {
            if (ErAdmin())
            {
                var flygninger = bllflygning.HentAlle(DateTime.Now);
                return View("ListFlygning", flygninger);
            }

            return RedirectToAction("Sok", "Home");
        }

        public ActionResult EndreFlygning(int id)
        {
            if (ErAdmin())
            {
                var flygning = bllflygning.HentEnFlygning(id);
                return View("RedigerFlygning", flygning);
            }
            return RedirectToAction("Sok", "Home");
        }

        public ActionResult OppdaterFlygning(Flygning flygning)
        {
            if (ErAdmin())
            {
                bllflygning.OppdaterFlygning(flygning);
                return RedirectToAction("Flygninger");
            }
            return RedirectToAction("Sok", "Home");
        }

        public ActionResult LagFlygning()
        {
            if (ErAdmin())
            {
                ViewBag.ruter = bllrute.HentAlle();
                ViewBag.fly = bllfly.HentAlle();
                return View("LagFlygning");
            }
            return RedirectToAction("Sok", "Home");
        }

        public ActionResult LagreFlygning(Flygning flygning, int ruteID, int flyID)
        {
            if (ErAdmin())
            {
                bllfly.Hent(flyID);
                bllrute.Hent(ruteID);
                bllflygning.LeggInnFlygning(flygning);
                return RedirectToAction("Flygninger");
            }
            return RedirectToAction("Sok", "Home");
        }

        public ActionResult OppdaterStatusFlygning(int id)
        {
            if (ErAdmin())
            {
                bllflygning.OppdaterStatus(id);
                return RedirectToAction("Flygninger");
            }
            return RedirectToAction("Sok", "Home");
        }

        private bool ErAdmin()
        {
            return Session["Admin"] != null && (bool)Session["Admin"] == true;
        }

        public ActionResult LoggUt()
        {
            Session["Admin"] = null;
            return RedirectToAction("Sok", "Home");
        }

    }
}