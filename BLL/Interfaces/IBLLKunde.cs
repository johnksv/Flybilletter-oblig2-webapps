using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IBLLKunde
    {
        bool LeggInn(Kunde innKunde);
        bool Endre(Kunde kunde);
        List<Kunde> HentAlle();
        string HentPoststed(string postnummer);
        bool Slett(int id);
    }
}
