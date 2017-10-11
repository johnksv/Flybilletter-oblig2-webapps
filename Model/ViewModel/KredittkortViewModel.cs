using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flybilletter.Models
{
    public class KredittkortViewModel
    {

        [Required]
        [RegularExpression(@"^[A-Za-zæøåÆØÅöÖäÄëË\- ]+$", ErrorMessage = "Feltet inneholder ugyldige bokstaver.")]
        public string Kortholder { get; set; }

        [Required]
        [RegularExpression("^[1-9][0-9]{15}$", ErrorMessage = "Ugyldig kortnummer. Må være 16 tall, og kortnummer kan ikke starte på 0.")]
        public long Kortnummer { get; set; }

        [Required]
        [RegularExpression("^[0-9]{3}$", ErrorMessage = "Ugyldig cvc. Må være 3 tall.")]
        public int CVC{ get; set; }

        [Required]
        [RegularExpression("^[0-9]{2}-[0-9]{2}$", ErrorMessage = "Ugyldig utløpsdato. Må være av format MM-ÅÅ.")]
        public string Utlop { get; set; }
    }
}