using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flybilletter.Model.DomeneModel
{
    public class Flyplass
    {
        [Required]
        [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "ID må være tre store bokstaver mellom A og Z")]
        public string ID { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-zæøåÆØÅ\- ]+$", ErrorMessage = "Navn kan kun bestå av bokstaver")]
        public string Navn { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-zæøåÆØÅ\- ]+$", ErrorMessage = "By kan kun bestå av bokstaver")]
        public string By { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-zæøåÆØÅ\- ]+$", ErrorMessage = "Land kan kun bestå av bokstaver")]
        public string Land { get; set; }

    }
}