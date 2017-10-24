using AutoMapper;
using DAL;
using Flybilletter.DAL.Interfaces;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flybilletter.DAL.DBModel
{
    public class DBFlyplass : IDBFlyplass
    {
        public string ID { get; set; }
        public string Navn { get; set; }
        public string By { get; set; }
        public string Land { get; set; }
        public virtual List<DBRute> Ruter { get; set; }

        public List<Flyplass> HentAlle()
        {
            using (var db = new DB())
            {
                try
                {
                    return Mapper.Map<List<Flyplass>>(db.Flyplasser.ToList());
                }catch(Exception e)
                {
                    DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                    return null;
                }
            }
        }

        public Flyplass Hent(string tilFlyplassID)
        {
            using (var db = new DB())
            {
                try
                {
                    return Mapper.Map<Flyplass>(db.Flyplasser.Find(tilFlyplassID));
                }catch(Exception e)
                {
                    DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                    return null;
                }
            }
        }
    }
}