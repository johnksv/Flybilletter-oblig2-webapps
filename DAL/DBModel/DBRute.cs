using Flybilletter.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flybilletter.DAL.DBModel
{
    public class DBRute : IDBRute
    {
        public int ID { get; set; }
        public virtual DBFlyplass Fra { get; set; }
        public virtual DBFlyplass Til { get; set; }
        public double BasePris { get; set; } //faktor for å regne ut total pris for turen
        public virtual List<DBFlygning> Flygninger { get; set; }


    }
}