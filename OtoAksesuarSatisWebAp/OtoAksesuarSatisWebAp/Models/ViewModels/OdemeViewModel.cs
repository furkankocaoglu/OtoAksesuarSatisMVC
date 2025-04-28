using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OtoAksesuarSatisWebAp.Models.ViewModels
{
    public class OdemeViewModel
    {
        [Required(ErrorMessage = "Bu alan zorunlu")]
        public string IsimSoyisim { get; set; }

        [Required(ErrorMessage = "Bu alan zorunlu")]
        public string KartNumarasi { get; set; }

        [Required(ErrorMessage = "Bu alan zorunlu")]
        public string Ay { get; set; }

        [Required(ErrorMessage = "Bu alan zorunlu")]
        public string Yıl { get; set; }

        [Required(ErrorMessage = "Bu alan zorunlu")]
        public string Cvv { get; set; }
    }
}