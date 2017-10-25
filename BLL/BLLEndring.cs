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
    public class BLLEndring : IBLLEndring
    {
        private IDBEndring dbEndring;

        public BLLEndring()
        {
            dbEndring = new DBEndring();
        }

        public BLLEndring(IDBEndring endringStub)
        {
            dbEndring = endringStub;
        }

        public List<Endring> Hent()
        {
            return dbEndring.Hent();
        }
    }
}
