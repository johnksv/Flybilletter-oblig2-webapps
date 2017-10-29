using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IBLLFly
    {
        bool LeggTil(Fly fly);
        bool Endre(Fly fly);
        List<Fly> HentAlle();
        bool Slett(int ID);
    }
}
