using Flybilletter.DAL.DBModel;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLFlyplass : IBLLFlyplass
    {
        private DBFlyplass dbFlyplass= new DBFlyplass();

        public List<Flyplass> HentAlle()
        {
            return dbFlyplass.HentAlle();
        }
    }
}
