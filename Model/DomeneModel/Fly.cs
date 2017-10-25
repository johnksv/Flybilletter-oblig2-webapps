using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flybilletter.Model.DomeneModel
{
    public class Fly
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string Modell { get; set; } //Modell-navn til flyet
        [Required]
        public int AntallSeter { get; set; }
    }
}