using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OtoAksesuarSatisWebAp.Models
{
    public class Favori
    {
        [Key]
        public int FavoriID { get; set; }

        public int  UrunID { get; set; }

        [ForeignKey("UrunID")]
        public virtual Urun Urun { get; set; }

        public int UyeID { get; set; }

        [ForeignKey("UyeID")]

        public virtual Uye Uye { get; set; }

        public bool Silinmis { get; set; }

    }
}