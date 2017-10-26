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
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime AvgangsTid { get; set; }

        public DateTime AnkomstTid
        {
            get
            {
                return AvgangsTid + Rute.Reisetid;
            }
        }

    }
}