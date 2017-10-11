using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flybilletter.DAL.DBModel
{
    public class DBFlygning
    {
        public int ID { get; set; }
        public virtual List<DBBestilling> Bestillinger { get; set; }
        public virtual DBRute Rute { get; set; }
        public virtual DBFly Fly { get; set; }
        public DateTime AvgangsTid { get; set; }
        public DateTime AnkomstTid { get; set; }
        
    }
}