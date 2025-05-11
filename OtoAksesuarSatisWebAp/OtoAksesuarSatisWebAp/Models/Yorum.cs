using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OtoAksesuarSatisWebAp.Models
{
    public class Yorum
    {
        [Key]
        public int YorumID { get; set; }

        public int UyeID { get; set; }  
        [ForeignKey("UyeID")]
        public virtual Uye Uye { get; set; }  

        public int? UrunID { get; set; }  
        [ForeignKey("UrunID")]
        public virtual Urun Urun { get; set; }
        public int? XmlUrunID { get; set; }  
        [ForeignKey("XmlUrunID")]
        public virtual XMLUrun XMLUrun { get; set; }  

        [Required(ErrorMessage = "Yorum alanı boş olamaz.")]
        [StringLength(500, ErrorMessage = "Yorum 500 karakteri geçemez.")]
        [Display(Name = "Yorum")]
        public string YorumMetni { get; set; }  

        [Display(Name = "Yorum Tarihi")]
        public DateTime YorumTarihi { get; set; } = DateTime.Now;

        [Display(Name = "Durum")]
        public bool Durum { get; set; } = true; 

        public bool Silinmis { get; set; } = false;  
    }
}