using Flybilletter.DAL.DBModel;
using Flybilletter.DAL.Interfaces;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLFlyplass : IBLLFlyplass
    {
        private IDBFlyplass dbFlyplass;

        public BLLFlyplass()
        {
            dbFlyplass = new DBFlyplass();
        }

        public BLLFlyplass(IDBFlyplass stub)
        {
            this.dbFlyplass = stub;
        }

        public Flyplass Hent(string id)
        {
            return dbFlyplass.Hent(id);
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
