using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OtoAksesuarSatisWebAp.Models
{
    public class Kategori
    {
        public int KategoriID { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olabilir.")]
        [Display(Name = "Kategori Adı")]
        public string KategoriAdi { get; set; }

        [Display(Name = "Durum")]
        public bool Durum { get; set; } = true;
        public bool Silinmis { get; set; } = false;
    }
}