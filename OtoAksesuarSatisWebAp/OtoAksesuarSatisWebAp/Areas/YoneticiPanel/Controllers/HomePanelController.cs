using OtoAksesuarSatisWebAp.Areas.YoneticiPanel.Filters;
using OtoAksesuarSatisWebAp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

            ViewBag.Urunler = db.XMLUrunler.ToList();
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

            var xmlUrunler = xmlDoc.Descendants("urun").Select(x => new XMLUrun
            {
                UrunAdi = x.Element("UrunAdi")?.Value.Trim() ?? "Bilinmeyen",
                Kategori = x.Element("Kategori")?.Value.Trim() ?? "Genel",
                Marka = x.Element("Marka")?.Value.Trim() ?? "Markasız",
                BronzFiyat = decimal.TryParse(x.Element("BronzFiyat")?.Value.Replace(",", "."), out var b) ? b : 0,
                SilverFiyat = decimal.TryParse(x.Element("SilverFiyat")?.Value.Replace(",", "."), out var s) ? s : 0,
                GoldFiyat = decimal.TryParse(x.Element("GoldFiyat")?.Value.Replace(",", "."), out var g) ? g : 0,
                Stok = int.TryParse(x.Element("Stok")?.Value, out var stok) ? stok : 0,
                Aciklama = x.Element("Aciklama")?.Value.Trim() ?? "Açıklama yok",
                Resim = x.Element("Resim")?.Value.Trim() ?? "resim_yok.jpg",
                EklenmeZamani = DateTime.TryParse(x.Element("EklenmeZamani")?.Value, out var dt) ? dt : DateTime.Now
            }).ToList();

            int eklenen = 0, guncellenen = 0;

            foreach (var urun in xmlUrunler)
            {
                var mevcut = db.XMLUrunler.FirstOrDefault(x =>
                    x.UrunAdi.ToLower().Trim() == urun.UrunAdi.ToLower().Trim() &&
                    x.Kategori.ToLower().Trim() == urun.Kategori.ToLower().Trim() &&
                    x.Marka.ToLower().Trim() == urun.Marka.ToLower().Trim());

                if (mevcut != null)
                {
                    bool degisti = false;

                   
                    if (mevcut.BronzFiyat != urun.BronzFiyat) { mevcut.BronzFiyat = urun.BronzFiyat; degisti = true; }
                    if (mevcut.SilverFiyat != urun.SilverFiyat) { mevcut.SilverFiyat = urun.SilverFiyat; degisti = true; }
                    if (mevcut.GoldFiyat != urun.GoldFiyat) { mevcut.GoldFiyat = urun.GoldFiyat; degisti = true; }
                    if (mevcut.Stok != urun.Stok) { mevcut.Stok = urun.Stok; degisti = true; }
                    if (mevcut.Aciklama != urun.Aciklama) { mevcut.Aciklama = urun.Aciklama; degisti = true; }
                    if (mevcut.Resim != urun.Resim) { mevcut.Resim = urun.Resim; degisti = true; }
                    if (mevcut.EklenmeZamani != urun.EklenmeZamani) { mevcut.EklenmeZamani = urun.EklenmeZamani; degisti = true; }

                    if (degisti)
                    {
                        guncellenen++;
                    }
                }
                else
                {
                    db.XMLUrunler.Add(urun); 
                    eklenen++;
                }
            }

            db.SaveChanges(); 

            TempData["Mesaj"] = $"{eklenen} ürün eklendi, {guncellenen} ürün güncellendi.";
            return RedirectToAction("Index", "HomePanel");
        }
    }






}