using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IBLLAdmin
    {
        bool IsPassordGyldig(string Username, string PwAttempt);
        bool LeggInn(Admin admin);
    }
}
