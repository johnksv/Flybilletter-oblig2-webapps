using Flybilletter.DAL.DBModel;
using Flybilletter.DAL.Interfaces;
using Flybilletter.Model.DomeneModel;
using Flybilletter.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLFlygning : IBLLFlygning
    {
        private IDBFlygning dbFlygning;
        private IDBFlyplass dbflyplass;

        public BLLFlygning()
        {
            dbFlygning = new DBFlygning();
            dbflyplass = new DBFlyplass();
        }

        public BLLFlygning(IDBFlygning stub, IDBFlyplass flyplassStub)
        {
            dbFlygning = stub;
            dbflyplass = flyplassStub;
        }

        public List<Reise> FinnReiseforslag(string fraFlyplassID, string tilFlyplassID, DateTime avreiseDag)
        {
            //TODO: Om vi implementerer quickgraph vil denne funksjonen være lettere
            List<Reise> reiseMuligheter = new List<Reise>();

            
            var fraFlyplass = dbflyplass.Hent(fraFlyplassID); // db.Flyplasser.Where(flyplass => flyplass.ID == fraFlyplassID).First(); //Hvis du tweaket i HTML-koden fortjener du ikke feilmelding
            var tilFlyplass = dbflyplass.Hent(tilFlyplassID); //db.Flyplasser.Where(flyplass => flyplass.ID == tilFlyplassID).First();

            bool ugyldigAvreiseTidspunkt = avreiseDag.Date < DateTime.Now.Date;
            if (fraFlyplass == null || tilFlyplass == null || ugyldigAvreiseTidspunkt) return reiseMuligheter;

            List<Flygning> fraListe = dbFlygning.HentFlygningerFra(fraFlyplass);
            List<Flygning> tilListe = dbFlygning.HentFlygningerTil(tilFlyplass);

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

        public void FinnReisemuligheter(SokViewModel innSok, out FlygningerViewModel reiser, out List<Reise> flygningerTur, out List<Reise> flygningerRetur)
        {
            string fraFlyplass = innSok.Fra;
            string tilFlyplass = innSok.Til;

            flygningerTur = FinnReiseforslag(fraFlyplass, tilFlyplass, innSok.Avreise);
            flygningerRetur = FinnReiseforslag(tilFlyplass, fraFlyplass, innSok.Retur);

            reiser = new FlygningerViewModel()
            {
                TurMuligheter = flygningerTur,
                ReturMuligheter = flygningerRetur,
                TurRetur = innSok.Retur >= innSok.Avreise
            };
        }

        public List<Flygning> HentAlle(DateTime dateTime)
        {
            return dbFlygning.HentAlle(dateTime);
        }

        public Flygning HentEnFlygning(int id)
        {
            return dbFlygning.HentEnFlygning(id);
        }

        public bool OppdaterFlygning(Flygning flygning)
        {
            return dbFlygning.OppdaterFlygning(flygning);
        }
    }
}
