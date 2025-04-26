using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OtoSatisWebApp.Models
{
    [Table("AltBayiler")]
    public class AltBayii
    {
        [Key]
        public int AltBayiID { get; set; }  
        public string BayiAdi { get; set; } 
        public string Segment { get; set; } 
        public DateTime SonXmlGuncellemeTarihi { get; set; }  
    }
}