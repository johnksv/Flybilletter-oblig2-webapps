using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.Model.DomeneModel;

namespace Flybilletter.DAL.Interfaces
{
    public interface IDBRute
    {
        List<Rute> HentAlle();
        bool Slett(int id);
        Rute Hent(int ruteID);
        bool LagreRute(Rute rute);
    }
}
