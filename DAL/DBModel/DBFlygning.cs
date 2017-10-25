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
    public class DBFlygning : IDBFlygning
    {
        public int ID { get; set; }
        public virtual List<DBBestilling> Bestillinger { get; set; }
        public virtual DBRute Rute { get; set; }
        public virtual DBFly Fly { get; set; }
        public DateTime AvgangsTid { get; set; }
        public DateTime AnkomstTid { get; set; }

        public List<Flygning> HentFlygningerFra(Flyplass flyplass)
        {
            using (var db = new DB())
            {
                try
                {
                    return Mapper.Map<List<Flygning>>(db.Flygninger.Include("Fly").Where(flygning => flygning.Rute.Fra.ID == flyplass.ID).ToList());
                }catch(Exception e)
                {
                    DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                    return null;
                }
            }
        }

        public List<Flygning> HentFlygningerTil(Flyplass flyplass)
        {
            using (var db = new DB())
            {
                try
                {
                    return Mapper.Map<List<Flygning>>(db.Flygninger.Include("Fly").Where(flygning => flygning.Rute.Til.ID == flyplass.ID).ToList());
                }catch(Exception e)
                {
                    DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                    return null;
                }
            }
        }

        public Flygning Finn(int ID)
        {
            using(var db = new DB())
            {
                try
                {
                    return Mapper.Map<Flygning>(db.Flygninger.AsNoTracking().FirstOrDefault(fly => fly.ID == ID));
                }catch(Exception e)
                {
                    DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                    return null;
                }
            }
        }

        public List<Flygning> HentAlle(DateTime dateTime)
        {
            using(var db = new DB())
            {

                var dbflygninger = db.Flygninger.Include("Fly").Where(flyg => flyg.AvgangsTid > dateTime).ToList();
                List<Flygning> flygninger = new List<Flygning>();
                foreach(var flygning in dbflygninger)
                {
                    flygninger.Add(Mapper.Map<Flygning>(flygning));
                }
                return flygninger;
            }
        }

        public Flygning HentEnFlygning(int id)
        {
            using(var db = new DB())
            {
                var dbflygning = db.Flygninger.Include("Fly").Where(item => item.ID == id).FirstOrDefault();
                return Mapper.Map<Flygning>(dbflygning);
            }
        }

        public bool OppdaterFlygning(Flygning flygning)
        {
            using(var db = new DB())
            {
                var dbflygning = db.Flygninger.Find(flygning.ID);
                dbflygning.AnkomstTid = flygning.AnkomstTid;
                dbflygning.AvgangsTid = flygning.AvgangsTid;

                try
                {
                    db.SaveChanges();
                }catch(Exception e)
                {
                    DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                    return false;
                }
                return true;
            }
        }
    }
}