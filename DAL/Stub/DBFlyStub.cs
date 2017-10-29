using Flybilletter.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.Model.DomeneModel;

namespace Flybilletter.DAL.Stub
{
    public class DBFlyStub : IDBFly
    {
        private List<Fly> fly = new List<Fly>()
        {
            new Fly()
            {
                ID = 1,
                Modell = "Boeing 737",
                AntallSeter = 150
            },
            new Fly()
            {
                ID = 2,
                Modell = "Boeing 737",
                AntallSeter = 150
            },
            new Fly()
            {
                ID = 3,
                Modell = "Airbus A380",
                AntallSeter = 450
            }
        };
        public Fly Hent(int ID)
        {
            return fly.FirstOrDefault(fly => fly.ID == ID);
        }

        public List<Fly> HentAlle()
        {
            return fly;
        }

        public bool LeggInn(Fly innfly)
        {
            if(innfly != null && innfly.ID > 0 && innfly.Modell != null && innfly.AntallSeter > 0)
            {
                fly.Add(innfly);
                return true;
            }
            return false;
        }

        public bool Endre(Fly innfly)
        {
            return innfly != null && innfly.ID > 0 && innfly.Modell != null && innfly.AntallSeter > 0;
        }

        public bool Slett(int ID)
        {
            var element =  fly.FirstOrDefault(f => f.ID == ID);
            return fly.Remove(element);
        }
    }
}
