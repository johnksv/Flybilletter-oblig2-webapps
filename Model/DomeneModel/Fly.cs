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
        [RegularExpression(@"^[A-Za-z0-9 ]+$", ErrorMessage = "Modell kan kun være bokstaver mellom a og z, tall og mellomrom")]
        public string Modell { get; set; } //Modell-navn til flyet
        [Required]
        [RegularExpression(@"^([0-9]+)$", ErrorMessage = "Antall seter kan kun være et positivt heltall")]
        public int AntallSeter { get; set; }
    }
}