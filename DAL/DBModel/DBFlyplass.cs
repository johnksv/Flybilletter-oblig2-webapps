using AutoMapper;
using DAL;
using Flybilletter.DAL.Interfaces;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flybilletter.DAL.DBModel
{
    public class DBFlyplass : IDBFlyplass
    {
        [Required]
        public string ID { get; set; }
        [Required]
        public string Navn { get; set; }
        [Required]
        public string By { get; set; }
        [Required]
        public string Land { get; set; }
        public virtual List<DBRute> Ruter { get; set; }

        public List<Flyplass> HentAlle()
        {
            using (var db = new DB())
            {
                try
                {
                    return Mapper.Map<List<Flyplass>>(db.Flyplasser.ToList());
                }
                catch (Exception e)
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
                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                    return null;
                }
            }
        }

        public bool LeggInn(Flyplass flyplass)
        {
            using (var db = new DB())
            {
                try
                {
                    var dbflyplass = Mapper.Map<DBFlyplass>(flyplass);
                    if (! db.Flyplasser.Contains(dbflyplass))
                    {
                        db.Flyplasser.Add(dbflyplass);
                        db.SaveChanges();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                    
                }
            }
            return false;
        }
    }
}