using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flybilletter.Models
{
    public class Kunde
    {
        public Kunde()
        {
            Fodselsdag = new DateTime(1970, 1, 1);
        }

        public int ID { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-zæøåÆØÅ\- ]+$", ErrorMessage = "Fornavn kan kun være bokstaver")]
        public string Fornavn { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-zæøåÆØÅ\- ]+$", ErrorMessage = "Etternavn kan kun være bokstaver")]
        public string Etternavn { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fodselsdag { get; set; }

        [Required]
        [RegularExpression("^[A-Za-z0-9æøåÆØÅ,. ]+$", ErrorMessage = "Adresse kan kun være bokstaver og tall")]
        public string Adresse { get; set; }

        [Required]
        [RegularExpression(@"^(?:\+[0-9]{10}|[0-9]{8})$", ErrorMessage = "Telefonnummer må være 8 siffer")]
        public string Tlf { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [RegularExpression(@"^[A-Za-zæøåÆØÅ0-9_\-,\. ]+@[a-zA-Z0-9]+\.[a-zA-Z]+$", ErrorMessage = "Ugyldig e-post")]
        public string EPost { get; set; }

        [Required]
        [RegularExpression("^[0-9]{4}$", ErrorMessage = "Postnummer må være 4 siffer")]
        public string Postnummer { get; set; }

        public string Poststed { get; set; }

        public List<Bestilling> Bestillinger { get; set; }
    }
}