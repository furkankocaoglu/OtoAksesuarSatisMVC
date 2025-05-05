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
                return RedirectToAction("Index", "HomePanel");
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
                    EklenmeZamani = DateTime.TryParse(x.Element("EklenmeZamani")?.Value, out var eklenmeZamani) ? eklenmeZamani : DateTime.Now
                }).ToList();

            int eklenen = 0;

            foreach (var x in xmlUrunler)
            {
                // Kategori bul
                var kategori = db.Kategoriler.FirstOrDefault(k => k.KategoriAdi == x.KategoriAdi);
                if (kategori == null)
                {
                    kategori = db.Kategoriler.FirstOrDefault(k => k.KategoriID == 1); // Genel kategori
                    if (kategori == null)
                    {
                        TempData["Mesaj"] = "Kategori bulunamadı ve ID 1 olan 'Genel' kategori yok.";
                        continue;
                    }
                }

                // Marka bul
                var marka = db.Markalar.FirstOrDefault(m => m.MarkaAdi == x.MarkaAdi);
                if (marka == null)
                {
                    marka = db.Markalar.FirstOrDefault(m => m.MarkaID == 1); // Genel marka
                    if (marka == null)
                    {
                        TempData["Mesaj"] = "Marka bulunamadı ve ID 1 olan 'Genel' marka yok.";
                        continue;
                    }
                }

                // Aynı ürün zaten eklenmiş mi kontrol et (sadece ürün adına ve kategoriye göre)
                bool urunVarMi = db.Urunler.Any(u => u.UrunAdi == x.UrunAdi && u.KategoriID == kategori.KategoriID);
                if (urunVarMi)
                {
                    continue;
                }

                // Ürünü oluştur
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

                try
                {
                    db.Urunler.Add(urun);
                    db.SaveChanges();
                    eklenen++;
                }
                catch (Exception ex)
                {
                    TempData["Mesaj"] = "Veritabanı hatası: " + ex.Message;
                    continue;
                }
            }

            if (eklenen == 0)
            {
                TempData["Mesaj"] = "Zaten tüm ürünler aktarılmış. Yeni ürün bulunamadı.";
            }
            else
            {
                TempData["Mesaj"] = $"{eklenen} yeni ürün başarıyla aktarıldı.";
            }
            Session["Urunler"] = null;

            return RedirectToAction("Index", "HomePanel");
        }
    }

    




}