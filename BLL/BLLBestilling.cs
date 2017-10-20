using Flybilletter.DAL.DBModel;
using Flybilletter.Model.DomeneModel;
using Flybilletter.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLBestilling
    {

        private DBBestilling dbBestilling = new DBBestilling();

        public Bestilling FinnBestilling(string referanse)
        {
            return dbBestilling.FinnBestilling(referanse);
        }

        public bool VerifiserKredittkort(string CVCstring, string utlop, out string feilmelding)
        {
            feilmelding = "";
            int cvc = 0;
            bool ok = int.TryParse(CVCstring, out cvc);
            if (!ok || cvc < 100 || cvc > 999)
            {
                feilmelding = "Ugyldig CVC. Må være mellom 100 og 999";
                return false;
            }


            var regex = new Regex(@"^([0-9]{2})\-([0-9]{2})$");
            Match match = regex.Match(utlop);
            bool resultat = false;
            if (match.Success)
            {
                int mnd = int.Parse(match.Groups[1].Value);
                int aar = int.Parse(match.Groups[2].Value);

                if (mnd > 0 && mnd <= 12) //Sjekk at mnd er OK
                {
                    resultat = true;
                    if (aar < DateTime.Now.Year - 2000) //Sjekk at vi ikke er i fortiden
                    {
                        feilmelding = "Ugyldig utløp. Kortet kan ikke være utløpt.";
                        resultat = false;
                    }
                    else if (aar == DateTime.Now.Year - 2000) //Om kortet går ut samme år som nå sjekker vi at mnd er OK
                    {
                        int currmnd = DateTime.Now.Month;
                        if (mnd < currmnd)
                        {
                            feilmelding = "Ugyldig utløp. Kortet kan ikke være utløpt.";
                            resultat = false;
                        }
                    }
                }
            }

            return resultat;
        }

        public Bestilling LeggInn(List<Kunde> kunder, BestillingViewModel gjeldende)
        {
            var bestilling = new Bestilling()
            {
                BestillingsTidspunkt = DateTime.Now,
                FlygningerTur = new List<Flygning>(),
                Passasjerer = kunder,
                Totalpris = gjeldende.Totalpris
            };

            do //Lag en unik UUID helt til det ikke finnes i databasen fra før.
            {
                bestilling.Referanse = Guid.NewGuid().ToString().ToUpper().Substring(0, 6);
            } while (dbBestilling.FinnBestilling(bestilling.Referanse) != null);


            var dbflygning = new DBFlygning();
            foreach (var flygning in gjeldende.Tur.Flygninger)
            {
                Flygning dbFlygning = dbflygning.Finn(flygning.ID);
                if (dbFlygning == null) throw new InvalidOperationException("Ugyldig flygning"); //Det skjedde en feil

                bestilling.FlygningerTur.Add(dbFlygning);
            }

            if (gjeldende.Retur != null)
            {
                bestilling.FlygningerRetur = new List<Flygning>();
                foreach (var flygning in gjeldende.Retur.Flygninger)
                {
                    var dbFlygning = dbflygning.Finn(flygning.ID);
                    if (dbFlygning == null) throw new InvalidOperationException("Ugyldig flygning");

                    bestilling.FlygningerRetur.Add(dbFlygning);
                }
            }

            dbBestilling.LeggInn(bestilling);


            return bestilling;
        }

        public bool EksistererReferanse(string referanse)
        {
            return dbBestilling.EksistererReferanse(referanse);
        }
    }
}
