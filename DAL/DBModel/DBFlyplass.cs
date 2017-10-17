using AutoMapper;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flybilletter.DAL.DBModel
{
    public class DBFlyplass
    {
        public string ID { get; set; }
        public string Navn { get; set; }
        public string By { get; set; }
        public string Land { get; set; }
        public virtual List<DBRute> Ruter { get; set; }

        public static List<Flyplass> HentAlle()
        {
            List<Flyplass> flyplasser = null;
            using (var db = new DB())
            {
                 flyplasser = Mapper.Map<List<Flyplass>>(db.Flyplasser.ToList());
            }
            return flyplasser;
        }

        public static Flyplass Hent(string tilFlyplassID)
        {
            Flyplass resultat = null;
            using (var db = new DB())
            {
                resultat = Mapper.Map<Flyplass>(db.Flyplasser.Find(tilFlyplassID));
            }
            return resultat;
        }
    }
}