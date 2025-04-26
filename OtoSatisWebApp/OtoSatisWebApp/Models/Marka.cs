using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OtoSatisWebApp.Models
{
    public class Marka
    {
        public int MarkaID { get; set; }

        [Display(Name = "İsim")]
        [Required(ErrorMessage = "Bu alan zorunludur")]
        [StringLength(maximumLength: 75, ErrorMessage = "bu alan en fazla 75 karakter olmalıdır")]
        public string MarkaAdi{ get; set;}

        [Display(Name = "Durum")]
        public bool AktifMi { get; set; }

        public bool Silinmis { get; set; }

        public ICollection<Urun> Urunler { get; set; }

    }
}