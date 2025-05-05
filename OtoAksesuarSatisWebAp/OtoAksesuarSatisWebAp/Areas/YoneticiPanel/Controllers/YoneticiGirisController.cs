using OtoAksesuarSatisWebAp.Areas.YoneticiPanel.Data;
using OtoAksesuarSatisWebAp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace OtoAksesuarSatisWebAp.Areas.YoneticiPanel.Controllers
{
    public class YoneticiGirisController : Controller
    {
        OtoAksesuarSatisDB db = new OtoAksesuarSatisDB();

        [HttpGet]
        public ActionResult Index()
        {
            if (Session["YoneticiSession"] != null)
            {
                return RedirectToAction("Index", "HomePanel");
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(YoneticiLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcı doğrulama
                var y = db.Yoneticiler.FirstOrDefault(x => x.Eposta == model.Eposta && x.Sifre == model.Sifre);

                if (y != null && y.AktifMi)
                {
                    Session["YoneticiSession"] = y;
                    Session["FiyatSeviyesi"] = y.YoneticiIsim;

                    string xmlDosyaYolu = @"C:\BayilikXML\Urunler.xml";

                    if (System.IO.File.Exists(xmlDosyaYolu))
                    {
                        try
                        {
                            XDocument xmlDoc = XDocument.Load(xmlDosyaYolu);

                            var xmlUrunler = xmlDoc.Descendants("urun")
                                .Select(x => new XMLUrun
                                {
                                    UrunAdi = x.Element("UrunAdi")?.Value ?? "Bilinmeyen",
                                    Kategori = x.Element("Kategori")?.Value ?? "Genel",
                                    Marka = x.Element("Marka")?.Value ?? "Markasız",
                                    BronzFiyat = decimal.TryParse(x.Element("BronzFiyat")?.Value.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var b) ? b : 0,
                                    SilverFiyat = decimal.TryParse(x.Element("SilverFiyat")?.Value.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var s) ? s : 0,
                                    GoldFiyat = decimal.TryParse(x.Element("GoldFiyat")?.Value.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var g) ? g : 0,
                                    Stok = int.TryParse(x.Element("Stok")?.Value, out var stok) ? stok : 0,
                                    Aciklama = x.Element("Aciklama")?.Value ?? "Açıklama yok",
                                    Resim = x.Element("Resim")?.Value ?? "resim_yok.jpg",
                                    EklenmeZamani = DateTime.TryParse(x.Element("EklenmeZamani")?.Value, out var dt) ? dt : DateTime.Now
                                }).ToList();

                            // Yeni ürünleri XMLUrunler tablosuna ekle (aynı ada sahip olanlar atlanır)
                            int eklenen = 0;
                            foreach (var urun in xmlUrunler)
                            {
                                bool varMi = db.XMLUrunler.Any(x =>
                                    x.UrunAdi == urun.UrunAdi &&
                                    x.Marka == urun.Marka &&
                                    x.Kategori == urun.Kategori);

                                if (!varMi)
                                {
                                    db.XMLUrunler.Add(urun);
                                    eklenen++;
                                }
                            }

                            if (eklenen > 0)
                            {
                                db.SaveChanges();
                                TempData["Mesaj"] = $"{eklenen} ürün XMLUrunler tablosuna başarıyla aktarıldı.";
                            }
                            else
                            {
                                TempData["Mesaj"] = "Zaten tüm XML ürünleri eklenmişti. Yeni ürün bulunamadı.";
                            }

                            return RedirectToAction("Index", "HomePanel");
                        }
                        catch (Exception ex)
                        {
                            ViewBag.mesaj = "XML dosyası işlenirken bir hata oluştu: " + ex.Message;
                        }
                    }
                    else
                    {
                        ViewBag.mesaj = "Urunler.xml dosyası bulunamadı.";
                    }
                }
                else
                {
                    ViewBag.mesaj = y == null
                        ? "Kullanıcı bulunamadı."
                        : "Kullanıcı hesabınız askıya alınmıştır.";
                }
            }

            return View(model);

        }

        public ActionResult LogOut()
        {
            
            Session["YoneticiSession"] = null;
            return RedirectToAction("Index", "YoneticiGiris");
        }
    }
}