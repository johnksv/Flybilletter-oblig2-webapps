using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flybilletter.DAL.DBModel
{
    public class DBFlygning
    {
        public int ID { get; set; }
        public virtual List<DBBestilling> Bestillinger { get; set; }
        public virtual DBRute Rute { get; set; }
        public virtual DBFly Fly { get; set; }
        public DateTime AvgangsTid { get; set; }
        public DateTime AnkomstTid { get; set; }

        public static List<Flygning> HentFlygninger(string fraFlyplassID)
        {
            List<Flygning> flygninger = null;
            using (var db = new DB())
            {
                flygninger = db.Flygninger.Include("Fly").Where(flygning => flygning.Rute.Fra.ID.Equals(fraFlyplassID)).Select(model =>
                     new Flygning()
                     {
                         AnkomstTid = model.AnkomstTid,
                         AvgangsTid = model.AvgangsTid
                         
                     }
                ).ToList();
            }

            return flygninger;
        }


    }
}