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
        public Fly Hent(int iD)
        {
            throw new NotImplementedException();
        }

        public List<Fly> HentAlle()
        {
            throw new NotImplementedException();
        }

        public bool LeggTil(Fly fly)
        {
            throw new NotImplementedException();
        }

        public bool Oppdater(Fly fly)
        {
            throw new NotImplementedException();
        }

        public bool Slett(int ID)
        {
            throw new NotImplementedException();
        }
    }
}
