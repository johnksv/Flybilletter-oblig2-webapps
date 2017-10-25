using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flybilletter.Model.DomeneModel
{
    public class Rute
    {
        public int ID { get; set; }
        public Flyplass Fra { get; set; }
        public Flyplass Til { get; set; }
        public double BasePris { get; set; } //faktor for å regne ut total pris for turen

    }
}