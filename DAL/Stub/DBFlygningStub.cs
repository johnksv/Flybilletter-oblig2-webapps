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
            throw new NotImplementedException();
        }

        public List<Flygning> HentAlle(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public Flygning Hent(int id)
        {
            throw new NotImplementedException();
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
