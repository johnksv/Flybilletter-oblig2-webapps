using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flybilletter.Model.DomeneModel
{
    public class Bestilling
    { 
        public string Referanse { get; set; } //ID til bestillingen
        public virtual List<Kunde> Passasjerer { get; set; } //Passasjerer knyttet til en bestilling //Gjort om fra DBKunde til Kunde
        public virtual List<Flygning> FlygningerTur { get; set; }
        public virtual List<Flygning> FlygningerRetur { get; set; }
        public DateTime Bestillingstidspunkt { get; set; }
        public double Totalpris { get; set; }

    }
}