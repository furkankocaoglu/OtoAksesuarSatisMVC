using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace OtoSatisWebApp.Models
{
    public partial class OtoSatisDB : DbContext
    {
        public OtoSatisDB()
            : base("name=OtoSatisDB")
        {
        }

        public DbSet<AltBayi> AltBayiler { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
