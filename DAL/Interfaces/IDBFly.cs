using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flybilletter.DAL.Interfaces
{
    public interface IDBFly
    {
        List<Fly> HentAlle();
        bool Endre(Fly fly);
        Fly Hent(int ID);
        bool Slett(int ID);
        bool LeggInn(Fly fly);
    }
}
