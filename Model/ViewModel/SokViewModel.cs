using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flybilletter.Models
{
    public class SokViewModel
    {

        [Required]
        [RegularExpression("^[A-Z]{3}$", ErrorMessage ="Ugyldig flyplass-ID")]
        public String Fra { get; set; }

        [Required]
        [RegularExpression("^[A-Z]{3}$", ErrorMessage = "Ugyldig flyplass-ID")]
        public String Til { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Avreise { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Retur { get; set; }

        [Range (1, 100)]
        [Display(Name = "Antall Billetter")]
        public int AntallBilletter { get; set; }




    }
}