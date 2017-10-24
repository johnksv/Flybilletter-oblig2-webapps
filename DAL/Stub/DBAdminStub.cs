using Flybilletter.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.DAL.DBModel;
using Model.DomeneModel;

namespace Flybilletter.DAL.Stub
{
    public class DBAdminStub : IDBAdmin
    {
        public DBAdminStub()
        {
        }

        public byte[] HashPassord(string password, string salt)
        {
            throw new NotImplementedException();
        }

        public DBAdmin Hent(string username)
        {
            throw new NotImplementedException();
        }

        public bool LeggInn(Admin admin)
        {
            throw new NotImplementedException();
        }
    }
}
