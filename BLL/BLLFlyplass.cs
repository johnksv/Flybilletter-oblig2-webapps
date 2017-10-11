using Flybilletter.DAL.DBModel;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLFlyplass
    {

        public static List<Flyplass> HentAlle()
        {
            return DBFlyplass.HentAlle();
        }
    }
}
