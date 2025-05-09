using OtoAksesuarSatisWebAp.Filters;
using OtoAksesuarSatisWebAp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OtoAksesuarSatisWebAp.Areas.UyePanel.Controllers
{
    [UyeLoginRequiredFilter]
    public class YorumController : Controller
    {
        OtoAksesuarSatisDB db = new OtoAksesuarSatisDB();
        public ActionResult Index()
        {
            var uye = Session["uye"] as Uye;
            if (uye == null)
            {
                return RedirectToAction("Login", "Uye");
            }

            
            var siparisler = db.Siparisler
                                .Where(s => s.UyeID == uye.UyeID && s.Silinmis == false)
                                .ToList();

          
            var siparisliUrunIds = siparisler.Select(s => s.UrunID).ToList();

            
            var yorumlar = db.Yorumlar.Where(x => x.UyeID == uye.UyeID && x.Silinmis == false).ToList();

           
            ViewBag.SiparisliUrunIds = siparisliUrunIds;

           
            var urunler = db.Urunler.ToList(); 

            
            ViewBag.Urunler = urunler;

            return View(yorumlar);
        }
        [HttpGet]
        public ActionResult Create(int? urunId)
        {
            var uye = Session["uye"] as Uye;
            if (uye == null)
            {
                return RedirectToAction("Login", "Uye");
            }

            if (urunId == null)
            {
                return RedirectToAction("Index", "Siparis");
            }

            var satinAlindiMi = db.Siparisler
                                  .Any(s => s.UrunID == urunId && s.UyeID == uye.UyeID && s.Silinmis == false);

            if (!satinAlindiMi)
            {
                TempData["mesaj"] = "Bu ürünü satın almadığınız için yorum yapamazsınız.";
                return RedirectToAction("Index", "Siparis");
            }

            var dahaOnceYorumYapildiMi = db.Yorumlar
                                           .Any(y => y.UrunID == urunId && y.UyeID == uye.UyeID && y.Silinmis == false);

            if (dahaOnceYorumYapildiMi)
            {
                TempData["mesaj"] = "Bu ürüne zaten yorum yaptınız.";
                return RedirectToAction("Index", "Siparis");
            }

            var yorum = new Yorum
            {
                UrunID = urunId.Value,
                UyeID = uye.UyeID
            };

            return View(yorum);
        }
        [HttpPost]
        public ActionResult Create(Yorum model)
        {
            if (ModelState.IsValid)
            {
                var uye = Session["uye"] as Uye;
                model.UyeID = uye.UyeID;
                model.YorumTarihi = DateTime.Now;
                model.Durum = true;
                model.Silinmis = false;

                db.Yorumlar.Add(model);
                db.SaveChanges();
                TempData["mesaj"] = "Yorumunuz başarıyla eklendi.";
                return RedirectToAction("Index", "Yorum");
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var uye = Session["uye"] as Uye;

            if (id != null)
            {
                Yorum y = db.Yorumlar.Find(id);
                if (y != null && y.UyeID == uye.UyeID)
                {
                    return View(y);
                }
            }
            return RedirectToAction("Index", "Yorum");

        }
        [HttpPost]
        public ActionResult Edit(Yorum model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["mesaj"] = "Yorum güncelleme başarılı";
                    return RedirectToAction("Index", "Yorum");
                }
                catch
                {
                    ViewBag.mesaj = "Bir hata oluştu";
                }
            }
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            var uye = Session["uye"] as Uye;

            if (id != null)
            {
                Yorum y = db.Yorumlar.Find(id);
                if (y != null && y.UyeID == uye.UyeID)
                {
                    y.Silinmis = true;
                    y.Durum = false;
                    db.SaveChanges();
                    TempData["mesaj"] = "Yorum silindi.";
                }
            }
            return RedirectToAction("Index", "Yorum");
        }
    }
}