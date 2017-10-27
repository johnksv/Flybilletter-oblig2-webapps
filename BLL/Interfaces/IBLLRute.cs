using Flybilletter.Model.DomeneModel;
using Flybilletter.Model.ViewModel;
using System.Collections.Generic;

namespace BLL
{
    public interface IBLLRute
    {
        List<Rute> HentAlle();
        bool Slett(int id);
        Rute Hent(int ruteID);
        bool LagreRute(Rute rute);
        bool LagRute(NyRuteViewModel rute);
    }
}
