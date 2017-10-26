using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flybilletter.Model.ViewModel
{
    public class LagFlygningViewModel
    {
        [Required]
        public String RuteID { get; set; }

        [Required]
        public String FlyID { get; set; }

        [Required]
        public DateTime AvgangsTid { get; set; }
    }
}
