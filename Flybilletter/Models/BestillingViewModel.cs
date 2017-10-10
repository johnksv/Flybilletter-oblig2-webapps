using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flybilletter.Models
{
    public class BestillingViewModel
    {

        public Reise Tur { get; set; }

        public Reise Retur { get; set; }

        public KredittkortViewModel Kredittkort { get; set; }

        public List<Kunde> Kunder { get; set; }

        public double Totalpris
        {
            get
            {
                double pris = 0;
                if (Tur != null) pris += Tur.Pris;
                if (Retur != null) pris += Retur.Pris;
                return pris;
            }

            private set { }
        }
    }
}