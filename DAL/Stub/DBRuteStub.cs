using System.Collections.Generic;
using Flybilletter.DAL.Interfaces;
using Flybilletter.Model.DomeneModel;

namespace Flybilletter.DAL.Stub
{
    public class DBRuteStub : IDBRute
    {
        public List<Rute> HentAlle()
        {
            throw new System.NotImplementedException();
        }

        public bool LagreRute(Rute rute)
        {
            throw new System.NotImplementedException();
        }

        public bool LagRute(Rute innrute)
        {
            throw new System.NotImplementedException();
        }

        public bool Slett(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
