using Flybilletter.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.Model.DomeneModel;

namespace Flybilletter.DAL.Stub
{
    public class DBBestillingStub : IDBBestilling
    {
        private List<Bestilling> bestillinger = new List<Bestilling>()
        {
            new Bestilling()
            {
                Referanse ="ARP123"
            },
            new Bestilling()
            {
                Referanse = "AAA123",
                FlygningerTur = new List<Flygning>(){
                    new Flygning() //Kunde kan slette
                    {
                      AvgangsTid = new DateTime(2018,01,01,00,00,00)
                    }
                },
               Bestillingstidspunkt = new DateTime(2017,10,20,12,00,00) //Kunde kan ikke slette
            },
            new Bestilling()
            {
                Referanse = "AAB123",
                FlygningerTur = new List<Flygning>(){
                    new Flygning() //Kunde kan slette
                    {
                        AvgangsTid = new DateTime(2019,01,01,00,00,00)
                    }
                },
                Bestillingstidspunkt = new DateTime(2018,11,27,12,00,00) //Kunde kan slette
            },
            new Bestilling()
            {
                Referanse = "ABB123",
                FlygningerTur = new List<Flygning>(){
                    new Flygning() //Kunde kan ikke slette
                    {
                        AvgangsTid = new DateTime(2017,01,01,00,00,00)
                    }
                },
                Bestillingstidspunkt = new DateTime(2018,11,27,12,00,00) //Kunde kan slette
            },
            new Bestilling()
            {
                Referanse = "ABC123",
                FlygningerTur = new List<Flygning>(){
                    new Flygning() //Kunde kan ikke slette
                {
                    AvgangsTid = new DateTime(2016,01,01,00,00,00)
                } },
                Bestillingstidspunkt = new DateTime(2016,11,27,12,00,00) //Kunde kan ikke slette
            }
        };


        public bool EksistererReferanse(string referanse)
        {
            return FinnBestilling(referanse) != null;
        }

        public Bestilling FinnBestilling(string referanse)
        {
            return bestillinger.FirstOrDefault(best => best.Referanse == referanse);
        }

        public List<Bestilling> HentAlle()
        {
            return bestillinger;
        }

        public void LeggInn(Bestilling bestilling)
        {

        }

        public bool Slett(string referanse)
        {
            var bestilling = bestillinger.FirstOrDefault(best => best.Referanse == referanse);
            if ((DateTime.Now - bestilling.Bestillingstidspunkt) < new TimeSpan(48, 0, 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
