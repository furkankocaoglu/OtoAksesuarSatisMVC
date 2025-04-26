using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Drawing.Drawing2D;

namespace OtoSatisWebApp.Models
{
    public class Urun
    {
        public int KategoriID { get; set; }

        [ForeignKey("KategoriID")]
        public virtual Kategori Kategori { get; set; }

        public int MarkaID { get; set; }

        [ForeignKey("MarkaID")]
        public virtual Marka Marka { get; set; }

        [Key]
        public int UrunID { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Ürün adı en fazla 100 karakter olabilir.")]
        [Display(Name = "Ürün Adı")]
        public string UrunAdi { get; set; }

        [Display(Name = "Stok Miktarı")]
        public int StokMiktari { get; set; }

        [Display(Name = "Açıklama")]
        public string Aciklama { get; set; }

        [Display(Name = "Resim Yolu")]
        public string ResimYolu { get; set; }

        [Display(Name = "Bronz Fiyat")]
        public decimal BronzFiyat { get; set; }

        [Display(Name = "Silver Fiyat")]
        public decimal SilverFiyat { get; set; }

        [Display(Name = "Gold Fiyat")]
        public decimal GoldFiyat { get; set; }

        [Display(Name = "Eklenme Tarihi")]
        public DateTime EklenmeTarihi { get; set; } = DateTime.Now;

        public bool AktifMi { get; set; } = true;

        public bool Silinmis { get; set; } = false;
    }
}