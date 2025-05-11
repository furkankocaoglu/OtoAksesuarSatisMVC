using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OtoAksesuarSatisWebAp.Models
{
    public class XMLUrun
    {
        [Key]
        public int XmlUrunID { get; set; }

        [Required]
        [StringLength(100)]
        public string UrunAdi { get; set; }

        [Required]
        [StringLength(100)]
        public string Kategori { get; set; }

        [Required]
        [StringLength(100)]
        public string Marka { get; set; }

        public decimal BronzFiyat { get; set; }
        public decimal SilverFiyat { get; set; }
        public decimal GoldFiyat { get; set; }

        public int Stok { get; set; }

        public string Aciklama { get; set; }
        public string Resim { get; set; }

        public DateTime EklenmeZamani { get; set; }
        public DateTime? GuncellenmeZamani { get; set; }

        public ICollection<Yorum> Yorumlar { get; set; }
    }
}