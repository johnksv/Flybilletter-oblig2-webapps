using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flybilletter.Models
{
    public class Fly
    {
        public int ID { get; set; }
        public string Modell { get; set; } //Modell-navn til flyet
        public int AntallSeter { get; set; }
        public virtual List<Flygning> Flygninger { get; set; }
    }
}