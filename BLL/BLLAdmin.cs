using Flybilletter.DAL.DBModel;
using Flybilletter.DAL.Interfaces;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.Model.ViewModel;

namespace BLL
{
    public class BLLAdmin : IBLLAdmin
    {
        private IDBAdmin dbAdmin;
        public BLLAdmin() : this(new DBAdmin())
        {
        }

        public BLLAdmin(IDBAdmin stub)
        {
            dbAdmin = stub;
        }

        public bool EndrePassord(AdminPassordViewModel adminViewModel)
        {
            if (IsPassordGyldig(adminViewModel.Username, adminViewModel.Gammelt)) 
            {
                if (adminViewModel.Nytt.Equals(adminViewModel.NyttBekreft))
                {
                    return dbAdmin.EndrePassord(adminViewModel.Username, adminViewModel.Nytt);
                }
            }
            return false;
        }

        public List<Admin> Hent()
        {
            return dbAdmin.HentAlle();
        }

        public bool IsPassordGyldig(string Username, string PwAttempt) //Username og PWAttempt er klartekst
        {
            var admin = dbAdmin.Hent(Username);

            if (admin != null)
            {
                var cipherText = dbAdmin.HashPassord(PwAttempt, admin.Salt);
                var i = 0;
                foreach (byte b in cipherText)
                {
                    if (!b.Equals(admin.Password[i])) return false;
                    i++;
                }
                return true;
            }
            return false;
        }

        public bool LeggInn(Admin admin)
        {
            return dbAdmin.LeggInn(admin);
        }
    }
}
