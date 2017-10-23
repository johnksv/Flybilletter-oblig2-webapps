using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Flybilletter.Controllers;
using BLL;
using Flybilletter.DAL.Stub;
using System.Web.Mvc;
using Flybilletter.Model.DomeneModel;
using System.Collections.Generic;

namespace Enhetstesting
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void SokTest()
        {
            var sessionMock = new TestControllerBuilder();
            var bllflyplass = new BLLFlyplass(new DBFlyplassStub());
            var bllflygning = new BLLFlygning(new DBFlygningStub());
            var bllbestilling =new BLLBestilling(new DBBestillingStub());
            var bllkunde = new BLLKunde(new DBKundeStub(), new DBPostnummerStub());


            var controller = new HomeController(bllflyplass, bllflygning, bllbestilling, bllkunde);

            var resultat = (ViewResult) controller.Sok();
            var a = resultat.ViewBag.flyplasser;

            Assert.AreEqual(a.Count, 3);
        }

        [TestMethod]
        public void SokPostTest()
        {

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
        public void HentPoststedTest()
        {

        }

    }
}
