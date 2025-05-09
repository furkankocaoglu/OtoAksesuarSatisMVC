using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace OtoAksesuarSatisWebAp.Models
{
    public partial class OtoAksesuarSatisDB : DbContext
    {
        public OtoAksesuarSatisDB()
            : base("name=OtoAksesuarSatisDB")
        {
        }

        public DbSet<Yonetici> Yoneticiler { get; set; }
        public DbSet<Marka> Markalar { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<Uye> Uyeler { get; set; }
        public DbSet<Favori> Favoriler { get; set; }
        public DbSet<Siparis> Siparisler { get; set; }
        public DbSet<Yorum> Yorumlar { get; set; }
        public DbSet<XMLUrun> XMLUrunler { get; set; }
        public DbSet<Sepet> Sepetler { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
