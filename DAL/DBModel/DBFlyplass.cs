using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flybilletter.DAL.DBModel
{
    public class DBFlyplass
    {
        public string ID { get; set; }
        public string Navn { get; set; }
        public string By { get; set; }
        public string Land { get; set; }
        public virtual List<DBRute> Ruter { get; set; }

    }
}