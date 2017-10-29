using Flybilletter.DAL.DBModel;
using Flybilletter.DAL.Interfaces;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLFlyplass : IBLLFlyplass
    {
        private IDBFlyplass dbFlyplass;

        [ExcludeFromCodeCoverage]
        public BLLFlyplass()
        {
            dbFlyplass = new DBFlyplass();
        }

        public BLLFlyplass(IDBFlyplass stub)
        {
            this.dbFlyplass = stub;
        }

        public bool Endre(Flyplass item)
        {
            return dbFlyplass.Endre(item);
        }

        public List<Flyplass> HentAlle()
        {
            return dbFlyplass.HentAlle();
        }

        public bool LeggInn(Flyplass flyplass)
        {
            return dbFlyplass.LeggInn(flyplass);
        }
    }
}
