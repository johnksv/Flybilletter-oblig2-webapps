using Flybilletter.DAL.DBModel;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.Model.ViewModel;

namespace Flybilletter.DAL.Interfaces
{
    public interface IDBAdmin
    {
        DBAdmin Hent(string username);
        bool LeggInn(Admin admin);
        byte[] HashPassord(string password, string salt);
        List<Admin> HentAlle();
        bool EndrePassord(string username, string password);
        bool Slett(string username);
    }
}
