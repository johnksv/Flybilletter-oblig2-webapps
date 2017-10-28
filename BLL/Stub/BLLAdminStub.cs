using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.Model.DomeneModel;

namespace BLL.Stub
{
    public class BLLAdminStub : IBLLAdmin
    {
        //Vi simulrerer DBStub her, på grunn av at DBAdmin fungerer på en annen måte enn de andre databaseklassene (grunnet kryptering).
        private List<Admin> adminer = new List<Admin>();

        public bool IsPassordGyldig(string Brukernavn, string PwAttempt)
        {
            var admin = adminer.FirstOrDefault(a => a.Username == Brukernavn);
            if(admin != null)
            {
                return admin.Password == PwAttempt;
            }
            return false;
        }

        public bool LeggInn(Admin admin)
        {
            var eksisterer = adminer.Where(a => a.Username == admin.Username).Any();
            if (eksisterer || admin.Password == "")
            {
                return false;
            }

            adminer.Add(admin);
            return true;

        }
    }
}
