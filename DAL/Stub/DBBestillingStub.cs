using Flybilletter.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.Model.DomeneModel;

namespace Flybilletter.DAL.Stub
{
    public class DBBestillingStub : IDBBestilling
    {
        public bool EksistererReferanse(string referanse)
        {
            throw new NotImplementedException();
        }

        public Bestilling FinnBestilling(string referanse)
        {
            Bestilling bestilling = null;
            if (referanse == null) return bestilling;

            if (referanse.Length == 6) bestilling = new Bestilling();

            return bestilling;
        }

        public void LeggInn(Bestilling bestilling)
        {
            throw new NotImplementedException();
        }
    }
}
