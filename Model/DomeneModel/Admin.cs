using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flybilletter.Model.DomeneModel
{//Teknisk sett view-modell også?
    public class Admin
    {
        //Testbruker: username="root", password="test"
        [Required(ErrorMessage = "Brukernavn må oppgis.")]
        [RegularExpression(@"^[A-Za-z0-9]{4,}$", ErrorMessage = "Brukernavn må hvertfall være 4 karakterer lang, og kun ASCII-tegn til.")]
        public string Brukernavn { get; set; }
        [Required(ErrorMessage = "Passord må oppgis.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z0-9]{5,}$", ErrorMessage = "Passord må ha hvertfall én stor bokstav, én liten bokstav, ett tall, og minimum 5 tegn. Også kun ASCII-tegn (dvs ekskludert bokstaver som Æ, Ø, og Å).")]
        public string Passord { get; set; }
        // public DateTime lastLogin { get; set; }  //nice to have(?)
    }
}
