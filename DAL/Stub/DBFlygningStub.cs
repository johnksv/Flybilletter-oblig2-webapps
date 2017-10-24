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
        public Flygning Finn(int ID)
        {
            if (ID > 0) return new Flygning();
            return null;
        }

        public List<Flygning> HentFlygningerFra(Flyplass flyplass)
        {
            throw new NotImplementedException();
        }

        public List<Flygning> HentFlygningerTil(Flyplass flyplass)
        {
            throw new NotImplementedException();
        }
    }
}
