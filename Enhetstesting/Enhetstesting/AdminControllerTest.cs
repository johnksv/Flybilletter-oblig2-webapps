using BLL;
using BLL.Stub;
using Flybilletter.Controllers;
using Flybilletter.DAL.Stub;
using Flybilletter.Model.DomeneModel;
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
            var bllflygning = new BLLFlygning(new DBFlygningStub(), new DBFlyplassStub());
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
                Username = "test",
                Password = "riktigPassord"
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
        public void FlySkalReturnereViewMedModellUtenFeilmelding()
        {
            var controller = NyAdminControllerMedSession(true);

            var faktisk = (ViewResult)controller.Fly();


            Assert.AreEqual("ListFly", faktisk.ViewName);
            Assert.AreNotEqual(null, faktisk.Model);
            Assert.AreEqual(null, faktisk.ViewBag.Feilmelding);
        }

        [TestMethod]
        public void FlySkalReturnereViewMedModellMedFeilmelding()
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
        public void SkalRedirecteTilSokNaarIkkeAdmin()
        {
            var controller = NyAdminControllerMedSession(false);
            controller.Session["admin"] = null;
            for (var i = 0; i < 2; i++)
            {
                //Sjekk først med null, så med false
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

                faktisk = (RedirectToRouteResult)controller.Flyplasser();
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.NyFlyplass();
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.NyFlyplass(null);
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.Ruter();
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.LagRute();
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.LagRute(null);
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.SlettRute(0);
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.Kunder();
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.LagKunde();
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.LeggTilKunde(null);
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.OppdaterKunde(null);
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.SlettKunde(0);
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.Flygninger();
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.OppdaterFlygning(null);
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.LagFlygning();
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.OppdaterStatusFlygning(0);
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);

                faktisk = (RedirectToRouteResult)controller.Administrator();
                Assert.AreEqual("Sok", faktisk.RouteValues["action"]);
            }
        }


    }
}
