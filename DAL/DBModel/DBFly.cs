using AutoMapper;
using Flybilletter.DAL.Interfaces;
using System.Collections.Generic;
using Flybilletter.Model.DomeneModel;
using System;
using DAL;

namespace Flybilletter.DAL.DBModel
{
    public class DBFly : IDBFly
    {
        public int ID { get; set; }
        public string Modell { get; set; } //Modell-navn til flyet
        public int AntallSeter { get; set; }
        public virtual List<DBFlygning> Flygninger { get; set; }

        public Fly Hent(int iD)
        {
            using (var db = new DB())
            {
                try
                {
                    return Mapper.Map<Fly>(db.Fly.Find(iD));
                } catch (Exception e)
                {
                    DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                    return null;
                }
            }
        }

        public List<Fly> HentAlle()
        {
            using (var db = new DB())
            {
                try
                {
                    return Mapper.Map<List<Fly>>(db.Fly);
                } catch (Exception e)
                {
                    DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                    return null;
                }
            }
        }

        public bool Oppdater(Fly fly)
        {
            using (var db = new DB())
            {
                try
                {
                    // DB oppdater
                    var dbflyentitet = db.Fly.Find(fly.ID);
                    dbflyentitet.Modell = fly.Modell;
                    dbflyentitet.AntallSeter = fly.AntallSeter;
                    db.SaveChanges();
                    return true;
                } catch (Exception e)
                {
                    DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                    return false;
                }
            }
        }
    }
}