using Flybilletter.DAL.DBModel;
using Flybilletter.DAL.Interfaces;
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
                if (cipherText == admin.Password) return true;
            }
            return false;
        }

        private string HashPassord(string password, string salt)
        {
            var algorithm = System.Security.Cryptography.SHA256.Create();
            byte[] str = Encoding.ASCII.GetBytes(String.Concat(password, salt));

            return Encoding.Default.GetString(algorithm.ComputeHash(str));
        }
    }
}
