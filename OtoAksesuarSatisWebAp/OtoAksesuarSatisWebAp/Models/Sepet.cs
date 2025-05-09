using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OtoAksesuarSatisWebAp.Models
{
    public class Sepet
    {
        public int ID { get; set; }
       
        public int? UrunID { get; set; }

        [ForeignKey("UrunID")]
        public virtual Urun Urun { get; set; }

        public int? XmlUrunID { get; set; }

        [ForeignKey("XmlUrunID")]
        public virtual XMLUrun XMLUrun { get; set; }

        public int UyeID { get; set; }

        [ForeignKey("UyeID")]
        public virtual Uye Uye { get; set; }

        public int Adet { get; set; }
    }
}