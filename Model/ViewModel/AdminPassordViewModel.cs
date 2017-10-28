using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flybilletter.Model.ViewModel
{
    public class AdminPassordViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z0-9]{5,}$", ErrorMessage = "Passord må ha hvertfall én stor bokstav, én liten bokstav, ett tall, og minimum 5 tegn. Også kun ASCII-tegn (dvs ekskludert bokstaver som Æ, Ø, og Å).")]
        public string Gammelt { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z0-9]{5,}$", ErrorMessage = "Passord må ha hvertfall én stor bokstav, én liten bokstav, ett tall, og minimum 5 tegn. Også kun ASCII-tegn (dvs ekskludert bokstaver som Æ, Ø, og Å).")]
        public string Nytt { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z0-9]{5,}$", ErrorMessage = "Passord må ha hvertfall én stor bokstav, én liten bokstav, ett tall, og minimum 5 tegn. Også kun ASCII-tegn (dvs ekskludert bokstaver som Æ, Ø, og Å).")]
        public string NyttBekreft { get; set; }
    }
}
