using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flybilletter.DAL.Interfaces
{
    public interface IDBFlyplass
    {
        List<Flyplass> HentAlle();
        Flyplass Hent(string tilFlyplassID);

    }
}
