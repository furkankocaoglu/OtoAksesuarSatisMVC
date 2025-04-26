using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace OtoSatisWebApp.Models
{
    public partial class OtoSatis_DB : DbContext
    {
        public OtoSatis_DB()
            : base("name=OtoSatis_DB")
        {
            Database.SetInitializer<OtoSatis_DB>(null);
        }

        public DbSet<AltBayii> AltBayilerr { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
