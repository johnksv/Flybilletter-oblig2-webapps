using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.Model.DomeneModel;
using Flybilletter.Model.ViewModel;

namespace BLL.Stub
{
    public class BLLAdminStub : IBLLAdmin
    {
        //Vi simulrerer DBStub her, på grunn av at DBAdmin fungerer på en annen måte enn de andre databaseklassene (grunnet kryptering).
        private List<Admin> adminer = new List<Admin>();

        public bool EndrePassord(AdminPassordViewModel adminViewModel)
        {
            throw new NotImplementedException();
        }

        public List<Admin> Hent()
        {
            var adminer = new List<Admin>()
            {
                new Admin()
                {
                    Brukernavn = "",
                    Passord = ""
                },
                new Admin()
                {
                    Brukernavn = "",
                    Passord = ""
                }
            };
            return adminer;
        }

        public bool IsPassordGyldig(string Brukernavn, string PwAttempt)
        {
            var admin = adminer.FirstOrDefault(a => a.Brukernavn == Brukernavn);
            if(admin != null)
            {
                return admin.Passord == PwAttempt;
            }
            return false;
        }

        public bool LeggInn(Admin admin)
        {
            var eksisterer = adminer.Where(a => a.Brukernavn == admin.Brukernavn).Any();
            if (eksisterer || admin.Passord == "")
            {
                return false;
            }

            adminer.Add(admin);
            return true;

        }

        public bool SlettAdmin(string Username)
        {
            throw new NotImplementedException();
        }
    }
}
