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
            List<Sepet> uyeSepet = db.Sepetler.Where(x => x.UyeID == mid).ToList();

            if (ModelState.IsValid)
            {
                decimal total = 0;

                foreach (Sepet item in ViewBag.sepet ?? new List<Sepet>())
                {
                    if (item.Urun != null)
                    {
                        total += item.Adet * item.Urun.Fiyat;
                    }
                }
                string musteriNumarasi = "135789";
                string musteriSifre = "1234";
                string priceStr = total.ToString().Replace(",", ".");
                string apiUrl = $"https://localhost:44369/api/Pay?musterino={musteriNumarasi}&musterisifre={musteriSifre}&kartno={model.KartNumarasi}&sonkullanmaay={model.Ay}&sonkullanmayil={model.Yıl}&cvv={model.Cvv}&bakiye={priceStr}";

                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.PostAsync(apiUrl, null).Result;
                var result = response.Content.ReadAsStringAsync().Result;
               
                if (result == "\"101\"")
                {
                    foreach (Sepet item in uyeSepet)
                    {
                        db.Sepetler.Remove(item);
                    }
                    db.SaveChanges();
                    return RedirectToAction("PaymentSuccess", "Odeme");
                }
                else
                {    
                    ViewBag.hata = "Ödeme işlemi başarısız oldu, lütfen tekrar deneyin.";
                }
            }
            ViewBag.sepet = uyeSepet;
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
            List<Sepet> uyeSepet = db.Sepetler.Where(x => x.UyeID == mid).ToList();

            if (ModelState.IsValid)
            {
                decimal total = 0;

                foreach (var item in uyeSepet)
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

                        switch (itemFiyatSeviyesi)
                        {
                            case "bronz":
                                urunFiyati = item.XMLUrun.BronzFiyat;
                                break;
                            case "silver":
                                urunFiyati = item.XMLUrun.SilverFiyat;
                                break;
                            case "gold":
                                urunFiyati = item.XMLUrun.GoldFiyat;
                                break;
                            default:
                                urunFiyati = item.XMLUrun.BronzFiyat;
                                break;
                        }
                    }

                    total += urunFiyati * item.Adet;
                }
                string musteriNumarasi = "135789";
                string musteriSifre = "1234";
                string priceStr = total.ToString().Replace(",", ".");
                string apiUrl = $"https://localhost:44369/api/Pay?musterino={musteriNumarasi}&musterisifre={musteriSifre}&kartno={model.KartNumarasi}&sonkullanmaay={model.Ay}&sonkullanmayil={model.Yıl}&cvv={model.Cvv}&bakiye={priceStr}";

                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.PostAsync(apiUrl, null).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                if (result == "\"101\"")
                {
                    foreach (Sepet item in uyeSepet)
                    {
                        db.Sepetler.Remove(item);
                    }
                    db.SaveChanges();
                    return RedirectToAction("PaymentSuccess", "Odeme");
                }
                else
                {
                    ViewBag.hata = "Ödeme işlemi başarısız oldu, lütfen tekrar deneyin.";
                }
            }
            ViewBag.sepet = uyeSepet;
            return View(model);
        }
           
        
    }
}