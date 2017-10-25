﻿using BLL;
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
        private IBLLFlyplass bllflyplass;

        public AdminController()
        {
            bllbestilling = new BLLBestilling();
            bllfly = new BLLFly();
            bllkunde = new BLLKunde();
            bllflyplass = new BLLFlyplass();
        }

        public AdminController(IBLLBestilling bestillingstub, IBLLFly flystub, IBLLKunde kundestub, IBLLFlyplass flyplasstub)
        {
            bllbestilling = bestillingstub;
            bllfly = flystub;
            bllkunde = kundestub;
            bllflyplass = flyplasstub;
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
                return View("FlyListe", fly);
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
                if (fly == null) RedirectToAction("Sok", "Home");
                return View("RedigerFly", fly);
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

        public ActionResult LagFlyplasser()
        {
            if (ErAdmin())
            {
                return View("NyFlyplass");
            }
            return RedirectToAction("Sok", "Home");
        }

        [HttpPost]
        public ActionResult LagFlyplasser(Flyplass flyplass)
        {
            if (ErAdmin())
            {
                if (bllflyplass.LeggInn(flyplass))
                    return RedirectToAction("Flyplasser");
            }
            return RedirectToAction("Sok", "Home");
        }

        public ActionResult EndreFlyplass(string id)
        {
            if (ErAdmin())
            {
                return View("EndreFlyplass");
            }
            return RedirectToAction("Sok", "Home");
        }
        public ActionResult OppdaterFlyplass(Flyplass flyplass)
        {
            if (ErAdmin())
            {
            }
            return RedirectToAction("Sok", "Home");
        }

        public ActionResult SlettFlyplass(string id)
        {
            if (ErAdmin())
            {
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
                return Kunder();
            }
            return RedirectToAction("Sok", "Home");
        }

        private bool ErAdmin()
        {
            return Session["Admin"] != null && (bool)Session["Admin"] == true;
        }

    }
}