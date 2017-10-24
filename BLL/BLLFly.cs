using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.Model.DomeneModel;
using Flybilletter.DAL.Interfaces;
using Flybilletter.DAL.DBModel;

namespace BLL
{
    public class BLLFly : IBLLFly
    {
        private IDBFly dbFly;

        public BLLFly()
        {
            dbFly = new DBFly();
        }

        public BLLFly(IDBFly stub)
        {
            dbFly = stub;
        }

        public Fly Hent(int iD)
        {
            return dbFly.Hent(iD);
        }

        public List<Fly> HentAlle()
        {
            return dbFly.HentAlle();
        }

        public bool Oppdater(Fly fly)
        {
            return dbFly.Oppdater(fly);
        }
    }
}
