﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Flybilletter.Controllers;
using BLL;
using Flybilletter.DAL.Stub;
using System.Web.Mvc;
using Flybilletter.Model.DomeneModel;
using System.Collections.Generic;
using Flybilletter.Model.ViewModel;
using System.Web.Helpers;

namespace Enhetstesting
{
    [TestClass]
    public class HomeControllerTest
    {

        private static HomeController NyHomeControllerMedSession()
        {
            var bllflyplass = new BLLFlyplass(new DBFlyplassStub());
            var bllflygning = new BLLFlygning(new DBFlygningStub(), new DBFlyplassStub(), new DBRuteStub(), new DBFlyStub());
            var bllbestilling = new BLLBestilling(new DBBestillingStub(), new DBFlygningStub());
            var bllkunde = new BLLKunde(new DBKundeStub(), new DBPostnummerStub());

            var sessionMock = new TestControllerBuilder();
            var controller = new HomeController(bllflyplass, bllflygning, bllbestilling, bllkunde);
            sessionMock.InitializeController(controller);
            return controller;
        }

        [TestMethod]
        public void SokSkalKunneHenteUtAlleFlyplasser()
        {
            var controller = NyHomeControllerMedSession();

            var faktisk = (ViewResult)controller.Sok();
            var flyplasser = faktisk.ViewBag.flyplasser;

            Assert.AreEqual("", faktisk.ViewName);
            Assert.AreEqual(3, flyplasser.Count);
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

            var controller = NyHomeControllerMedSession();


            var faktisk = (PartialViewResult)controller.Sok(model);

            Assert.AreEqual("_Flygninger", faktisk.ViewName);
            Assert.AreNotEqual(null, faktisk.Model);
        }


        [TestMethod]
        public void SokMedFeilParameter()
        {
            var model = new SokViewModel();

            var controller = NyHomeControllerMedSession();

            var faktisk = (PartialViewResult)controller.Sok(model);

            Assert.AreEqual("_Flygninger", faktisk.ViewName);
            Assert.AreEqual(null, faktisk.Model);
        }


        [TestMethod]
        public void ValgtReiseGyldigeParametre()
        {
            var controller = NyHomeControllerMedSession();
            

          var reiseliste = new List<Reise>()
            {
                GenererNyReise()
            };

            controller.Session["returListe"] = controller.Session["turListe"] = reiseliste;
            controller.Session["antallbilletter"] = 1;


            var faktisk = (ViewResult) controller.ValgtReise("0", "0");
            var faktiskModel = (BestillingViewModel) faktisk.Model;


            Assert.AreEqual("BestillingDetaljer", faktisk.ViewName);
            Assert.AreEqual(reiseliste[0], faktiskModel.Tur);
        }

        [TestMethod]
        public void ValgtReiseUgyldigeParametre()
        {

            var controller = NyHomeControllerMedSession();
            

            controller.Session["turListe"] = new List<Reise>()
            {
                GenererNyReise()
            };


            var faktisk = (RedirectToRouteResult)controller.ValgtReise("-1", "-1");

            Assert.IsTrue(faktisk.RouteValues.ContainsKey("action"));
            Assert.AreEqual("Sok", faktisk.RouteValues["action"]);
        }

        [TestMethod]
        public void ValgtReiseUgyldigTurIndeks()
        {
            var controller = NyHomeControllerMedSession();


            controller.Session["turListe"] = new List<Reise>()
            {
                GenererNyReise()
            };
            controller.Session["returListe"] = new List<Reise>();


            var faktisk = (RedirectToRouteResult) controller.ValgtReise("-1", "-1");

            Assert.IsTrue(faktisk.RouteValues.ContainsKey("action"));
            Assert.AreEqual("Sok", faktisk.RouteValues["action"]);
        }

