using Flybilletter.DAL.DBModel;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flybilletter.DAL.Interfaces
{
    public interface IDBKunde
    {
        bool LeggInn(IEnumerable<Kunde> kunder);
        DBKunde LeggInn(Kunde innKunde);
        List<Kunde> HentAlle();
        Kunde HentEnKunde(int id);
        bool Oppdater(Kunde kunde);
    }
}
