using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IBLLRute
    {
        List<Rute> HentAlle();
        bool Slett(int id);
    }
}
