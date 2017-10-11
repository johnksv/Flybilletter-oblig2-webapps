using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Flybilletter.Models
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

        public virtual DbSet<Bestilling> Bestillinger { get; set; }
        public virtual DbSet<Fly> Fly { get; set; }
        public virtual DbSet<Flygning> Flygninger { get; set; }
        public virtual DbSet<Flyplass> Flyplasser { get; set; }
        public virtual DbSet<DBKunde> Kunder { get; set; }
        public virtual DbSet<DBPoststed> Poststeder { get; set; }
        public virtual DbSet<Rute> Ruter { get; set; }
    }
}