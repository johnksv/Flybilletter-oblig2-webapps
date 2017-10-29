using Flybilletter.Model.DomeneModel;
using Flybilletter.Model.ViewModel;
using System.Collections.Generic;

namespace BLL
{
    public interface IBLLRute
    {
        bool LeggInn(NyRuteViewModel rute);
        bool Endre(Rute rute);
        List<Rute> HentAlle();
        bool Slett(int id);
    }
}
