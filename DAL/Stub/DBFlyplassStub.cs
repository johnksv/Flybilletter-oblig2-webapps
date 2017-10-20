using Flybilletter.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.Model.DomeneModel;

namespace Flybilletter.DAL.Stub
{
    public class DBFlyplassStub : IDBFlyplass
    {
        public Flyplass Hent(string tilFlyplassID)
        {
            throw new NotImplementedException();
        }

        public List<Flyplass> HentAlle()
        {
            throw new NotImplementedException();
        }
    }
}
