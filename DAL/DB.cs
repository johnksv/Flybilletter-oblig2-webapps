using AutoMapper;
using Flybilletter.DAL.DBModel;
using Flybilletter.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Flybilletter.DAL
{
    public class DB : DbContext
    {

        public DB() : base("name=Flybilletter")
        {
            Database.CreateIfNotExists();
            Database.SetInitializer(new DBInit());

            
            Mapper.Initialize(cfg => {
                cfg.CreateMap<DBBestilling, Model.DomeneModel.Bestilling>();
                cfg.CreateMap<DBFly, Model.DomeneModel.Fly>();
                cfg.CreateMap<DBFlygning, Model.DomeneModel.Flygning>();
                cfg.CreateMap<DBFlyplass, Model.DomeneModel.Flyplass>();
                cfg.CreateMap<DBRute, Model.DomeneModel.Rute>();
                cfg.CreateMap<DBKunde, Model.DomeneModel.Kunde>();
                cfg.CreateMap<DBPoststed, Model.DomeneModel.Postnummer>();
                cfg.CreateMap<DBReise, Model.DomeneModel.Reise>();

                // Skjønner ikke hvorfor eller om vi i det hele tatt trenger å mappe begge veier (?)
                cfg.CreateMap<Model.DomeneModel.Bestilling, DBBestilling>();
                cfg.CreateMap<Model.DomeneModel.Fly, DBFly>();
                cfg.CreateMap<Model.DomeneModel.Flygning, DBFlygning>();
                cfg.CreateMap<Model.DomeneModel.Flyplass, DBFlyplass>();
                cfg.CreateMap<Model.DomeneModel.Rute, DBRute>();
                cfg.CreateMap<Model.DomeneModel.Kunde, DBKunde>();
                cfg.CreateMap<Model.DomeneModel.Postnummer, DBPoststed>();
                cfg.CreateMap<Model.DomeneModel.Reise, DBReise>();
            });
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public virtual DbSet<DBBestilling> Bestillinger { get; set; }
        public virtual DbSet<DBFly> Fly { get; set; }
        public virtual DbSet<DBFlygning> Flygninger { get; set; }
        public virtual DbSet<DBFlyplass> Flyplasser { get; set; }
        public virtual DbSet<DBKunde> Kunder { get; set; }
        public virtual DbSet<DBPoststed> Poststeder { get; set; }
        public virtual DbSet<DBRute> Ruter { get; set; }
    }
}