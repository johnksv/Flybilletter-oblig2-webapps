using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IBLLFlyplass
    {
        bool LeggInn(Flyplass flyplass);
        bool Endre(Flyplass item);
        List<Flyplass> HentAlle();
    }
}
