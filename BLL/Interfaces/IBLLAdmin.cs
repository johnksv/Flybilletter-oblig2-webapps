using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.Model.ViewModel;

namespace BLL
{
    public interface IBLLAdmin
    {
        bool LeggInn(Admin admin);
        List<Admin> Hent();
        bool SlettAdmin(string brukernavn);
        bool IsPassordGyldig(string brukernavn, string passordforsok);
        bool EndrePassord(AdminPassordViewModel adminViewModel);
    }
}
