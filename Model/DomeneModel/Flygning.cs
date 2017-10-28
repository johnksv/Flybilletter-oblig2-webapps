using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flybilletter.Model.DomeneModel
{
    public class Flygning
    {
        public int ID { get; set; }
        public Rute Rute { get; set; }
        public Fly Fly { get; set; }
        public bool Kansellert { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AvgangsTid { get; set; }

        public DateTime AnkomstTid
        {
            get
            {
                if (Rute == null) return AvgangsTid;
                return AvgangsTid + Rute.Reisetid;
            }
        }

    }
}