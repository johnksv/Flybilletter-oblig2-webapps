using Flybilletter.Model.DomeneModel;
using Flybilletter.Model.ViewModel;
using System.Collections.Generic;

namespace BLL
{
    public interface IBLLRute
    {
        List<Rute> HentAlle();
        bool Slett(int id);
        bool LagreRute(Rute rute);
        bool LagRute(NyRuteViewModel rute);
    }
}
