using BLL;
using BLL.Stub;
using Flybilletter.Controllers;
using Flybilletter.DAL.Stub;
using Flybilletter.Model.DomeneModel;
using Flybilletter.Model.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Enhetstesting
{
    [TestClass]
    public class AdminControllerTest
    {

        private static AdminController NyAdminControllerMedSession(bool settErAdminTilTrue)
        {
            var bllfly = new BLLFly(new DBFlyStub());
            var bllflyplass = new BLLFlyplass(new DBFlyplassStub());
            var bllflygning = new BLLFlygning(new DBFlygningStub(), new DBFlyplassStub(), new DBRuteStub(), new DBFlyStub());
            var bllbestilling = new BLLBestilling(new DBBestillingStub(), new DBFlygningStub());
            var bllkunde = new BLLKunde(new DBKundeStub(), new DBPostnummerStub());
            var bllrute = new BLLRute(new DBRuteStub());
            var bllendring = new BLLEndring(new DBEndringStub());
            var blladmin = new BLLAdminStub();

            var sessionMock = new TestControllerBuilder();
            var controller = new AdminController(bllbestilling, bllfly, bllkunde, bllflyplass, bllflygning, bllrute, bllendring, blladmin);
            sessionMock.InitializeController(controller);
            if (settErAdminTilTrue)
            {
                controller.Session["admin"] = true;
            }

            return controller;
        }

        [TestMethod]
        public void SkalIkkeKunneLoggeInn()
        {
            var controller = NyAdminControllerMedSession(false);

            bool faktisk = controller.LoginAttempt("test", "feilPassord");

            Assert.IsFalse(faktisk);

        }

        [TestMethod]
        public void SkalKunneLoggeInn()
        {
            var controller = NyAdminControllerMedSession(true);

            var admin = new Admin()
            {
                Brukernavn = "test",
                Passord = "riktigPassord"
            };

            controller.LagAdmin(admin);
            controller.Session["admin"] = false;
            bool faktisk = controller.LoginAttempt("test", "riktigPassord");

            Assert.IsTrue(faktisk);
        }

        [TestMethod]
        public void BestillingerSkalReturnereViewMedModell()
        {
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (ViewResult)controller.Bestillinger();

            Assert.AreEqual("ListBestillinger", faktisk.ViewName);
            Assert.AreNotEqual(null, faktisk.Model);
        }

        [TestMethod]
        public void FlyUtenFeilmelding()
        {
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (ViewResult)controller.Fly();


            Assert.AreEqual("ListFly", faktisk.ViewName);
            Assert.AreNotEqual(null, faktisk.Model);
            Assert.AreEqual(null, faktisk.ViewBag.Feilmelding);
        }

        [TestMethod]
        public void FlyMedFeilmelding()
        {
            var controller = NyAdminControllerMedSession(true);
            string feilmelding = "Dette er en feilmelding";
            controller.TempData["feilmelding"] = feilmelding;

            var faktisk = (ViewResult)controller.Fly();

            Assert.AreEqual("ListFly", faktisk.ViewName);
            Assert.AreNotEqual(null, faktisk.Model);
            Assert.AreEqual(feilmelding, faktisk.ViewBag.Feilmelding);
        }

        [TestMethod]
        public void SeDetaljertBestillingSkalReturnView()
        {
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (RedirectToRouteResult)controller.SeDetaljerBestilling("ASD123");

            Assert.AreEqual("ReferanseSok", faktisk.RouteValues["action"]);
        }

        [TestMethod]
        public void EndreFlySkalFungereMedGyldigModell()
        {
            var controller = NyAdminControllerMedSession(true);
            var fly = new Fly()
            {
                AntallSeter = 5,
                ID = 1,
                Modell = "Airbus A380"
            };
            string faktisk = controller.EndreFly(fly);

            Assert.AreEqual("true", faktisk);
        }

        [TestMethod]
        public void EndreFlyMedUgyldigID()
        {
            var controller = NyAdminControllerMedSession(true);
            var fly = new Fly()
            {
                AntallSeter = 5,
                ID = -1,
                Modell = "Airbus A380"
            };
            string faktisk = controller.EndreFly(fly);

            Assert.AreEqual("En feil oppsto under lagring til databasen.", faktisk);
        }

        [TestMethod]
        public void EndreFlyMedUgyldigModell()
        {
            var controller = NyAdminControllerMedSession(true);
            var fly = new Fly()
            {
                AntallSeter = -5,
                ID = 1,
                Modell = "Airbus A380"
            };
            controller.ModelState.AddModelError("Fly.AntallSeter", "Antall seter kan kun være et positivt heltall");

            string faktisk = controller.EndreFly(fly);

            Assert.AreEqual("Antall seter kan kun være et positivt heltall", faktisk);
        }

        [TestMethod]
        public void SlettFlyUtenFeilmelding()
        {
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (RedirectToRouteResult)controller.SlettFly(1);

            Assert.AreEqual("Fly", faktisk.RouteValues["action"]);
        }

        [TestMethod]
        public void SlettFlyMedFeilmelding()
        {
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (RedirectToRouteResult)controller.SlettFly(-1);

            Assert.AreEqual("Fly", faktisk.RouteValues["action"]);
            string forventet = "Kunne ikke slette fly. Mulig det har flygninger relatert til seg.";
            Assert.AreEqual(forventet, controller.TempData["feilmelding"]);
        }

        [TestMethod]
        public void LagViewSkalGiView()
        {
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (ViewResult)controller.LagFly();

            Assert.AreEqual("", faktisk.ViewName);
        }


        [TestMethod]
        public void LagFlyMedUgyldigModelState()
        {
            var fly = new Fly()
            {
                AntallSeter = -5,
                ID = 1,
                Modell = "Airbus A380"
            };
            var controller = NyAdminControllerMedSession(true);
            controller.ModelState.AddModelError("Fly.AntallSeter", "Antall seter kan kun være et positivt heltall");
            var faktisk = (ViewResult)controller.LagFly(fly);

            Assert.AreEqual("", faktisk.ViewName);
            Assert.AreEqual(fly, faktisk.Model);
        }

        [TestMethod]
        public void LagFlyMedUgyldigModelTilDatabasen()
        {
            var fly = new Fly()
            {
                AntallSeter = 0,
                ID = 1,
                Modell = "Airbus A380"
            };
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (RedirectToRouteResult)controller.LagFly(fly);

            Assert.AreEqual("Fly", faktisk.RouteValues["action"]);

            string forventet = "Kunne ikke legge inn fly. Feil i databasen.";
            Assert.AreEqual(forventet, controller.TempData["feilmelding"]);
        }

        [TestMethod]
        public void LagFlyMedGyldigModel()
        {
            var fly = new Fly()
            {
                AntallSeter = 4,
                ID = 1,
                Modell = "Airbus A380"
            };
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (RedirectToRouteResult)controller.LagFly(fly);

            Assert.AreEqual("Fly", faktisk.RouteValues["action"]);
            Assert.AreEqual(null, controller.TempData["feilmelding"]);
        }

        [TestMethod]
        public void SlettBestillingUgyldigReferanse()
        {
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (RedirectToRouteResult)controller.SlettBestilling("ABC123");

            Assert.AreEqual("Bestillinger", faktisk.RouteValues["action"]);
            Assert.AreEqual("Kunne ikke slette bestilling. Mulig flyet allerede har gått.", controller.TempData["feilmelding"]);
        }

        [TestMethod]
        public void SlettBestillingGyldigReferanse()
        {
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (RedirectToRouteResult)controller.SlettBestilling("AAB123");

            Assert.AreEqual("Bestillinger", faktisk.RouteValues["action"]);
            Assert.AreEqual(null, controller.TempData["feilmelding"]);
        }

        [TestMethod]
        public void FlyplasserUteneilmelding()
        {
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (ViewResult)controller.Flyplasser();

            Assert.AreEqual("ListFlyplasser", faktisk.ViewName);
            Assert.AreNotEqual(null, faktisk.Model);
            Assert.AreEqual(null, faktisk.ViewBag.Feilmelding);
        }

        [TestMethod]
        public void FlyplasserMedFeilmelding()
        {
            var controller = NyAdminControllerMedSession(true);
            string feilmelding = "Dette er en feilmelding";
            controller.TempData["feilmelding"] = feilmelding;

            var faktisk = (ViewResult)controller.Flyplasser();

            Assert.AreEqual("ListFlyplasser", faktisk.ViewName);
            Assert.AreNotEqual(null, faktisk.Model);
            Assert.AreEqual(feilmelding, faktisk.ViewBag.Feilmelding);
        }

        [TestMethod]
        public void LagFlyplass()
        {
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (ViewResult)controller.LagFlyplass();

            Assert.AreEqual("", faktisk.ViewName);
        }

        [TestMethod]
        public void LagFlyplassUgyldigModell()
        {
            var controller = NyAdminControllerMedSession(true);

            controller.ModelState.AddModelError("Fly", "Fly er obligatorisk");
            var faktisk = (ViewResult)controller.LagFlyplass(null);

            Assert.AreEqual("", faktisk.ViewName);
            Assert.AreEqual(null, faktisk.Model);
        }

        [TestMethod]
        public void LagFlyplassUgyldigModellIDatabase()
        {
            var controller = NyAdminControllerMedSession(true);
            var flyplass = new Flyplass();

            var faktisk = (RedirectToRouteResult)controller.LagFlyplass(flyplass);

            Assert.AreEqual("Flyplasser", faktisk.RouteValues["action"]);
            string forventet = "Kunne ikke legge inn flyplass. Feil i databasen.";
            Assert.AreEqual(forventet, controller.TempData["feilmelding"]);
        }


        [TestMethod]
        public void LagFlyplassGyldigModell()
        {
            var controller = NyAdminControllerMedSession(true);
            var flyplass = new Flyplass()
            {
                ID = "OSL",
                Navn = "Gardermoen Lufthavn",
                By = "Oslo",
                Land = "Norge"
            };

            var faktisk = (RedirectToRouteResult)controller.LagFlyplass(flyplass);

            Assert.AreEqual("Flyplasser", faktisk.RouteValues["action"]);
            Assert.AreEqual(null, controller.TempData["feilmelding"]);
        }

        [TestMethod]
        public void EndreFlyplassSkalFungereMedGyldigModell()
        {
            var controller = NyAdminControllerMedSession(true);
            var flyplass = new Flyplass()
            {
                ID = "OSL",
                Navn = "Gardermoen Lufthavnn",
                By = "Oslo",
                Land = "Norge"
            };
            string faktisk = controller.EndreFlyplass(flyplass);

            Assert.AreEqual("true", faktisk);
        }

        [TestMethod]
        public void EndreFlyplassMedUgyldigID()
        {
            var controller = NyAdminControllerMedSession(true);
            var flyplass = new Flyplass()
            {
                ID = "",
                Navn = "Gardermoen Lufthavn",
                By = "Oslo",
                Land = "Norge"
            };
            string faktisk = controller.EndreFlyplass(flyplass);

            Assert.AreEqual("En feil oppsto under lagring til databasen.", faktisk);
        }

        [TestMethod]
        public void EndreFlyplassMedUgyldigModell()
        {
            var controller = NyAdminControllerMedSession(true);
            var flyplass = new Flyplass()
            {
                ID = "OSL",
                Navn = "#!¤",
                By = "Oslo",
                Land = "Norge"
            };
            controller.ModelState.AddModelError("Flyplass.Navn", "Navn kan kun bestå av bokstaver");

            string faktisk = controller.EndreFlyplass(flyplass);

            Assert.AreEqual("Navn kan kun bestå av bokstaver", faktisk);
        }

        [TestMethod]
        public void RuterUtenFeilmelding()
        {
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (ViewResult)controller.Ruter();

            Assert.AreEqual("ListRuter", faktisk.ViewName);
            Assert.AreNotEqual(null, faktisk.Model);
            Assert.AreEqual(null, faktisk.ViewBag.Feilmelding);
        }

        [TestMethod]
        public void RuterMedFeilmelding()
        {
            var controller = NyAdminControllerMedSession(true);
            string feilmelding = "Dette er en feilmelding";
            controller.TempData["feilmelding"] = feilmelding;

            var faktisk = (ViewResult)controller.Ruter();

            Assert.AreEqual("ListRuter", faktisk.ViewName);
            Assert.AreNotEqual(null, faktisk.Model);
            Assert.AreEqual(feilmelding, faktisk.ViewBag.Feilmelding);
        }

        [TestMethod]
        public void LagRute()
        {
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (ViewResult)controller.LagRute();

            Assert.AreEqual("", faktisk.ViewName);
        }

        [TestMethod]
        public void LagRuteUgyldigModell()
        {
            var controller = NyAdminControllerMedSession(true);

            controller.ModelState.AddModelError("Rute", "Rute er obligatorisk");
            var forventet = new NyRuteViewModel();
            var faktisk = (ViewResult)controller.LagRute(forventet);

            Assert.AreEqual("", faktisk.ViewName);
            Assert.AreEqual(forventet, faktisk.Model);
        }

        [TestMethod]
        public void LagRuteSammeFraOgTil()
        {
            var controller = NyAdminControllerMedSession(true);
            var rute = new NyRuteViewModel()
            {
                FraFlyplassID = "OSL",
                TilFlyplassID = "OSL",
                Basepris = 1499,
                Reisetid = new TimeSpan(1, 0, 0)
            };

            var faktisk = (ViewResult)controller.LagRute(rute);

            Assert.AreEqual("", faktisk.ViewName);
            Assert.AreEqual("Flyplassene må være ulik", controller.ModelState["TilFlyplassID"].Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void LagRuteIngenInnomOslo()
        {
            var controller = NyAdminControllerMedSession(true);
            var rute = new NyRuteViewModel()
            {
                FraFlyplassID = "A",
                TilFlyplassID = "B",
                Basepris = 1499,
                Reisetid = new TimeSpan(1, 0, 0)
            };

            var faktisk = (ViewResult)controller.LagRute(rute);

            Assert.AreEqual("", faktisk.ViewName);
            string forventet = "Minst en av flygningene må gå til OSL på grunn av begrensninger gjort i oblig 1.";
            Assert.AreEqual(forventet, controller.ModelState["TilFlyplassID"].Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void LagRuteUgyldigModellIDatabase()
        {
            var controller = NyAdminControllerMedSession(true);
            var rute = new NyRuteViewModel()
            {
                FraFlyplassID = "",
                TilFlyplassID = "OSL",
                Basepris = -1,
                Reisetid = new TimeSpan(1, 0, 0)
            };

            var faktisk = (RedirectToRouteResult)controller.LagRute(rute);

            Assert.AreEqual("Ruter", faktisk.RouteValues["action"]);
            string forventet = "En feil oppso under lagring av ruten til databasen.";
            Assert.AreEqual(forventet, controller.TempData["feilmelding"]);
        }


        [TestMethod]
        public void LagRuteGyldigModell()
        {
            var controller = NyAdminControllerMedSession(true);
            var rute = new NyRuteViewModel()
            {
                FraFlyplassID = "OSL",
                TilFlyplassID = "BOO",
                Basepris = 1499,
                Reisetid = new TimeSpan(1, 0, 0)
            };

            var faktisk = (RedirectToRouteResult)controller.LagRute(rute);

            Assert.AreEqual("Ruter", faktisk.RouteValues["action"]);
            Assert.AreEqual(null, controller.TempData["feilmelding"]);
        }

        [TestMethod]
        public void EndreRuteSkalFungereMedGyldigModell()
        {
            var controller = NyAdminControllerMedSession(true);
            var rute = new Rute()
            {
                ID = 1,
                Fra = new Flyplass()
                {
                    ID = "OSL",
                    By = "Oslo",
                    Land = "Norge",
                    Navn = "Gardermoen Lufthavn"

                },
                Til = new Flyplass()
                {
                    ID = "BOO",
                    By = "Bodø",
                    Land = "Norge",
                    Navn = "Bodø Lufthavn"
                },
                BasePris = 1499,
                Reisetid = new TimeSpan(1, 0, 0)
            };

            string faktisk = controller.EndreRute(rute);

            Assert.AreEqual("true", faktisk);
        }

        [TestMethod]
        public void EndreRuterFeilModellOgSammeDestinasjon()
        {
            var controller = NyAdminControllerMedSession(true);
            var rute = new Rute()
            {
                ID = 1,
                Fra = new Flyplass()
                {
                    ID = "OSL",
                    By = "Oslo",
                    Land = "Norge",
                    Navn = "Gardermoen Lufthavn"
                },
                Til = new Flyplass()
                {
                    ID = "OSL",
                    By = "Bodø",
                    Land = "Norge",
                    Navn = "Bodø Lufthavn"
                },
                BasePris = 1499,
                Reisetid = new TimeSpan(1, 0, 0)
            };

            controller.ModelState.AddModelError("rute.ID", "feil");
            controller.ModelState.AddModelError("rute.BasePris", "feil");
            controller.ModelState.AddModelError("rute.Reisetid", "feil");
            controller.ModelState.AddModelError("rute.Fra.ID", "feil");
            controller.ModelState.AddModelError("rute.Til.ID", "feil");

            string faktisk = controller.EndreRute(rute);
            string forventet = "Ugyldig id: feil.\nUgyldig pris: feil.\nUgyldig id: feil.\nUgyldig fra Flyplass: feil.\nUgyldig til flyplass: feil.\nRute kan ikke ha samme til og fra.";

            Assert.AreEqual(forventet, faktisk);
        }

        [TestMethod]
        public void EndreRuterGaarIkkeGjennomOslo()
        {
            var controller = NyAdminControllerMedSession(true);
            var rute = new Rute()
            {
                ID = 1,
                Fra = new Flyplass()
                {
                    ID = "BOO",
                    By = "Bodø",
                    Land = "Norge",
                    Navn = "Bodø Lufthavn"
                },
                Til = new Flyplass()
                {
                    ID = "ARN",
                    By = "Stockholm",
                    Land = "Sverige",
                    Navn = "Arlanda Lufthavn"
                },
                BasePris = 1499,
                Reisetid = new TimeSpan(1, 0, 0)
            };

            string faktisk = controller.EndreRute(rute);
            string forventet = "Grunnet begrensninger fra oblig 1 må alle ruter gå innom Oslo (OSL).";

            Assert.AreEqual(forventet, faktisk);
        }

        [TestMethod]
        public void EndreRuterFeilIDatabase()
        {
            var controller = NyAdminControllerMedSession(true);
            var rute = new Rute()
            {
                ID = 1,
                Fra = new Flyplass()
                {
                    ID = "OSL",
                    By = "Oslo",
                    Land = "Norge",
                    Navn = "Gardermoen Lufthavn"
                },
                Til = new Flyplass()
                {
                    ID = "ARN",
                    By = "Stockholm",
                    Land = "Sverige",
                    Navn = "Arlanda Lufthavn"
                },
                BasePris = -5,
                Reisetid = new TimeSpan(1, 0, 0)
            };

            string faktisk = controller.EndreRute(rute);
            string forventet = "En feil oppsto med lagring i database.";

            Assert.AreEqual(forventet, faktisk);
        }

        [TestMethod]
        public void SlettRuteUtenFeilmelding()
        {
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (RedirectToRouteResult)controller.SlettRute(1);

            Assert.AreEqual("Ruter", faktisk.RouteValues["action"]);
        }

        [TestMethod]
        public void SlettRuteMedFeilmelding()
        {
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (RedirectToRouteResult)controller.SlettRute(-1);

            Assert.AreEqual("Ruter", faktisk.RouteValues["action"]);
            string forventet = "Kunne ikke slette rute. Mulig den har flygninger relatert til seg.";
            Assert.AreEqual(forventet, controller.TempData["feilmelding"]);
        }

        [TestMethod]
        public void KunderUtenFeilmelding()
        {
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (ViewResult)controller.Kunder();

            Assert.AreEqual("ListKunder", faktisk.ViewName);
            Assert.AreNotEqual(null, faktisk.Model);
            Assert.AreEqual(null, faktisk.ViewBag.Feilmelding);
        }

        [TestMethod]
        public void KunderMedFeilmelding()
        {
            var controller = NyAdminControllerMedSession(true);
            string feilmelding = "Dette er en feilmelding";
            controller.TempData["feilmelding"] = feilmelding;

            var faktisk = (ViewResult)controller.Kunder();

            Assert.AreEqual("ListKunder", faktisk.ViewName);
            Assert.AreNotEqual(null, faktisk.Model);
            Assert.AreEqual(feilmelding, faktisk.ViewBag.Feilmelding);
        }

        [TestMethod]
        public void LagKunde()
        {
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (ViewResult)controller.LagKunde();

            Assert.AreEqual("", faktisk.ViewName);
        }

        [TestMethod]
        public void LagKundeUgyldigModell()
        {
            var controller = NyAdminControllerMedSession(true);

            controller.ModelState.AddModelError("Kunde", "Kunde er obligatorisk");
            var faktisk = (ViewResult)controller.LagKunde(null);

            Assert.AreEqual("", faktisk.ViewName);
            Assert.AreEqual(null, faktisk.Model);
        }

        [TestMethod]
        public void LagKundeUgyldigModellIDatabase()
        {
            var controller = NyAdminControllerMedSession(true);
            var kunde = new Kunde();

            var faktisk = (RedirectToRouteResult)controller.LagKunde(kunde);

            Assert.AreEqual("Kunder", faktisk.RouteValues["action"]);
            string forventet = "Kunne ikke legge inn kunde. Feil i databasen.";
            Assert.AreEqual(forventet, controller.TempData["feilmelding"]);
        }

        [TestMethod]
        public void LagKundeGyldigModell()
        {
            var controller = NyAdminControllerMedSession(true);
            var kunde = new Kunde()
            {
               Fornavn = "Ola",
               Etternavn ="Nordmann",
               Adresse ="Testveien 1",
               EPost ="test@test.no",
               Fodselsdag = new DateTime(1970,1,1),
               Postnr= "0001",
               Poststed ="Oslo",
               Tlf = "12345678"
            };

            var faktisk = (RedirectToRouteResult)controller.LagKunde(kunde);

            Assert.AreEqual("Kunder", faktisk.RouteValues["action"]);
            Assert.AreEqual(null, controller.TempData["feilmelding"]);
        }

        [TestMethod]
        public void EndreKundeSkalFungereMedGyldigModell()
        {
            var controller = NyAdminControllerMedSession(true);
            var kunde = new Kunde()
            {
                Fornavn = "Ola",
                Etternavn = "Nordmann",
                Adresse = "Testveien 1",
                EPost = "test@test.no",
                Fodselsdag = new DateTime(1970, 1, 1),
                Postnr = "0001",
                Poststed = "Oslo",
                Tlf = "12345678"
            };
            string faktisk = controller.EndreKunde(kunde);

            Assert.AreEqual("true", faktisk);
        }

        [TestMethod]
        public void EndreKundeMedUgyldigID()
        {
            var controller = NyAdminControllerMedSession(true);
            var kunde = new Kunde()
            {
                Fornavn = null,
                Etternavn = "Nordmann",
                Adresse = "Testveien 1",
                EPost = "test@test.no",
                Fodselsdag = new DateTime(1970, 1, 1),
                Postnr = "0001",
                Poststed = "Oslo",
                Tlf = "12345678"
            };
            string faktisk = controller.EndreKunde(kunde);

            Assert.AreEqual("En feil oppsto under lagring til databasen.", faktisk);
        }

        [TestMethod]
        public void EndreKundeMedUgyldigModell()
        {
            var controller = NyAdminControllerMedSession(true);
            var kunde = new Kunde()
            {
                Fornavn = null,
                Etternavn = "Nordmann",
                Adresse = "Testveien 1",
                EPost = "test@test.no",
                Fodselsdag = new DateTime(1970, 1, 1),
                Postnr = "0001",
                Poststed = "Oslo",
                Tlf = "12345678"
            };
            controller.ModelState.AddModelError("item.Fornavn", "Fornavn er obligatorisk");

            string faktisk = controller.EndreKunde(kunde);

            Assert.AreEqual("Ugyldig Fornavn: Fornavn er obligatorisk.", faktisk);
        }

        [TestMethod]
        public void SlettKundeUtenFeilmelding()
        {
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (RedirectToRouteResult)controller.SlettKunde(1);

            Assert.AreEqual("Kunder", faktisk.RouteValues["action"]);
        }

        [TestMethod]
        public void SlettKundeMedFeilmelding()
        {
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (RedirectToRouteResult)controller.SlettKunde(-1);

            Assert.AreEqual("Kunder", faktisk.RouteValues["action"]);
            string forventet = "Kunne ikke slette kunde. Mulig den har bestillinger relatert til seg.";
            Assert.AreEqual(forventet, controller.TempData["feilmelding"]);
        }

        [TestMethod]
        public void FlygningerUtenFeilmelding()
        {
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (ViewResult)controller.Flygninger();


            Assert.AreEqual("ListFlygning", faktisk.ViewName);
            Assert.AreNotEqual(null, faktisk.Model);
            Assert.AreEqual(null, faktisk.ViewBag.Feilmelding);
        }

        [TestMethod]
        public void FlygningerMedFeilmelding()
        {
            var controller = NyAdminControllerMedSession(true);
            string feilmelding = "Dette er en feilmelding";
            controller.TempData["feilmelding"] = feilmelding;

            var faktisk = (ViewResult)controller.Flygninger();

            Assert.AreEqual("ListFlygning", faktisk.ViewName);
            Assert.AreNotEqual(null, faktisk.Model);
            Assert.AreEqual(feilmelding, faktisk.ViewBag.Feilmelding);
        }

        [TestMethod]
        public void EndreFlygningSkalFungereMedGyldigModell()
        {
            var controller = NyAdminControllerMedSession(true);
            
            string faktisk = controller.EndreFlygning(1, DateTime.Now.AddDays(2));

            Assert.AreEqual("true", faktisk);
        }

        [TestMethod]
        public void EndreFlygningMedUgyldigID()
        {
            var controller = NyAdminControllerMedSession(true);
 
            string faktisk = controller.EndreFlygning(0, DateTime.Now.AddDays(2) );

            Assert.AreEqual("En feil oppsto under lagring til databasen.", faktisk);
        }

        [TestMethod]
        public void EndreFlygningMedUgyldigModell()
        {
            var controller = NyAdminControllerMedSession(true);

            string faktisk = controller.EndreFlygning(0, DateTime.Now.AddDays(-2));

            Assert.AreEqual("Ugyldig data. Avgangstid må være et senere tiddspunkt enn nå.", faktisk);
        }

        [TestMethod]
        public void LagFlygning()
        {
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (ViewResult)controller.LagFlygning();

            Assert.AreEqual("", faktisk.ViewName);
            Assert.AreNotEqual(null, controller.ViewBag.ruter);
            Assert.AreNotEqual(null, controller.ViewBag.fly);
        }

        [TestMethod]
        public void LagFlygningUgyldigModell()
        {
            var controller = NyAdminControllerMedSession(true);

            controller.ModelState.AddModelError("Flygning", "Flygning er obligatorisk");
            var faktisk = (ViewResult)controller.LagFlygning(null);

            Assert.AreEqual("", faktisk.ViewName);
            Assert.AreEqual(null, faktisk.Model);
        }

        [TestMethod]
        public void LagFlygningUgyldigModellIDatabase()
        {
            var controller = NyAdminControllerMedSession(true);
            var flygning = new LagFlygningViewModel();

            var faktisk = (RedirectToRouteResult)controller.LagFlygning(flygning);

            Assert.AreEqual("Flygninger", faktisk.RouteValues["action"]);
            string forventet = "Kunne ikke legge inn flygning. Feil i databasen.";
            Assert.AreEqual(forventet, controller.TempData["feilmelding"]);
        }


        [TestMethod]
        public void LagFlygningGyldigModell()
        {
            var controller = NyAdminControllerMedSession(true);
            var flygning = new LagFlygningViewModel()
            {
                AvgangsTid = DateTime.Now.AddHours(5),
                FlyID = "1",
                RuteID = "1"
            };

            var faktisk = (RedirectToRouteResult)controller.LagFlygning(flygning);

            Assert.AreEqual("Flygninger", faktisk.RouteValues["action"]);
            Assert.AreEqual(null, controller.TempData["feilmelding"]);
        }

        [TestMethod]
        public void EndreStatusFlygningReturnererRiktigViewHvisAdmin()
        {
            var controller = NyAdminControllerMedSession(true);
            var resultat = (RedirectToRouteResult)controller.EndreStatusFlygning(0);
            Assert.AreEqual("Flygninger", resultat.RouteValues["action"]);
        }

        [TestMethod]
        public void EndreStatusFlygningFeilMeldingHvisIkkeSuksessIDB()
        {
            var controller = NyAdminControllerMedSession(true);
            var resultat = controller.EndreStatusFlygning(-1);
            Assert.IsNotNull(controller.TempData["feilmelding"]);
        }

        [TestMethod]
        public void AdministratorReturnererView()
        {
            var controller = NyAdminControllerMedSession(true);
            var result = (ViewResult)controller.Administrator();
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void LagAdminModelStateNotValid()
        {
            var controller = NyAdminControllerMedSession(true);
            var admin = new Admin()
            {
                Brukernavn = null,
                Passord = null
            };
            controller.ModelState.AddModelError("Brukernavn", "Feil");
            var result = (RedirectToRouteResult)controller.LagAdmin(admin);
            Assert.AreEqual("Administrator", result.RouteValues["action"]);
        }

        [TestMethod]
        public void LagAdminModelStateValid()
        {
            var controller = NyAdminControllerMedSession(true);
            var admin = new Admin()
            {
                Brukernavn = "testadmin",
                Passord = "Test1"
            };
            var result = (RedirectToRouteResult)controller.LagAdmin(admin);
            Assert.AreEqual("Administrator", result.RouteValues["action"]);
        }

        [TestMethod]
        public void EndreAdminModelStateNotValid()
        {
            var controller = NyAdminControllerMedSession(true);
            var adminPassordViewModel = new AdminPassordViewModel()
            {
                Username = null,
                Gammelt = null,
                Nytt = null,
                NyttBekreft = null
            };
            controller.ModelState.AddModelError("Gammelt", "Feil");
            var result = (string)controller.EndreAdmin(adminPassordViewModel);
            Assert.AreNotEqual("true", result);
        }

        [TestMethod]
        public void EndreAdminModelStateValidIkkeLeggeTil()
        {
            var controller = NyAdminControllerMedSession(true);
            var adminPassordViewModel = new AdminPassordViewModel()
            {
                Username = "testadmin",
                Gammelt = "Gammelt1",
                Nytt = "NyttPassord1",
                NyttBekreft = "NyttP" // Model er gyldig, men nytt != nyttbekreft
            };
            var result = (string)controller.EndreAdmin(adminPassordViewModel);
            Assert.AreNotEqual("true", result);
        }

        [TestMethod]
        public void EndreAdminAltErGyldigMenBrukerErRoot()
        {
            var controller = NyAdminControllerMedSession(true);
            var adminPassordViewModel = new AdminPassordViewModel()
            {
                Username = "root",
                Gammelt = "Gammelt1",
                Nytt = "NyttPassord1",
                NyttBekreft = "NyttPassord1"
            };
            var result = (string)controller.EndreAdmin(adminPassordViewModel);
            Assert.AreNotEqual("true", result);
        }

        [TestMethod]
        public void EndreAdminSkalKunneEndrePassord()
        {
            var controller = NyAdminControllerMedSession(true);
            var adminPassordViewModel = new AdminPassordViewModel()
            {
                Username = "testadmin",
                Gammelt = "Gammelt1",
                Nytt = "NyttPassord1",
                NyttBekreft = "NyttPassord1"
            };
            var result = (string)controller.EndreAdmin(adminPassordViewModel);
            Assert.AreEqual("true", result);
        }

        [TestMethod]
        public void SlettAdminReturnererRiktigView()
        {
            var controller = NyAdminControllerMedSession(true);
            string brukernavn = ""; // Har ikke noe å si at brukernavn er ugyldig
            var result = (RedirectToRouteResult)controller.SlettAdmin(brukernavn);
            Assert.AreEqual("Administrator", result.RouteValues["action"]);
        }

        [TestMethod]
        public void SlettAdminSkalIkkeKunneSlette()
        {
            var controller = NyAdminControllerMedSession(true);
            controller.TempData["feilmelding"] = null;
            string brukernavn = "root";
            var result = controller.SlettAdmin(brukernavn);
            Assert.IsNotNull(controller.TempData["feilmelding"]);
            controller.TempData["feilmelding"] = null;
            brukernavn = "a";
            result = controller.SlettAdmin(brukernavn);
            Assert.IsNotNull(controller.TempData["feilmelding"]);
            controller.TempData["feilmelding"] = null;
            brukernavn = "aa";
            result = controller.SlettAdmin(brukernavn);
            Assert.IsNotNull(controller.TempData["feilmelding"]);
            controller.TempData["feilmelding"] = null;
            brukernavn = "aaa";
            result = controller.SlettAdmin(brukernavn);
            Assert.IsNotNull(controller.TempData["feilmelding"]);

        }

        [TestMethod]
        public void SlettAdminSkalKunddeSlette()
        {
            var controller = NyAdminControllerMedSession(true);
            string brukernavn = "abcd";
            var result = controller.SlettAdmin(brukernavn);
            Assert.IsNull(controller.TempData["feilmeldinger"]);
        }

        [TestMethod]
        public void LoggUtSetterSessionLikNull()
        {
            var controller = NyAdminControllerMedSession(true);
            var result = controller.LoggUt();
            Assert.IsNull(controller.Session["Admin"]);
        }

        [TestMethod]
        public void LoggUtRedirecterTilIndeks()
        {
            var controller = NyAdminControllerMedSession(true);
            var result = (RedirectToRouteResult)controller.LoggUt();
            Assert.AreEqual("Sok", result.RouteValues["action"]);
        }
        
        [TestMethod]
        public void SkalRedirecteTilSokNaarIkkeAdmin()
        {
            var controller = NyAdminControllerMedSession(false);
            controller.Session["admin"] = null;
            for (var i = 0; i < 2; i++)
            {
                //Sjekk først med session lik null, så false
                if (i == 0)
                {
                    controller.Session["admin"] = null;
                }
                else
                {
                    controller.Session["admin"] = false;
                }

                var faktisk = (RedirectToRouteResult)controller.Bestillinger();
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.Fly();
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.SeDetaljerBestilling("");
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                var stringResult = controller.EndreFly(null);
                Assert.AreEqual("Ikke admin", stringResult);

                faktisk = (RedirectToRouteResult)controller.SlettFly(0);
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.LagFly();
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.LagFly(null);
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.SlettBestilling("");
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.Flyplasser();
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.LagFlyplass();
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.LagFlyplass(null);
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                stringResult = controller.EndreFlyplass(null);
                Assert.AreEqual("Ikke admin", stringResult);

                faktisk = (RedirectToRouteResult)controller.Ruter();
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.LagRute();
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.LagRute(null);
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                stringResult = controller.EndreRute(null);
                Assert.AreEqual("Ikke admin", stringResult);

                faktisk = (RedirectToRouteResult)controller.SlettRute(0);
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.Kunder();
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.LagKunde();
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.LagKunde(null);
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                stringResult = controller.EndreKunde(null);
                Assert.AreEqual("Ikke admin", stringResult);

                faktisk = (RedirectToRouteResult)controller.SlettKunde(0);
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.Flygninger();
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                stringResult = controller.EndreFlygning(0, new DateTime());
                Assert.AreEqual("Ikke admin", stringResult);

                stringResult = controller.EndreAdmin(null);
                Assert.AreEqual("Ikke admin", stringResult);

                faktisk = (RedirectToRouteResult)controller.LagFlygning();
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.LagFlygning(null);
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.EndreStatusFlygning(0);
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.Administrator();
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.LagAdmin(null);
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                stringResult = controller.EndreFlygning(0, new DateTime());
                Assert.AreEqual("Ikke admin", stringResult);

                faktisk = (RedirectToRouteResult)controller.SlettAdmin(null);
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);
            }
        }

    }
}
