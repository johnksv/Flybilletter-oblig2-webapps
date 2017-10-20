using Flybilletter.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.DAL.DBModel;
using Flybilletter.Model.DomeneModel;

namespace Flybilletter.DAL.Stub
{
    public class DBKundeStub : IDBKunde
    {
        public bool LeggInn(IEnumerable<Kunde> kunder)
        {
            throw new NotImplementedException();
        }

        public DBKunde LeggInn(Kunde innKunde)
        {
            throw new NotImplementedException();
        }
    }
}
