using OtoAksesuarSatisWebAp.Areas.YoneticiPanel.Filters;
using OtoAksesuarSatisWebAp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        
        public ActionResult Index()
        {

            var yonetici = Session["YoneticiSession"] as Yonetici;
            var tip = yonetici?.YoneticiIsim.ToLower();

            var urunler = db.Urunler.ToList();

            foreach (var urun in urunler)
            {
                switch (tip)
                {
                    case "bronz":
                        urun.Fiyat = urun.BronzFiyat;
                        break;
                    case "silver":
                        urun.Fiyat = urun.SilverFiyat;
                        break;
                    case "gold":
                        urun.Fiyat = urun.GoldFiyat;
                        break;
                }
            }

            return View(urunler);
        }
        [HttpPost]
        public ActionResult UrunleriAktar()
        {
            var yonetici = Session["YoneticiSession"] as Yonetici;

            if (yonetici == null)
            {
                return RedirectToAction("Index", "YoneticiGiris");
            }

            string bayiTipi = yonetici.YoneticiIsim.ToLower(); 
            string xmlDosyaYolu = $@"C:\BayilikXML\{bayiTipi}.xml";

            if (!System.IO.File.Exists(xmlDosyaYolu))
            {
                TempData["Mesaj"] = "XML dosyası bulunamadı.";
                return RedirectToAction("Index", "HomePanel");
            }

            var xmlDoc = XDocument.Load(xmlDosyaYolu);

            var xmlUrunler = xmlDoc.Descendants("urun").Select(x => new
            {
                UrunAdi = x.Element("UrunAdi")?.Value ?? "Bilinmeyen",
                KategoriAdi = x.Element("Kategori")?.Value ?? "Genel",
                MarkaAdi = x.Element("Marka")?.Value ?? "Markasız",
                BronzFiyat = decimal.TryParse(x.Element("BronzFiyat")?.Value.Replace("₺", "").Replace(",", "."), out var b) ? b : 0,
                SilverFiyat = decimal.TryParse(x.Element("SilverFiyat")?.Value.Replace("₺", "").Replace(",", "."), out var s) ? s : 0,
                GoldFiyat = decimal.TryParse(x.Element("GoldFiyat")?.Value.Replace("₺", "").Replace(",", "."), out var g) ? g : 0,
                Stok = int.TryParse(x.Element("Stok")?.Value, out var stok) ? stok : 0,
                Aciklama = x.Element("Aciklama")?.Value ?? "",
                Resim = x.Element("Resim")?.Value ?? "resim_yok.jpg",
                EklenmeZamani = DateTime.TryParse(x.Element("EklenmeZamani")?.Value, out var tarih) ? tarih : DateTime.Now
            }).ToList();

            int eklenen = 0;

            foreach (var x in xmlUrunler)
            {
                var kategori = db.Kategoriler.FirstOrDefault(k => k.KategoriAdi == x.KategoriAdi)
                    ?? db.Kategoriler.FirstOrDefault(k => k.KategoriID == 1);

                var marka = db.Markalar.FirstOrDefault(m => m.MarkaAdi == x.MarkaAdi)
                    ?? db.Markalar.FirstOrDefault(m => m.MarkaID == 1);

                if (kategori == null || marka == null) continue;

                string xmlUrunAdi = x.UrunAdi.Trim().ToLower();
                bool urunVarMi = db.Urunler.Any(u => u.UrunAdi.Trim().ToLower() == xmlUrunAdi);

                if (urunVarMi)
                {
                    Debug.WriteLine($"Ürün zaten var: {x.UrunAdi}");
                    continue;
                }

                decimal secilenFiyat;

                switch (bayiTipi)
                {
                    case "bronz":
                        secilenFiyat = x.BronzFiyat;
                        break;
                    case "silver":
                        secilenFiyat = x.SilverFiyat;
                        break;
                    case "gold":
                        secilenFiyat = x.GoldFiyat;
                        break;
                    default:
                        secilenFiyat = x.BronzFiyat;
                        break;
                }

                var urun = new Urun
                {
                    UrunAdi = x.UrunAdi,
                    Fiyat = secilenFiyat,
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

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                TempData["Mesaj"] = $"Veritabanına eklerken hata oluştu: {ex.Message}";
                return RedirectToAction("Index", "HomePanel");
            }

            TempData["Mesaj"] = eklenen > 0
                ? $"{eklenen} ürün başarıyla aktarıldı."
                : "Hiç yeni ürün eklenmedi. Zaten tüm ürünler veritabanında olabilir.";

            return RedirectToAction("Index", "HomePanel");
        }
    }

    




}