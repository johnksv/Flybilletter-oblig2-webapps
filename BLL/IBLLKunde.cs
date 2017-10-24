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
        string HentPoststed(string postnummer);
        bool LeggInn(IEnumerable<Kunde> kunder);
        bool LeggInn(Kunde innKunde);
        List<Kunde> HentAlle();
        Kunde HentEnKunde(int id);
    }
}
