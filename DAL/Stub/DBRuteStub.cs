using System.Collections.Generic;
using Flybilletter.DAL.Interfaces;
using Flybilletter.Model.DomeneModel;
using System.Linq;

namespace Flybilletter.DAL.Stub
{
    public class DBRuteStub : IDBRute
    {
        private List<Rute> ruter = new List<Rute>()
        {
            new Rute()
            {
                ID = 1,
                Fra =  new Flyplass()
                {
                    ID = "OSL",
                    By = "Oslo",
                    Land = "Norge",
                    Navn = "Gardermoen Lufthavn"

                },
                Til =  new Flyplass()
                {
                    ID = "BOO",
                    By = "Bodø",
                    Land = "Norge",
                    Navn = "Bodø Lufthavn"
                },
                BasePris=1499,
                Reisetid = new System.TimeSpan(1,0,0)
            }
        };

        public Rute Hent(int ruteID)
        {
            throw new System.NotImplementedException();
        }

        public List<Rute> HentAlle()
        {
            return ruter;
        }

        public bool Endre(Rute rute)
        {
            return ruter.FirstOrDefault(r => r.ID == rute.ID) != null;
        }

        public bool LeggInn(Rute innrute)
        {
            return innrute != null && innrute.Reisetid != null && innrute.Til != null && innrute.Til.ID != "" && innrute.Fra != null && innrute.Fra.ID != "" && innrute.BasePris > 0;
        }

        public bool Slett(int id)
        {
            return ruter.FirstOrDefault(r => r.ID == id) != null;
        }
    }
}
