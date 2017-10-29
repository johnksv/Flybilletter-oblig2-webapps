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
        DBKunde LeggInn(Kunde innKunde);
        List<Kunde> HentAlle();
        Kunde Hent(int id);
        bool Endre(Kunde kunde);
        bool Slett(int id);
    }
}
