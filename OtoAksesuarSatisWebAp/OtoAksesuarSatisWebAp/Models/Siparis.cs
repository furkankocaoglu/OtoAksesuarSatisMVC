using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OtoAksesuarSatisWebAp.Models
{
    public class Siparis
    {
        [Key]
        public int SiparisID { get; set; }
        
        public int UyeID { get; set; } 
        [ForeignKey("UyeID")]  
        public virtual Uye Uye { get; set; }

        public int UrunID { get; set; }  
        [ForeignKey("UrunID")] 
        public virtual Urun Urun { get; set; }

        [Display(Name = "Miktar")]
        public int Miktar { get; set; }

        [Display(Name = "Toplam Fiyat")]
        public decimal ToplamFiyat { get; set; }

        [Display(Name = "Sipariş Tarihi")]
        public DateTime SiparisTarihi { get; set; } = DateTime.Now;

        public bool Silinmis { get; set; } = false;
    }
}