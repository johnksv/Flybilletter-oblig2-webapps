using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flybilletter.Models
{
    public class Reise
    {
        public IEnumerable<Flygning> Flygninger { get; set; }
        public int Mellomlanding { get; set; }
        public Flyplass Fra { get; set; }
        public Flyplass Til { get; set; }
        public double Pris { get; set; }
        public DateTime Avgang { get; set; }
        public DateTime Ankomst { get; set; }

        public Reise(Flygning utenMellomLanding)
        {
            Flygninger = new List<Flygning>() {
                utenMellomLanding
            };
            this.Fra = utenMellomLanding.Rute.Fra;
            this.Til = utenMellomLanding.Rute.Til;
            this.Pris = utenMellomLanding.Rute.BasePris;
            this.Avgang = utenMellomLanding.AvgangsTid;
            this.Ankomst = utenMellomLanding.AnkomstTid;
        }

        public Reise(Flygning flygning1, Flygning flygning2)
        {
            Flygninger = new List<Flygning>
            {
                flygning1,
                flygning2
            };

            this.Fra = flygning1.Rute.Fra;
            this.Til = flygning2.Rute.Til;
            this.Pris = flygning1.Rute.BasePris + flygning2.Rute.BasePris;
            this.Avgang = flygning1.AvgangsTid;
            this.Ankomst = flygning2.AnkomstTid;
            this.Mellomlanding = 1;
        }

        

        




    }
}