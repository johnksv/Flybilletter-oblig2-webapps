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
        private List<Bestilling> bestillinger = new List<Bestilling>()
        {
            new Bestilling()
            {
                Referanse ="ARP123"
            }
        };


        public bool EksistererReferanse(string referanse)
        {
            return FinnBestilling(referanse) != null;
        }

        public Bestilling FinnBestilling(string referanse)
        {
            return bestillinger.FirstOrDefault(best => best.Referanse == referanse);
        }

        public void LeggInn(Bestilling bestilling)
        {
            
        }
    }
}
