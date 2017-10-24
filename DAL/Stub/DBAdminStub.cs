using Flybilletter.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.DAL.DBModel;

namespace Flybilletter.DAL.Stub
{
    public class DBAdminStub : IDBAdmin
    {
        public DBAdmin Hent(string username)
        {
            throw new NotImplementedException();
        }
    }
}
