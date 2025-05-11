using OtoAksesuarSatisWebAp.Filters;
using OtoAksesuarSatisWebAp.Models;
using OtoAksesuarSatisWebAp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;

namespace OtoAksesuarSatisWebAp.Controllers
{
    [UyeLoginRequiredFilter]
    public class OdemeController : Controller
    {
        OtoAksesuarSatisDB db = new OtoAksesuarSatisDB();
        public ActionResult PaymentSuccess()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Payment()
        {
            int mid = (Session["uye"] as Uye).UyeID;
            ViewBag.sepet = db.Sepetler.Where(x => x.UyeID == mid).ToList();
            return View();

        }
        [HttpPost]
        public ActionResult Payment(OdemeViewModel model)
        {
            int mid = (Session["uye"] as Uye).UyeID;
            List<Sepet> memberCart = db.Sepetler.Where(x => x.UyeID == mid).ToList();
            if (ModelState.IsValid)
            {
                decimal Total = 0;

                foreach (Sepet item in ViewBag.sepet ?? new List<Sepet>()) 
                {
                    if (item.Urun != null) 
                    {
                        Total += item.Adet * item.Urun.Fiyat;
                    }
                    else
                    {
                        
                    }
                }
                string musteriNumarasi = "159753";
                string musteriSifre = "1234";
                string pricestr = Total.ToString().Replace(",", ".");
                string apiUrl = "https://localhost:44369/api/Pay?musterino=" + musteriNumarasi + "&musterisifre=" + musteriSifre + "&kartno=" + model.KartNumarasi + "&sonkullanmaay=" + model.Ay + "&sonkullanmayil=" + model.Yıl + "&cvv=" + model.Cvv + "&bakiye=" + pricestr;
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.PostAsync(apiUrl, null).Result;
                var result = response.Content.ReadAsStringAsync();
                if (result.Result == "\"101\"")
                {
                    foreach (Sepet item in memberCart)
                    {
                        db.Sepetler.Remove(item);
                    }
                    db.SaveChanges();
                    return RedirectToAction("PaymentSuccess", "Odeme");
                }
                if (result.Result == "\"901\"")
                {
                    ViewBag.hata = "Verilerden en az biri boş";
                }
                if (result.Result == "\"902\"")
                {
                    ViewBag.hata = "Bir Hata Oluştu";
                }
                if (result.Result == "\"801\"")
                {
                    ViewBag.hata = "Pos Müşterisi Bulunamadı";
                }
                if (result.Result == "\"802\"")
                {
                    ViewBag.hata = "Pos Müşterisi İnaktif";
                }
                if (result.Result == "\"701\"")
                {
                    ViewBag.hata = "Kart Bulunamadı";
                }
                if (result.Result == "\"702\"")
                {
                    ViewBag.hata = "Son Kullanma Tarihi Geçmiş";
                }
                if (result.Result == "\"703\"")
                {
                    ViewBag.hata = "Güvenlik Kodu Hatalı";
                }
                if (result.Result == "\"601\"")
                {
                    ViewBag.hata = "Bakiye Yetersiz";
                }
            }

            ViewBag.sepet = memberCart;
            return View(model);
        }
        [HttpGet]
        public ActionResult PaymentXML()
        {
            int mid = (Session["uye"] as Uye).UyeID;
            ViewBag.sepet = db.Sepetler.Where(x => x.UyeID == mid).ToList();
            return View();

        }
        [HttpPost]
        public ActionResult PaymentXML(OdemeViewModel model)
        {
            int mid = (Session["uye"] as Uye).UyeID;
            List<Sepet> memberCart = db.Sepetler.Where(x => x.UyeID == mid).ToList();

            if (ModelState.IsValid)
            {
                decimal total = 0;

                foreach (var item in memberCart)
                {
                    string itemFiyatSeviyesi = "Bronz";
                    var yonetici = Session["YoneticiSession"] as Yonetici;

                    if (yonetici != null)
                    {
                        itemFiyatSeviyesi = yonetici.YoneticiIsim;
                    }

                    decimal urunFiyati = 0;

                    if (item.XMLUrun != null)
                    {
                        itemFiyatSeviyesi = string.IsNullOrEmpty(itemFiyatSeviyesi) ? "bronz" : itemFiyatSeviyesi.ToLower();

                        if (itemFiyatSeviyesi == "bronz")
                        {
                            urunFiyati = item.XMLUrun.BronzFiyat;
                        }
                        else if (itemFiyatSeviyesi == "silver")
                        {
                            urunFiyati = item.XMLUrun.SilverFiyat;
                        }
                        else if (itemFiyatSeviyesi == "gold")
                        {
                            urunFiyati = item.XMLUrun.GoldFiyat;
                        }
                        else
                        {
                            urunFiyati = item.XMLUrun.BronzFiyat;
                        }
                    }

                    total += urunFiyati * item.Adet;
                }
                string musteriNumarasi = "159753";
                string musteriSifre = "1234";
                string pricestr = total.ToString().Replace(",", ".");

                string apiUrl = $"https://localhost:44369/api/Pay?musterino={musteriNumarasi}&musterisifre={musteriSifre}&kartno={model.KartNumarasi}&sonkullanmaay={model.Ay}&sonkullanmayil={model.Yıl}&cvv={model.Cvv}&bakiye={pricestr}";

                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.PostAsync(apiUrl, null).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                if (result == "\"101\"")
                {
                    foreach (Sepet item in memberCart)
                    {
                        db.Sepetler.Remove(item);
                    }
                    db.SaveChanges();
                    return RedirectToAction("PaymentSuccess", "Odeme");
                }
                else if (result == "\"901\"")
                {
                    ViewBag.hata = "Verilerden en az biri boş";
                }
                else if (result == "\"902\"")
                {
                    ViewBag.hata = "Bir Hata Oluştu";
                }
                else if (result == "\"801\"")
                {
                    ViewBag.hata = "Pos Müşterisi Bulunamadı";
                }
                else if (result == "\"802\"")
                {
                    ViewBag.hata = "Pos Müşterisi İnaktif";
                }
                else if (result == "\"701\"")
                {
                    ViewBag.hata = "Kart Bulunamadı";
                }
                else if (result == "\"702\"")
                {
                    ViewBag.hata = "Son Kullanma Tarihi Geçmiş";
                }
                else if (result == "\"703\"")
                {
                    ViewBag.hata = "Güvenlik Kodu Hatalı";
                }
                else if (result == "\"601\"")
                {
                    ViewBag.hata = "Bakiye Yetersiz";
                }
                else
                {
                    ViewBag.hata = "Bilinmeyen hata";
                }
            }

            ViewBag.sepet = memberCart;
            return View(model);
        }
           
        
    }
}