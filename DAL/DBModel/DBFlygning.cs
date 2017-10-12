﻿using AutoMapper;
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

        public static List<Flygning> HentFlygningerFra(Flyplass flyplass)
        {
            List<Flygning> flygninger = null;
            using (var db = new DB())
            {
                flygninger = db.Flygninger.Include("Fly").Where(flygning => flygning.Rute.Fra.ID == flyplass.ID).Select(model =>
                     new Flygning()
                     {
                         AnkomstTid = model.AnkomstTid,
                         AvgangsTid = model.AvgangsTid
                         
                     }
                ).ToList();
            }

            return flygninger;
        }

        public static List<Flygning> HentFlygningerTil(Flyplass flyplass)
        {
            List<Flygning> flygninger = null;
            using (var db = new DB())
            {
                flygninger = db.Flygninger.Include("Fly").Where(flygning => flygning.Rute.Til.ID == flyplass.ID).Select(model =>
                     new Flygning()
                     {
                         AnkomstTid = model.AnkomstTid,
                         AvgangsTid = model.AvgangsTid

                     }
                ).ToList();
            }

            return flygninger;
        }

        public static Flygning Finn(int ID)
        {
            using(var db = new DB())
            {
                var dbFlygning = db.Flygninger.Find(ID);
                return Mapper.Map<Flygning>(dbFlygning);
            }
        }
    }
}