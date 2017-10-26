using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.Model.DomeneModel;
using Flybilletter.DAL.DBModel;
using Flybilletter.DAL.Interfaces;

namespace BLL
{
    public class BLLRute : IBLLRute
    {
        private IDBRute dbRute;

        public BLLRute() : this(new DBRute())
        {
        }

        public BLLRute(IDBRute rutestub)
        {
            this.dbRute = rutestub;
        }

        public List<Rute> HentAlle()
        {
            return dbRute.HentAlle();
        }

        public bool LagreRute(Rute rute)
        {
            return dbRute.LagreRute(rute);
        }

        public bool Slett(int id)
        {
            return dbRute.Slett(id);
        }
    }
}
