using Flybilletter.DAL.DBModel;
using Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flybilletter.DAL.Interfaces
{
    public interface IDBAdmin
    {
        DBAdmin Hent(string username);
        bool LeggInn(Admin admin);
        byte[] HashPassord(string password, string salt);
    }
}
