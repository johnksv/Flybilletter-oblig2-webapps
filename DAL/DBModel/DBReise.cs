using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flybilletter.DAL.DBModel
{
    public class DBReise
    {
        public IEnumerable<DBFlygning> Flygninger { get; set; }
        public int Mellomlanding { get; set; }
        public DBFlyplass Fra { get; set; }
        public DBFlyplass Til { get; set; }
        public double Pris { get; set; }
        public DateTime Avgang { get; set; }
        public DateTime Ankomst { get; set; }

        public DBReise(DBFlygning utenMellomLanding)
        {
            Flygninger = new List<DBFlygning>() {
                utenMellomLanding
            };
            this.Fra = utenMellomLanding.Rute.Fra;
            this.Til = utenMellomLanding.Rute.Til;
            this.Pris = utenMellomLanding.Rute.BasePris;
            this.Avgang = utenMellomLanding.AvgangsTid;
            this.Ankomst = utenMellomLanding.AnkomstTid;
        }

        public DBReise(DBFlygning flygning1, DBFlygning flygning2)
        {
            Flygninger = new List<DBFlygning>
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