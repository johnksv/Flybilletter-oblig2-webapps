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
        List<Fly> HentAlle();
        bool Oppdater(Fly fly);
        Fly Hent(int ID);
        bool Slett(int ID);
        bool LeggTil(Fly fly);
    }
}
