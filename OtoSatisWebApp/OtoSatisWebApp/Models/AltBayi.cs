using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OtoSatisWebApp.Models
{
    public class AltBayi
    {
        [Key]
        public int AltBayiID { get; set; }

        [Display(Name = "Bayi Adı")]
        [Required(ErrorMessage = "Bayi adı zorunludur.")]
        [StringLength(100, ErrorMessage = "En fazla 100 karakter olabilir.")]
        public string BayiAdi { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz.")]
        [StringLength(50, ErrorMessage = "En fazla 50 karakter olabilir.")]
        public string KullaniciAdi { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre boş bırakılamaz.")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Şifre 5-30 karakter arası olmalıdır.")]
        public string Sifre { get; set; }

        [Display(Name = "Segment")]
        [Required(ErrorMessage = "Segment seçilmelidir.")]
        [StringLength(10, ErrorMessage = "Geçerli bir segment giriniz.")]
        public string Segment { get; set; } 

        [Display(Name = "Son XML Güncelleme Tarihi")]
        public DateTime SonXmlGuncellemeTarihi { get; set; } = DateTime.Now;

        [Display(Name = "Aktif mi?")]
        public bool IsActive { get; set; } = true;
    }
}