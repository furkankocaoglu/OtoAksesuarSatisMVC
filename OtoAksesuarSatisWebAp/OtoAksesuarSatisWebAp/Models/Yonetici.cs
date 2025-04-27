using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OtoAksesuarSatisWebAp.Models
{
    public class Yonetici
    {
        [Key]
        public int YoneticiID { get; set; }

        [Display(Name = "İsim")]
        [Required(ErrorMessage = "İsim alanı boş bırakılamaz.")]
        public string YoneticiIsim { get; set; }

        [Display(Name = "E-Posta")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Bu alan 5 - 200 karakter arasında olabilir")]
        public string Eposta { get; set; }

        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Bu alan 5 - 30 karakter arasında olabilir")]
        public string Sifre { get; set; }

        public bool AktifMi { get; set; }
    }
}