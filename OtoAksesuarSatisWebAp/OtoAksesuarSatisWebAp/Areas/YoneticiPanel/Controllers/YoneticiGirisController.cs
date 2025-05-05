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
            // Model geçerliyse işleme başla
            if (ModelState.IsValid)
            {
                // Kullanıcıyı veritabanından sorguluyoruz
                Yonetici y = db.Yoneticiler.FirstOrDefault(x => x.Eposta == model.Eposta && x.Sifre == model.Sifre);

                // Kullanıcı bulunduysa ve aktifse giriş yapılıyor
                if (y != null)
                {
                    if (y.AktifMi)
                    {
                        // Kullanıcıyı session'a atıyoruz
                        Session["YoneticiSession"] = y;

                        // Kullanıcının adını küçük harfe çevirip xml dosya yolunu oluşturuyoruz
                        var segment = y.YoneticiIsim.ToLower();
                        string xmlKlasorYolu = @"C:\BayilikXML\";

                        // Eğer XML klasörü yoksa oluşturuyoruz
                        if (!Directory.Exists(xmlKlasorYolu))
                            Directory.CreateDirectory(xmlKlasorYolu);

                        // XML dosyasının tam yolu
                        string xmlPath = Path.Combine(xmlKlasorYolu, $"{segment}.xml");

                        // XML dosyasına erişiyoruz
                        if (System.IO.File.Exists(xmlPath))
                        {
                            XDocument xmlDoc = XDocument.Load(xmlPath);

                            // XML'den ürün verilerini alıyoruz
                            var urunler = xmlDoc.Descendants("urun")
                                .Select(x => new Urun
                                {
                                    UrunAdi = x.Element("UrunAdi")?.Value,
                                    Fiyat = Convert.ToDecimal(x.Element("Fiyat")?.Value.Replace("₺", "").Replace(",", ".")),
                                    StokMiktari = int.Parse(x.Element("Stok")?.Value),
                                    Aciklama = x.Element("Aciklama")?.Value,
                                    ResimYolu = x.Element("Resim")?.Value,

                                    // EklenmeZamani'na nullable kontrolü ekliyoruz
                                    EklenmeTarihi = DateTime.TryParse(x.Element("EklenmeZamani")?.Value, out DateTime parsedDate)
                                        ? parsedDate
                                        : DateTime.MinValue // Eğer null ise varsayılan tarih değerini kullanıyoruz
                                }).ToList();

                            // Ürünleri session'a atıyoruz
                            Session["Urunler"] = urunler;

                            // Anasayfaya yönlendiriyoruz
                            return RedirectToAction("Index", "HomePanel");
                        }
                        else
                        {
                            // XML dosyasına erişilemediyse mesaj veriyoruz
                            ViewBag.mesaj = "XML dosyasına ulaşılamadı.";
                        }
                    }
                    else
                    {
                        // Kullanıcı aktif değilse mesaj veriyoruz
                        ViewBag.mesaj = "Kullanıcı hesabınız askıya alınmıştır.";
                    }
                }
                else
                {
                    // Kullanıcı bulunamazsa mesaj veriyoruz
                    ViewBag.mesaj = "Kullanıcı bulunamadı.";
                }
            }

            // Model geçerli değilse veya hata oluştuysa tekrar formu gösteriyoruz
            return View(model);
        }

        public ActionResult LogOut()
        {
            
            Session["YoneticiSession"] = null;
            return RedirectToAction("Index", "YoneticiGiris");
        }
    }
}