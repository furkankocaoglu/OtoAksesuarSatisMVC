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
                Yonetici y = db.Yoneticiler.FirstOrDefault(x => x.Eposta == model.Eposta && x.Sifre == model.Sifre);
                if (y != null)
                {
                    if (y.AktifMi)
                    {
                        Session["YoneticiSession"] = y;

                        var segment = y.YoneticiIsim.ToLower();

                        string xmlKlasorYolu = @"C:\BayilikXML\";
                        if (!Directory.Exists(xmlKlasorYolu)) Directory.CreateDirectory(xmlKlasorYolu);

                        string xmlPath = Path.Combine(xmlKlasorYolu, $"{segment}.xml");

                        if (System.IO.File.Exists(xmlPath))
                        {
                            XDocument xmlDoc = XDocument.Load(xmlPath);

                            var urunler = xmlDoc.Descendants("urun")
                                .Select(x => new Urun
                                {
                                    UrunAdi = x.Element("UrunAdi")?.Value,
                                    Fiyat = Convert.ToDecimal(x.Element("Fiyat")?.Value.Replace("₺", "").Replace(",", ".")),
                                    StokMiktari = int.Parse(x.Element("Stok")?.Value),
                                    Aciklama = x.Element("Aciklama")?.Value,
                                    ResimYolu = x.Element("Resim")?.Value,
                                    
                                    EklenmeTarihi = DateTime.TryParse(x.Element("EklenmeZamani")?.Value, out var eklenmeTarihi) ? eklenmeTarihi : DateTime.MinValue
                                }).ToList();

                            
                            if (urunler.Any())
                            {
                                Session["Urunler"] = urunler;
                                return RedirectToAction("Index", "HomePanel");
                            }
                            else
                            {
                                ViewBag.mesaj = "XML dosyasından ürünler alınamadı.";
                            }
                        }
                        else
                        {
                            ViewBag.mesaj = "XML dosyasına ulaşılamadı.";
                        }
                    }
                    else
                    {
                        ViewBag.mesaj = "Kullanıcı hesabınız askıya alınmıştır.";
                    }
                }
                else
                {
                    ViewBag.mesaj = "Kullanıcı bulunamadı.";
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