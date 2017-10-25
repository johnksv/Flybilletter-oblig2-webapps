using Flybilletter.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flybilletter.Model.DomeneModel;
using AutoMapper;

namespace Flybilletter.DAL.DBModel
{
    public class DBRute : IDBRute
    {
        public int ID { get; set; }
        public virtual DBFlyplass Fra { get; set; }
        public virtual DBFlyplass Til { get; set; }
        public double BasePris { get; set; } //faktor for å regne ut total pris for turen
        public virtual List<DBFlygning> Flygninger { get; set; }

        public List<Rute> HentAlle()
        {
            using(var db = new DB())
            {
                var dbruter = db.Ruter.Include("Fra").Include("Til").ToList();
                var ruter = new List<Rute>();
                foreach(var dbrute in dbruter)
                {
                    ruter.Add(Mapper.Map<Rute>(dbrute));
                }
                return ruter;
            }
        }
    }
}