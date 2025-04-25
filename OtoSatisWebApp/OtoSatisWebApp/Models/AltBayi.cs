using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OtoSatisWebApp.Models
{
    public class AltBayi
    {
        public int AltBayiID { get; set; }
        public string BayiAdi { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public string Segment { get; set; }
        public DateTime SonXmlGuncellemeTarihi { get; set; }
    }
}