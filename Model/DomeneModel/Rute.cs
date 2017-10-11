using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flybilletter.Models
{
    public class Rute
    {
        public int ID { get; set; }
        public virtual Flyplass Fra { get; set; }
        public virtual Flyplass Til { get; set; }
        public double BasePris { get; set; } //faktor for å regne ut total pris for turen
        public virtual List<Flygning> Flygninger { get; set; }


    }
}