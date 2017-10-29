using Flybilletter.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.Model.DomeneModel;

namespace Flybilletter.DAL.Stub
{
    public class DBFlygningStub : IDBFlygning
    {
        private Rute BOOOSL = new Rute()
        {
            Fra = BOO,
            Til = OSL,
            BasePris = 1199,
            Reisetid = new TimeSpan(1, 30, 0)
        };

        private List<Flygning> flygninger = new List<Flygning>()
        {
            new Flygning()
            {
                ID =1,
                AvgangsTid = DateTime.Now,
                Fly = Boeing737_1,
                Rute = BOOOSL,
                Kansellert = false
            }
        };
        public bool Endre(int id, DateTime nyAvgangstid)
        {
            throw new NotImplementedException();
        }

        public Flygning Finn(int ID)
        {
            if (ID > 0) return new Flygning();
            return null;
        }

        public List<Flygning> HentAlle()
        {
            return flygninger;
        }

        public List<Flygning> HentAlle(DateTime dateTime)
        {
            return flygninger.Where(f => f.AvgangsTid >= dateTime).ToList();
        }

        public Flygning Hent(int id)
        {
            return flygninger.FirstOrDefault(f => f.ID == id);
        }

        public List<Flygning> HentFlygningerFra(Flyplass flyplass)
        {
            throw new NotImplementedException();
        }

        public List<Flygning> HentFlygningerTil(Flyplass flyplass)
        {
            throw new NotImplementedException();
        }

        public bool LeggInn(Flygning flygning)
        {
            throw new NotImplementedException();
        }

        public bool Endre(Flygning flygning)
        {
            throw new NotImplementedException();
        }

        public bool EndreStatus(int id)
        {
            throw new NotImplementedException();
        }
    }
}
