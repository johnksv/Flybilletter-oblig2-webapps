using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flybilletter.Model.DomeneModel
{
    public class Postnummer
    {
        [Required]
        [RegularExpression("^[0-9]{4}$", ErrorMessage = "Postnummer må være 4 siffer")]
        public string Postnr { get; set; }
        public string Poststed { get; set; }
    }
}
