using OtoAksesuarSatisWebAp.Areas.YoneticiPanel.Filters;
using OtoAksesuarSatisWebAp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OtoAksesuarSatisWebAp.Areas.YoneticiPanel.Controllers
{
    [YoneticiLoginRequiredFilter]
    public class UyeUpdateController : Controller
    {
        OtoAksesuarSatisDB db = new OtoAksesuarSatisDB();
        public ActionResult Index()
        {
            var uyeler = db.Uyeler.Where(u => u.Silinmis == false).ToList(); 
            return View(uyeler);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                var uye = db.Uyeler.Find(id);
                if (uye != null && !uye.Silinmis)
                {
                    return View(uye);
                }
            }
            TempData["mesaj"] = "Düzenlenecek üye bulunamadı.";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(Uye model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["mesaj"] = "Üye bilgileri başarıyla güncellendi.";
                    return RedirectToAction("Index","UyeUpdate");
                }
                catch
                {
                    ViewBag.mesaj = "Bir hata oluştu, lütfen tekrar deneyiniz.";
                }
            }
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                var uye = db.Uyeler.Find(id);
                if (uye != null && !uye.Silinmis)
                {
                    uye.Silinmis = true;
                    uye.AktifMi = false;
                    db.SaveChanges();
                    TempData["mesaj"] = "Üye başarıyla silindi.";
                }
            }
            return RedirectToAction("Index", "UyeUpdate");
        }
    }

}