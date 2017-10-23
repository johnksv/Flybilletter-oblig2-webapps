using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Flybilletter.Controllers;
using BLL;
using Flybilletter.DAL.Stub;
using System.Web.Mvc;
using Flybilletter.Model.DomeneModel;
using System.Collections.Generic;
using Flybilletter.Model.ViewModel;

namespace Enhetstesting
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void SokSkalKunneHenteUtAlleFlyplasser()
        {
            var bllflyplass = new BLLFlyplass(new DBFlyplassStub());
            var bllflygning = new BLLFlygning(new DBFlygningStub(), new DBFlyplassStub());
            var bllbestilling = new BLLBestilling(new DBBestillingStub());
            var bllkunde = new BLLKunde(new DBKundeStub(), new DBPostnummerStub());

            var controller = new HomeController(bllflyplass, bllflygning, bllbestilling, bllkunde);

            var faktisk = (ViewResult)controller.Sok();
            var flyplasser = faktisk.ViewBag.flyplasser;

            Assert.AreEqual(flyplasser.Count, 3);
        }

        [TestMethod]
        public void SokSkalKunneGiEnListeFlygningerOmGyldigModell()
        {
            var model = new SokViewModel()
            {
                AntallBilletter = 1,
                Avreise = new DateTime(2017, 10, 20, 12, 0, 0),
                Fra = "OSL",
                Til = "BOO"
            };

            var sessionMock = new TestControllerBuilder();
            var bllflyplass = new BLLFlyplass(new DBFlyplassStub());
            var bllflygning = new BLLFlygning(new DBFlygningStub(), new DBFlyplassStub());
            var bllbestilling = new BLLBestilling(new DBBestillingStub());
            var bllkunde = new BLLKunde(new DBKundeStub(), new DBPostnummerStub());

            var controller = new HomeController(bllflyplass, bllflygning, bllbestilling, bllkunde);

            sessionMock.InitializeController(controller);
            var faktisk = (PartialViewResult)controller.Sok(model);

            Assert.AreEqual("_Flygninger", faktisk.ViewName);
            Assert.AreNotEqual(null, faktisk.Model);
        }


        [TestMethod]
        public void SokMedFeilParameter()
        {
            var model = new SokViewModel();


            var bllflyplass = new BLLFlyplass(new DBFlyplassStub());
            var bllflygning = new BLLFlygning(new DBFlygningStub(), new DBFlyplassStub());
            var bllbestilling = new BLLBestilling(new DBBestillingStub());
            var bllkunde = new BLLKunde(new DBKundeStub(), new DBPostnummerStub());

            var controller = new HomeController(bllflyplass, bllflygning, bllbestilling, bllkunde);

            var faktisk = (PartialViewResult)controller.Sok(model);

            Assert.AreEqual("_Flygninger", faktisk.ViewName);
            Assert.AreEqual(null, faktisk.Model);
        }

        [TestMethod]
        public void ValgtReiseTest()
        {

        }

        [TestMethod]
        public void KundePostTest()
        {

        }


        [TestMethod]
        public void GenererReferansePostTest()
        {

        }

        [TestMethod]
        public void ReferanseSokTest()
        {

        }

        [TestMethod]
        public void KvitteringTest()
        {

        }

        [TestMethod]
        public void ReferanseEksistererTest()
        {

        }

        [TestMethod]
        public void HentPoststedNaarEksiterer()
        {
            var bllflyplass = new BLLFlyplass(new DBFlyplassStub());
            var bllflygning = new BLLFlygning(new DBFlygningStub(), new DBFlyplassStub());
            var bllbestilling = new BLLBestilling(new DBBestillingStub());
            var bllkunde = new BLLKunde(new DBKundeStub(), new DBPostnummerStub());

            var controller = new HomeController(bllflyplass, bllflygning, bllbestilling, bllkunde);
            string faktisk = controller.HentPoststed("0001");

            Assert.AreEqual("OSLO", faktisk);

        }

        [TestMethod]
        public void HentPoststedNaarIkkeEksiterer()
        {
            var bllflyplass = new BLLFlyplass(new DBFlyplassStub());
            var bllflygning = new BLLFlygning(new DBFlygningStub(), new DBFlyplassStub());
            var bllbestilling = new BLLBestilling(new DBBestillingStub());
            var bllkunde = new BLLKunde(new DBKundeStub(), new DBPostnummerStub());

            var controller = new HomeController(bllflyplass, bllflygning, bllbestilling, bllkunde);
            string faktisk = controller.HentPoststed("0000");

            Assert.AreEqual("UGYLDIG POSTNUMMER", faktisk);
        }

    }
}
