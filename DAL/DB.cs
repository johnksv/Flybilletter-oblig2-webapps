﻿using AutoMapper;
using Flybilletter.DAL.DBModel;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Flybilletter.DAL
{
    internal class DB : DbContext
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
                cfg.CreateMap<DBPostnummer, Model.DomeneModel.Postnummer>();

                cfg.CreateMap<Model.DomeneModel.Bestilling, DBBestilling>();
                cfg.CreateMap<Model.DomeneModel.Fly, DBFly>();
                cfg.CreateMap<Model.DomeneModel.Flygning, DBFlygning>();
                cfg.CreateMap<Model.DomeneModel.Flyplass, DBFlyplass>();
                cfg.CreateMap<Model.DomeneModel.Rute, DBRute>();
                cfg.CreateMap<Model.DomeneModel.Kunde, DBKunde>();
                cfg.CreateMap<Model.DomeneModel.Postnummer, DBPostnummer>();
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
        public virtual DbSet<DBPostnummer> Poststeder { get; set; }
        public virtual DbSet<DBRute> Ruter { get; set; }
        public virtual DbSet<DBAdmin> Administratorer { get; set; }
    }
}