using BLL;
using Flybilletter.Model.DomeneModel;
using Flybilletter.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private IBLLEndring bllendring;
        private IBLLAdmin blladmin;

        public AdminController() : this(new BLLBestilling(), new BLLFly(), new BLLKunde(), new BLLFlyplass(), new BLLFlygning(), new BLLRute(), new BLLEndring(), new BLLAdmin())
        {
        }

        public AdminController(IBLLBestilling bestillingstub, IBLLFly flystub, IBLLKunde kundestub, IBLLFlyplass flyplasstub, IBLLFlygning flygningstub, IBLLRute rutestub, IBLLEndring endringstub, IBLLAdmin adminstub)

        {
            bllbestilling = bestillingstub;
            bllfly = flystub;
            bllkunde = kundestub;
            bllflygning = flygningstub;
            bllflyplass = flyplasstub;
            bllrute = rutestub;
            bllendring = endringstub;
            blladmin = adminstub;
        }

        [HttpPost]
        public bool LoginAttempt(string username, string password)
        {
            if (blladmin.IsPassordGyldig(username, password))
            {
                // Login er gyldig, session variabel settes
                Session["Admin"] = true;
                return true;
            }
                
            return false;
        }

        [HttpGet]
        public ActionResult Bestillinger()
        {
            if (ErAdmin())
            {
                List<Bestilling> bestillinger = bllbestilling.HentAlle();
                return View("ListBestillinger", bestillinger);
            }
            return RedirectToAction("Sok", "Home");
        }


        public ActionResult Fly()
        {
            if (ErAdmin())
            {
                List<Fly> fly = bllfly.HentAlle();

                if (TempData["feilmelding"] != null)
                {
                    ViewBag.Feilmelding = (String)TempData["feilmelding"];
                }
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
        public string EndreFly(Fly item)
        {
            if (ErAdmin())
            {
                if (ModelState.IsValid)
                {
                    if (bllfly.Oppdater(item))
                    {
                        return "true";
                    }
                    else
                    {
                        return "Fikk ikke oppdatert flyet.";
                    }
                }
                else
                {
                    // https://stackoverflow.com/questions/1352948/how-to-get-all-errors-from-asp-net-mvc-modelstate
                    return string.Join(".\n", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                }
            }
            return "Ikke admin";
        }

        public ActionResult SlettFly(int ID)
        {
            if (ErAdmin())
            {
                if (!bllfly.Slett(ID))
                {
                    TempData["feilmelding"] = "Kunne ikke slette fly. Mulig det har flygninger relatert til seg.";
                }

                return RedirectToAction("Fly");
            }
            return RedirectToAction("Sok", "Home");
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
                return View("LagFlyplass");
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
                if (model != null)
                {
                    return View("EndreFlyplass", model);
                }
                ViewBag.Feilmelding = "Fant ikke flyplass med ID " + id + " i databasen";
                //TODO: redirect til feilmelding
            }
            return RedirectToAction("Sok", "Home");
        }

        [HttpPost]
        public string EndreFlyplass(Flyplass item)
        {
            if (ErAdmin())
            {
                if (ModelState.IsValid)
                {
                    if (bllflyplass.Endre(item))
                    {
                        return "true";
                    }
                    return "En feil oppsto under lagring til databasen.";

                }
                // https://stackoverflow.com/questions/1352948/how-to-get-all-errors-from-asp-net-mvc-modelstate 
                return string.Join(".\n", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));

            }
            return "NotAdmin";
        }

        public ActionResult Ruter()
        {
            if (ErAdmin())
            {
                var model = bllrute.HentAlle();
                ViewBag.Flyplasser = bllflyplass.HentAlle();
                if (TempData["feilmelding"] != null)
                {
                    ViewBag.Feilmelding = (String)TempData["feilmelding"];
                }
                return View("ListRuter", model);
            }
            return RedirectToAction("Sok", "Home");
        }

        public ActionResult LagRute()
        {
            if (ErAdmin())
            {
                ViewBag.Flyplasser = bllflyplass.HentAlle();
                return View();
            }
            return RedirectToAction("Sok", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LagRute(NyRuteViewModel rute)
        {
            if (ErAdmin())
            {
                if (rute.FraFlyplassID == rute.TilFlyplassID)
                {
                    ModelState.AddModelError("TilFlyplassID", "Flyplassene må være ulik");
                }
                if (rute.FraFlyplassID != "OSL" && rute.TilFlyplassID != "OSL")
                {
                    ModelState.AddModelError("TilFlyplassID", "Minst en av flygningene må gå til OSL på grunn av begrensninger gjort i oblig 1.");
                }
                if (ModelState.IsValid)
                {
                    if (! bllrute.LagRute(rute))
                    {
                        TempData["feilmelding"] = "En feil oppso under lagring av ruten til databasen";
                    }
                    return RedirectToAction("Ruter");
                }
                ViewBag.Flyplasser = bllflyplass.HentAlle();
                return View(rute);
            }
            return RedirectToAction("Sok", "Home");
        }


        [HttpPost]
        public string LagreRute(Rute rute)
        {
            if (ErAdmin())
            {
                var feilmeldinger = new List<String>();

                if (!ModelState.IsValidField("rute.ID"))
                {
                    ModelState.TryGetValue("rute.ID", out var value);
                    string hvorfor = value.Errors.First().ErrorMessage;
                    feilmeldinger.Add($"Ugyldig id: {hvorfor}.");
                }
                if (!ModelState.IsValidField("rute.BasePris"))
                {
                    ModelState.TryGetValue("rute.BasePris", out var value);
                    string hvorfor = value.Errors.First().ErrorMessage;
                    feilmeldinger.Add($"Ugyldig pris: {hvorfor} .");
                }
                if (!ModelState.IsValidField("rute.Reisetid"))
                {
                    ModelState.TryGetValue("rute.Reisetid", out var value);
                    string hvorfor = value.Errors.First().ErrorMessage;
                    feilmeldinger.Add($"Ugyldig id: {hvorfor}.");
                }
                if (!ModelState.IsValidField("rute.Fra.ID"))
                {
                    ModelState.TryGetValue("rute.Fra.ID", out var value);
                    string hvorfor = value.Errors.First().ErrorMessage;
                    feilmeldinger.Add($"Ugyldig fra Flyplass: {hvorfor}.");
                }
                if (!ModelState.IsValidField("rute.Til.ID"))
                {
                    ModelState.TryGetValue("rute.Til.ID", out var value);
                    string hvorfor = value.Errors.First().ErrorMessage;
                    feilmeldinger.Add($"Ugyldig til flyplass: {hvorfor}.");
                }

                if (rute.Fra.ID != "OSL" && rute.Til.ID != "OSL")
                {
                    feilmeldinger.Add("Grunnet begrensninger fra oblig 1 må alle ruter gå innom Oslo (OSL).");
                }
                if (rute.Fra.ID == rute.Til.ID)
                {
                    feilmeldinger.Add("Rute kan ikke ha samme til og fra.");
                }


                if (feilmeldinger.Count > 0)
                {
                    return String.Join("\n", feilmeldinger);
                }

                bool ok = bllrute.LagreRute(rute);
                if (ok)
                {
                    return "true";
                }
                else
                {
                    return "En feil oppsto med lagring i database.";
                }

            }
            return "NotAdmin";
        }

        public ActionResult SlettRute(int id)
        {
            if (ErAdmin())
            {
                if (!bllrute.Slett(id))
                {
                    TempData["feilmelding"] = "Kunne ikke slette rute. Mulig den har flygninger relatert til seg.";
                }
                return RedirectToAction("Ruter");
            }
            return RedirectToAction("Sok", "Home");
        }


        public ActionResult Kunder()
        {
            if (ErAdmin())
            {
                List<Kunde> kunder = bllkunde.HentAlle();
                if (TempData["feilmelding"] != null)
                {
                    ViewBag.Feilmelding = (String)TempData["feilmelding"];
                }
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
        public ActionResult LeggTilKunde(Kunde kunde)
        {
            if (ErAdmin())
            {
                if (bllkunde.LeggInn(kunde))
                    return RedirectToAction("Kunder");
                else return RedirectToAction("Kunder");
            }
                return RedirectToAction("Sok", "Home");
        }

        [HttpPost]
        public string LagreKunde(Kunde item)
        {
            if (ErAdmin())
            {
                if (ModelState.IsValid)
                {
                    if (bllkunde.Oppdater(item))
                    {
                        return "true";
                    }
                    return "En feil oppsto med lagring i database.";
                }
                var feilmeldinger = new List<String>();

                for (var i = 0; i < ModelState.Keys.Count(); i++)
                {
                    var value = ModelState.Values.ElementAt(i);
                    if (value.Errors.Count > 0)
                    {
                        string hva = ModelState.Keys.ElementAt(i).Substring(5);
                        string hvorfor = value.Errors.First().ErrorMessage;
                        feilmeldinger.Add($"Ugyldig {hva}: {hvorfor}.");
                    }
                }

                if (feilmeldinger.Count > 0)
                {
                    return String.Join("\n", feilmeldinger);
                }

            }
            return "NotAdmin";
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
                if (! bllkunde.SlettKunde(id))
                {

                    TempData["feilmelding"] = "Kunne ikke slette kunde. Mulig den har bestillinger relatert til seg.";
                }

                return RedirectToAction("Kunder");

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

        [HttpPost]
        public string EndreFlygning(int id, DateTime nyAvgangstid)
        {
            if (ErAdmin())
            {
                if(nyAvgangstid <= DateTime.Now)
                {
                    return "Ugyldig data. Avgangstid må være et senere tiddspunkt enn nå.";
                }
                if(bllflygning.EndreFlygning(id, nyAvgangstid))
                {
                    return "true";
                }
                return "En feil oppsto under lagring til databasen.";
            }
            return "NotAdmin";
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
                return View("LagNyFlygning");
            }
            return RedirectToAction("Sok", "Home");
        }

        [HttpPost]
        public ActionResult LagreFlygning(LagFlygningViewModel flygning)
        {
            if (ErAdmin())
            {
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
        public ActionResult Administrator()
        {
            if (ErAdmin())
            {
                ViewBag.FeilFraFil = bllendring.ParseFeil(@"..\DAL\flybilletter-log.txt");
                ViewBag.Endringer = bllendring.Hent();
                return View();
            }
            return RedirectToAction("Sok", "Home");
        }

        [HttpPost]
        public ActionResult LagAdmin(Admin model)
        {
            if (ErAdmin())
            {
                if (ModelState.IsValid)
                {
                    blladmin.LeggInn(model);
                }
                return RedirectToAction("Administrator");
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