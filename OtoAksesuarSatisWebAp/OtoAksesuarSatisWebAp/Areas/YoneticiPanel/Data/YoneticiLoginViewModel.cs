﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OtoAksesuarSatisWebAp.Areas.YoneticiPanel.Data
{
    public class YoneticiLoginViewModel
    {       
        [Display(Name = "Eposta")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Mail alanı boş bırakılamaz")]
        [StringLength(maximumLength: 200, MinimumLength = 5, ErrorMessage = "Bu alan 5 - 200 karakter arasında olabilir")]
        public string Eposta { get; set; }

        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "şifre alanı boş bırakılamaz")]
        [StringLength(maximumLength: 30, MinimumLength = 5, ErrorMessage = "Bu alan 5 - 30 karakter arasında olabilir")]
        public string Sifre { get; set; }
    }
}