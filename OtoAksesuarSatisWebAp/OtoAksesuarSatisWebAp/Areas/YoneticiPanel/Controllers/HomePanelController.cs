using OtoAksesuarSatisWebAp.Areas.YoneticiPanel.Filters;
using OtoAksesuarSatisWebAp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace OtoAksesuarSatisWebAp.Areas.YoneticiPanel.Controllers
{
    [YoneticiLoginRequiredFilter]
    public class HomePanelController : Controller
    {
        OtoAksesuarSatisDB db = new OtoAksesuarSatisDB();
        //sadasfdmsfsssssssssssssssssssssssssssssssssssssssssssssss
        public ActionResult Index()
        {
           
            var yonetici = Session["YoneticiSession"] as Yonetici;

            if (yonetici == null)
            {
                return RedirectToAction("Index", "YoneticiGiris");
            }

            var urunler = Session["Urunler"] as List<Urun>;

            if (urunler != null)
            {
                ViewBag.Urunler = urunler;
            }
            else
            {
                ViewBag.Urunler = "XML dosyasına ulaşılamadı.";
            }

            return View();
        }
        [HttpPost]
        public ActionResult UrunleriAktar()
        {
            var yonetici = Session["YoneticiSession"] as Yonetici;

            if (yonetici == null)
            {
                return RedirectToAction("Index", "YoneticiGiris");
            }

            string bayiTipi = yonetici.YoneticiIsim; 
            string xmlDosyaYolu = $@"C:\BayilikXML\{bayiTipi}.xml";

            if (!System.IO.File.Exists(xmlDosyaYolu))
            {
                TempData["Mesaj"] = "XML dosyası bulunamadı.";
                return RedirectToAction("Index","HomePanel");
            }

            var xmlDoc = XDocument.Load(xmlDosyaYolu);
            
            var xmlUrunler = xmlDoc.Descendants("urun")
                .Select(x => new
                {
                    UrunID = int.TryParse(x.Element("UrunID")?.Value, out var urunId) ? urunId : 0,
                    UrunAdi = x.Element("UrunAdi")?.Value ?? "Bilinmeyen",
                    KategoriAdi = x.Element("Kategori")?.Value ?? "Bilinmeyen Kategori",
                    MarkaAdi = x.Element("Marka")?.Value ?? "Bilinmeyen Marka",
                    Fiyat = decimal.TryParse(x.Element("Fiyat")?.Value.Replace("₺", "").Replace(",", "."), out var fiyat) ? fiyat : 0,
                    Stok = int.TryParse(x.Element("Stok")?.Value, out var stok) ? stok : 0,
                    Aciklama = x.Element("Aciklama")?.Value ?? "Açıklama yok",
                    Resim = x.Element("Resim")?.Value ?? "resim_yok.jpg", 
                    EklenmeZamani = DateTime.TryParse(x.Element("EklenmeZamani")?.Value, out var eklenmeZamani) ? eklenmeZamani : DateTime.MinValue
                }).ToList();

            int eklenen = 0;

            foreach (var x in xmlUrunler)
            {
                
                var kategori = db.Kategoriler.FirstOrDefault(k => k.KategoriAdi == x.KategoriAdi);
                if (kategori == null)
                {
                    
                    kategori = db.Kategoriler.FirstOrDefault(k => k.KategoriID == 11);
                    if (kategori == null)
                    {
                        
                        TempData["Mesaj"] = "Kategori bulunamadı ve ID 11 olan kategori veritabanında yok.";
                        continue;
                    }
                }

               
                var marka = db.Markalar.FirstOrDefault(m => m.MarkaAdi == x.MarkaAdi);
                if (marka == null)
                {
                    
                    marka = db.Markalar.FirstOrDefault(m => m.MarkaID == 6);
                    if (marka == null)
                    {
                        
                        TempData["Mesaj"] = "Marka bulunamadı ve ID 6 olan marka veritabanında yok.";
                        continue;
                    }
                }

                bool urunVarMi = db.Urunler.Any(u => u.UrunAdi == x.UrunAdi && u.Fiyat == x.Fiyat);
                if (urunVarMi) continue;

                
                var urun = new Urun
                {
                    UrunAdi = x.UrunAdi,
                    Fiyat = x.Fiyat,
                    StokMiktari = x.Stok,
                    Aciklama = x.Aciklama,
                    ResimYolu = x.Resim,
                    EklenmeTarihi = x.EklenmeZamani,
                    AktifMi = true,
                    Silinmis = false,
                    KategoriID = kategori.KategoriID, 
                    MarkaID = marka.MarkaID 
                };

                
                db.Urunler.Add(urun);
                eklenen++;
            }

            db.SaveChanges();

            if (eklenen == 0)
            {
                TempData["Mesaj"] = "Zaten tüm ürünler başarıyla aktarıldı.";
            }
            else
            {
                TempData["Mesaj"] = $"{eklenen} yeni ürün başarıyla aktarıldı.";
            }

            return RedirectToAction("Index", "HomePanel");
        }

    }


    
    
}