        [TestMethod]
        public void ValgtReiseUgyldigReturIndeks()
        {
            var controller = NyHomeControllerMedSession();

            controller.Session["returListe"] = controller.Session["turListe"] = new List<Reise>()
            {
                GenererNyReise()
            };


            var faktisk = (RedirectToRouteResult)controller.ValgtReise("0", "1");

            Assert.IsTrue(faktisk.RouteValues.ContainsKey("action"));
            Assert.AreEqual("Sok", faktisk.RouteValues["action"]);
        }


        [TestMethod]
        public void UgyldigeKunder()
        {
            var controller = NyHomeControllerMedSession();

            var kunder = new List<Kunde>()
            {
                new Kunde ()
            };

            controller.ViewData.ModelState.AddModelError("Kunde.Fornavn", "Ikke oppgitt fornavn");
            string faktisk = controller.Kunde(kunder);
            string forventet = "En eller flere kunder har ugyldig state. Sjekk informasjonen på nytt.";

            Assert.AreEqual(forventet, faktisk);
        }


        [TestMethod]
        public void GyldigeKunder()
        {
            var controller = NyHomeControllerMedSession();

            var kunder = new List<Kunde>()
            {
                new Kunde ()
                {
                    Fornavn = "Ola",
                    Etternavn = "Nordmann",
                    Adresse ="Osloveien 11",
                    EPost ="Ola@nordmann.no",
                    Fodselsdag = new DateTime(1984,1,2,0,0,0),
                    Postnr = "0001",
                    Poststed ="Oslo",
                    Tlf = "12345678"
                }
            };

            string faktisk = controller.Kunde(kunder);
            var faktiskKunder = controller.Session["KunderBestilling"];

            Assert.AreEqual("success", faktisk);
            Assert.AreEqual(kunder, faktiskKunder);
        }


        [TestMethod]
        public void GenererReferanseMedRiktigModell()
        {
            var controller = NyHomeControllerMedSession();

            controller.Session["GjeldendeBestilling"] = new BestillingViewModel()
            {
                Tur = GenererNyReise()
            };

            var model = new BestillingViewModel()
            {
                Kredittkort = new KredittkortViewModel()
                {
                    CVC = 123,
                    Kortholder = "Ola Nordmann",
                    Kortnummer = 1234567891234567,
                    Utlop = "11-21"
                }
            };
            var faktisk = (RedirectToRouteResult)controller.GenererReferanse(model);

            var tempData = (Bestilling)controller.TempData["bestilling"];

            Assert.IsTrue(faktisk.RouteValues.ContainsKey("action"));
            Assert.AreEqual("Kvittering", faktisk.RouteValues["action"]);

        }

        [TestMethod]
        public void GenererReferanseMedFeilModell()
        {
            var controller = NyHomeControllerMedSession();

            var model = new BestillingViewModel()
            {
                Kredittkort = new KredittkortViewModel()
            };

            controller.ViewData.ModelState.AddModelError("Kortholder", "Ikke oppgitt fornavn");

            var faktisk = (ViewResult)controller.GenererReferanse(model);

            Assert.AreEqual("BetalingFeilet", faktisk.ViewName);

        }

        [TestMethod]
        public void GenererReferanseMedUgyldigKredittkort()
        {
            var controller = NyHomeControllerMedSession();

            var model = new BestillingViewModel()
            {
                Kredittkort = new KredittkortViewModel()
                {
                    CVC = 123,
                    Kortholder = "",
                    Kortnummer = 1,
                    Utlop = ""

                }
            };

            var faktisk = (ViewResult)controller.GenererReferanse(model);

            Assert.AreEqual("BetalingFeilet", faktisk.ViewName);
            Assert.IsNotNull(faktisk.ViewBag.Feilmelding);
        }



        [TestMethod]
        public void ReferanseSokDerReferanseEksisterer()
        {
            var controller = NyHomeControllerMedSession();

            var faktisk = (ViewResult)controller.ReferanseSok("ARP123");

            Assert.AreNotEqual(null, faktisk.Model);
            Assert.IsTrue(faktisk.Model is Bestilling, "Model er ikke av type Bestilling");
        }

