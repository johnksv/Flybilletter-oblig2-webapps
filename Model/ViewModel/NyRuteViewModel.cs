using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flybilletter.Model.ViewModel
{
    public class NyRuteViewModel
    {

        [Required]
        [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "ID må være tre store bokstaver mellom A og Z")]
        public string FraFlyplassID { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "ID må være tre store bokstaver mellom A og Z")]
        public string TilFlyplassID { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = false)]
        public double Basepris { get; set; } //faktor for å regne ut total pris for turen
        [Required]
        [DataType(DataType.Time)]
        public TimeSpan Reisetid { get; set; }

    }
}
