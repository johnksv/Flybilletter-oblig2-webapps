using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flybilletter.Model.DomeneModel
{
    public class Rute
    {
        public int ID { get; set; }
        public Flyplass Fra { get; set; }
        public Flyplass Til { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = false)]
        public double BasePris { get; set; } //faktor for å regne ut total pris for turen
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:H:mm:ss}", ApplyFormatInEditMode = true)]
        public TimeSpan Reisetid { get; set; }

    }
}