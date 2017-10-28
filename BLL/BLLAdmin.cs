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
    public class BLLAdmin : IBLLAdmin
    {
        private IDBAdmin dbAdmin;
        public BLLAdmin()
        {
            dbAdmin = new DBAdmin();
        }

        public BLLAdmin(IDBAdmin stub)
        {
            dbAdmin = stub;
        }

        public bool IsPassordGyldig(string Username, string PwAttempt) //Username og PWAttempt er klartekst
        {
            var admin = dbAdmin.Hent(Username);

            if (admin != null)
            {
                var cipherText = HashPassord(PwAttempt, admin.Salt);
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



        private byte[] HashPassord(string password, string salt)
        {
            return dbAdmin.HashPassord(password, salt);
        }

    }
}
