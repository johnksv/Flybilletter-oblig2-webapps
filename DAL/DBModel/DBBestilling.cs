using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flybilletter.DAL.DBModel
{
    public class DBBestilling
    { 
        public int ID { get; set; }
        public string Referanse { get; set; } //ID til bestillingen
        public virtual List<DBKunde> Passasjerer { get; set; } //Passasjerer knyttet til en bestilling
        public virtual List<DBFlygning> FlygningerTur { get; set; }
        public virtual List<DBFlygning> FlygningerRetur { get; set; }
        public DateTime BestillingsTidspunkt { get; set; }
        public double Totalpris { get; set; }

    }
}