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

namespace Enhetstesting
{
    [TestClass]
    public class AdminControllerTest
    {

        private static AdminController NyAdminControllerMedSession()
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

            return controller;
        }

        [TestMethod]
        public void SkalIkkeKunneLoggeInn()
        {
            var controller = NyAdminControllerMedSession();

            bool faktisk = controller.LoginAttempt("test", "feilPassord");

            Assert.IsFalse(faktisk);

        }

        [TestMethod]
        public void SkalKunneLoggeInn()
        {
            var controller = NyAdminControllerMedSession();
            controller.Session["admin"] = true;

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


    }
}
