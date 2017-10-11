using Flybilletter.DAL.DBModel;
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