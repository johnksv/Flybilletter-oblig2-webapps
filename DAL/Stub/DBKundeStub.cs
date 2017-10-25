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
        public List<Kunde> HentAlle()
        {
            throw new NotImplementedException();
        }

        public Kunde HentEnKunde(int id)
        {
            throw new NotImplementedException();
        }

        public bool LeggInn(IEnumerable<Kunde> kunder)
        {
            throw new NotImplementedException();
        }

        public DBKunde LeggInn(Kunde innKunde)
        {
            throw new NotImplementedException();
        }

        public bool Oppdater(Kunde kunde)
        {
            throw new NotImplementedException();
        }

        public bool SlettKunde(int id)
        {
            throw new NotImplementedException();
        }
    }
}
