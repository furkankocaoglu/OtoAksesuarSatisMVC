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
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Marka> Markalar { get; set; }
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<Uye> Uyeler { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
