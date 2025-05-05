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

            var xmlUrunler = db.XMLUrunler.ToList();

            ViewBag.Urunler = xmlUrunler;

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

            string xmlDosyaYolu = @"C:\BayilikXML\Urunler.xml";

            if (!System.IO.File.Exists(xmlDosyaYolu))
            {
                TempData["Mesaj"] = "XML dosyası bulunamadı.";
                return RedirectToAction("Index", "HomePanel");
            }

            var xmlDoc = XDocument.Load(xmlDosyaYolu);

            var xmlUrunler = xmlDoc.Descendants("urun")
                .Select(x => new XMLUrun
                {
                    UrunAdi = x.Element("UrunAdi")?.Value ?? "Bilinmeyen",
                    Kategori = (x.Element("Kategori")?.Value ?? "").Trim(),
                    Marka = (x.Element("Marka")?.Value ?? "").Trim(),
                    BronzFiyat = decimal.TryParse(x.Element("BronzFiyat")?.Value.Replace(",", "."), out var b) ? b : 0,
                    SilverFiyat = decimal.TryParse(x.Element("SilverFiyat")?.Value.Replace(",", "."), out var s) ? s : 0,
                    GoldFiyat = decimal.TryParse(x.Element("GoldFiyat")?.Value.Replace(",", "."), out var g) ? g : 0,
                    Stok = int.TryParse(x.Element("Stok")?.Value, out var stok) ? stok : 0,
                    Aciklama = string.IsNullOrWhiteSpace(x.Element("Aciklama")?.Value) ? "Açıklama yok" : x.Element("Aciklama")?.Value,
                    Resim = string.IsNullOrWhiteSpace(x.Element("Resim")?.Value) ? "resim_yok.jpg" : x.Element("Resim")?.Value,
                    EklenmeZamani = DateTime.TryParse(x.Element("EklenmeZamani")?.Value, out var eklenmeZamani) ? eklenmeZamani : DateTime.Now
                }).ToList();

            int eklenen = 0;
            string hataMesajlari = "";

            foreach (var urun in xmlUrunler)
            {
                bool zatenVar = db.XMLUrunler.Any(x =>
                    x.UrunAdi == urun.UrunAdi &&
                    x.Kategori == urun.Kategori &&
                    x.Marka == urun.Marka);

                if (zatenVar)
                {
                    continue;
                }

                try
                {
                    db.XMLUrunler.Add(urun);
                    db.SaveChanges();
                    eklenen++;
                }
                catch (Exception ex)
                {
                    hataMesajlari += $"<br/>Hata: {urun.UrunAdi} - {ex.Message}";
                    continue;
                }
            }

            TempData["Mesaj"] = eklenen > 0
                ? $"{eklenen} XML ürünü başarıyla aktarıldı." + hataMesajlari
                : "Tüm XML ürünleri zaten aktarılmış. Yeni ürün bulunamadı." + hataMesajlari;

            return RedirectToAction("Index", "HomePanel");
        }
    }






}