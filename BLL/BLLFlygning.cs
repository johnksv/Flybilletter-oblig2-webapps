using Flybilletter.DAL.DBModel;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLFlygning
    {

        public static List<Reise> FinnReiseforslag(string fraFlyplassID, string tilFlyplassID, DateTime avreiseDag)
        {
            //TODO: Om vi implementerer quickgraph vil denne funksjonen være lettere
            List<Reise> reiseMuligheter = new List<Reise>();


            var fraFlyplass = DBFlyplass.Hent(fraFlyplassID); // db.Flyplasser.Where(flyplass => flyplass.ID == fraFlyplassID).First(); //Hvis du tweaket i HTML-koden fortjener du ikke feilmelding
            var tilFlyplass = DBFlyplass.Hent(tilFlyplassID); //db.Flyplasser.Where(flyplass => flyplass.ID == tilFlyplassID).First();

            bool ugyldigAvreiseTidspunkt = avreiseDag.Date < DateTime.Now.Date;
            if (fraFlyplass == null || tilFlyplass == null || ugyldigAvreiseTidspunkt) return reiseMuligheter;

            List<Flygning> fraListe = DBFlygning.HentFlygningerFra(fraFlyplass);
            List<Flygning> tilListe = DBFlygning.HentFlygningerTil(tilFlyplass);

            foreach (Flygning fraFlygning in fraListe)
            {
                bool direkteFlygning = fraFlygning.Rute.Til.ID == tilFlyplass.ID;
                if (direkteFlygning)
                {
                    if (fraFlygning.AvgangsTid.Date == avreiseDag.Date)
                        reiseMuligheter.Add(new Reise(fraFlygning));
                }
                else
                {
                    foreach (Flygning tilFlygning in tilListe)
                    {
                        if (fraFlygning.Rute.Til.ID == tilFlygning.Rute.Fra.ID && fraFlygning.AvgangsTid.Date == avreiseDag.Date &&
                            (tilFlygning.AvgangsTid - fraFlygning.AnkomstTid) >= new TimeSpan(1, 0, 0))
                        {
                            reiseMuligheter.Add(new Reise(fraFlygning, tilFlygning));
                            break;
                        }
                    }
                }
            }

            return reiseMuligheter;
        }
    }
}
