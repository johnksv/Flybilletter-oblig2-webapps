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
    }
}
