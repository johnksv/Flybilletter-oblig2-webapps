using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flybilletter.Models
{
    public class Flygning
    {
        public int ID { get; set; }
        public virtual List<Bestilling> Bestillinger { get; set; }
        public virtual Rute Rute { get; set; }
        public virtual Fly Fly { get; set; }
        public DateTime AvgangsTid { get; set; }
        public DateTime AnkomstTid { get; set; }
        
    }
}