        [TestMethod]
        public void ReferanseSokDerReferanseIkkeEksisterer()
        {
            var controller = NyHomeControllerMedSession();
            var faktisk = (ViewResult)controller.ReferanseSok(null);

            Assert.AreEqual(null, faktisk.Model);
        }



        [TestMethod]
        public void ReferanseEksisterer()
        {
            var controller = NyHomeControllerMedSession();

            string json = controller.ReferanseEksisterer("ARP123");
            var faktisk = Json.Decode(json);

            Assert.AreEqual("True", faktisk.Exists);
            Assert.AreEqual("/Home/ReferanseSok?referanse=ARP123", faktisk.Url);
        }

        [TestMethod]
        public void ReferanseEksistererIkke()
        {
            var controller = NyHomeControllerMedSession();

            string json = controller.ReferanseEksisterer(null);
            var faktisk = Json.Decode(json);

            Assert.AreEqual("False", faktisk.Exists);
            Assert.AreEqual("", faktisk.Url);
        }

        [TestMethod]
        public void Kvittering()
        {
            var controller = NyHomeControllerMedSession();
            var faktisk = (ViewResult)controller.Kvittering();
            Assert.AreEqual("", faktisk.ViewName);
            Assert.AreEqual(null, faktisk.Model);
        }

        [TestMethod]
        public void HentPoststedNaarEksiterer()
        {
            var controller = NyHomeControllerMedSession(); ;
            string faktisk = controller.HentPoststed("0001");

            Assert.AreEqual("OSLO", faktisk);

        }

        [TestMethod]
        public void HentPoststedNaarIkkeEksiterer()
        {
            var controller = NyHomeControllerMedSession();
            string faktisk = controller.HentPoststed("0000");

            Assert.AreEqual("UGYLDIG POSTNUMMER", faktisk);
        }

        [TestMethod]
        public void SlettBestillingFoer48TimerRiktigFlyIkkeGaatt()
        {
            var controller = NyHomeControllerMedSession();
            var faktisk = (RedirectToRouteResult)controller.SlettBestilling("AAB123");

            Assert.AreEqual("Sok", faktisk.RouteValues["action"]);
        }

        [TestMethod]
        public void SlettBestillingFoer48TimerFeilFlyIkkeGaatt()
        {
            var controller = NyHomeControllerMedSession();
            var faktisk = (RedirectToRouteResult)controller.SlettBestilling("AAA123");

            Assert.AreEqual("ReferanseSok", faktisk.RouteValues["action"]);
        }

        [TestMethod]
        public void SlettBestilling48TimerRiktigFlyGaatt()
        {
            var controller = NyHomeControllerMedSession();
            var faktisk = (RedirectToRouteResult)controller.SlettBestilling("ABB123");

            Assert.AreEqual("ReferanseSok", faktisk.RouteValues["action"]);
        }

        [TestMethod]
        public void SlettBestilling48TimerFeilFlyGaatt()
        {
            var controller = NyHomeControllerMedSession();
            var faktisk = (RedirectToRouteResult)controller.SlettBestilling("ABC123");

            Assert.AreEqual("ReferanseSok", faktisk.RouteValues["action"]);
        }
        private static Reise GenererNyReise()
        {
            var rute = new Rute()
            {
                Fra = new Flyplass()
                {
                    ID = "OSL",
                    By = "Oslo",
                    Land = "Norge",
                    Navn = "Gardermoen"
                },
                Til = new Flyplass()
                {
                    ID = "BOO",
                    By = "Bodø",
                    Land = "Norge",
                    Navn = "Bodø Lufthavn"
                },
                BasePris = 1099,
                Reisetid = new TimeSpan(1,0,0)
            };

            var flygning = new Flygning()
            {
                ID = 10,
                Rute = rute,
                AvgangsTid = DateTime.Now.AddHours(1),
                Fly = new Fly()
                {
                    Modell = "Boieng 737",
                    AntallSeter = 150,
                }

            };


            return new Reise(flygning);
        }

    }
}
