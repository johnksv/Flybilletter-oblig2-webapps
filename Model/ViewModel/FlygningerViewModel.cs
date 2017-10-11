using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flybilletter.Models
{
    public class FlygningerViewModel
    {
        public List<Reise> TurMuligheter { get; set; }
        public List<Reise> ReturMuligheter { get; set; }
        public bool TurRetur { get; set; }
    }
}