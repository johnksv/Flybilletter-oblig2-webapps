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
        public void SkalRedirecteTilSokNaarIkkeAdmin()
        {
            var controller = NyAdminControllerMedSession(false);
            controller.Session["admin"] = null;
            for (var i = 0; i < 2; i++)
            {
                //Sjekk først med null, så med false
                if(i == 0)
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
