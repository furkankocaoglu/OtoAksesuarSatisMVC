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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var y = db.Yoneticiler.FirstOrDefault(x => x.Eposta == model.Eposta && x.Sifre == model.Sifre);

            if (y != null && y.AktifMi)
            {
                Session["YoneticiSession"] = y;
                Session["FiyatSeviyesi"] = y.YoneticiIsim;

                string xmlDosyaYolu = @"C:\BayilikXML\Urunler.xml";

                if (!System.IO.File.Exists(xmlDosyaYolu))
                {
                    ViewBag.mesaj = "Urunler.xml dosyası bulunamadı.";
                    return View(model);
                }

                try
                {
                    var xmlDoc = XDocument.Load(xmlDosyaYolu);

                    var xmlUrunler = xmlDoc.Descendants("urun").Select(x => new XMLUrun
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

                    int eklenen = 0;
                    int guncellenen = 0;

                    var tumDbUrunler = db.XMLUrunler.ToList();

                    
                    foreach (var dbUrun in tumDbUrunler)
                    {
                        bool xmldeVarMi = xmlUrunler.Any(x =>
                            x.UrunAdi.ToLower().Trim() == dbUrun.UrunAdi.ToLower().Trim() &&
                            x.Kategori.ToLower().Trim() == dbUrun.Kategori.ToLower().Trim() &&
                            x.Marka.ToLower().Trim() == dbUrun.Marka.ToLower().Trim());

                        if (!xmldeVarMi)
                        {
                            db.XMLUrunler.Remove(dbUrun); 
                        }
                    }


                    foreach (var urun in xmlUrunler)
                    {
                        var mevcut = db.XMLUrunler.FirstOrDefault(x =>
                            x.UrunAdi.ToLower().Trim() == urun.UrunAdi.ToLower().Trim() &&
                            x.Kategori.ToLower().Trim() == urun.Kategori.ToLower().Trim() &&
                            x.Marka.ToLower().Trim() == urun.Marka.ToLower().Trim());

                        if (mevcut != null)
                        {
                            bool degisti = false;
                           
                            if (mevcut.UrunAdi != urun.UrunAdi)
                            {
                                mevcut.UrunAdi = urun.UrunAdi;
                                degisti = true;
                            }


                            if (mevcut.BronzFiyat != urun.BronzFiyat) { mevcut.BronzFiyat = urun.BronzFiyat; degisti = true; }
                            if (mevcut.SilverFiyat != urun.SilverFiyat) { mevcut.SilverFiyat = urun.SilverFiyat; degisti = true; }
                            if (mevcut.GoldFiyat != urun.GoldFiyat) { mevcut.GoldFiyat = urun.GoldFiyat; degisti = true; }
                            if (mevcut.Stok != urun.Stok) { mevcut.Stok = urun.Stok; degisti = true; }
                            if (mevcut.Aciklama != urun.Aciklama) { mevcut.Aciklama = urun.Aciklama; degisti = true; }
                            if (mevcut.Resim != urun.Resim) { mevcut.Resim = urun.Resim; degisti = true; }
                            if (mevcut.EklenmeZamani != urun.EklenmeZamani) { mevcut.EklenmeZamani = urun.EklenmeZamani; degisti = true; }

                           
                            if (degisti && !string.IsNullOrEmpty(urun.Resim) && urun.Resim != "resim_yok.jpg")
                            {
                              
                                string kaynakYolu = Path.Combine(@"C:\Images", Path.GetFileName(urun.Resim));

                             
                                string hedefKlasor = Server.MapPath("~/Images/");
                                string hedefYolu = Path.Combine(hedefKlasor, Path.GetFileName(urun.Resim));

                                
                                if (!System.IO.File.Exists(hedefYolu) && System.IO.File.Exists(kaynakYolu))
                                {
                                    System.IO.File.Copy(kaynakYolu, hedefYolu);
                                }
                            }

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

                    TempData["Mesaj"] = $"{eklenen} ürün eklendi, {guncellenen} ürün güncellendi. XML'de olmayan ürünler silindi.";
                    return RedirectToAction("Index", "HomePanel");
                }
                catch (Exception ex)
                {
                    ViewBag.mesaj = "XML dosyası işlenirken hata: " + ex.Message;
                    return View(model);
                }
            }

            ViewBag.mesaj = y == null
                ? "Kullanıcı bulunamadı."
                : "Kullanıcı hesabınız askıya alınmıştır.";

            return View(model);

        }

        public ActionResult LogOut()
        {
            
            Session["YoneticiSession"] = null;
            return RedirectToAction("Index", "YoneticiGiris");
        }
    }
}