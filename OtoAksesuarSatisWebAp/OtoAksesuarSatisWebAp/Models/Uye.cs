using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OtoAksesuarSatisWebAp.Models
{
    public class Uye
    {
        [Key]
        public int UyeID { get; set; }

        [Required(ErrorMessage = "İsim zorunludur.")]
        [StringLength(50)]
        [Display(Name = "İsim")]
        public string Isim { get; set; }

        [Required(ErrorMessage = "Soyisim zorunludur.")]
        [StringLength(50)]
        [Display(Name = "Soyisim")]
        public string Soyisim { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [StringLength(100)]
        [Display(Name = "E-Posta")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Şifre 5-50 karakter arasında olmalıdır.")]
        [Display(Name = "Şifre")]
        public string Sifre { get; set; }

        [Display(Name = "Kayıt Tarihi")]
        public DateTime KayitTarihi { get; set; } = DateTime.Now;

        [Display(Name = "Durum")]
        public bool AktifMi { get; set; } = true;

        public bool Silinmis { get; set; } = false;
    }
}