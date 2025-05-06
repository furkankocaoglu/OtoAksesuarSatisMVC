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
    public class KategoriController : Controller
    {
        OtoAksesuarSatisDB db = new OtoAksesuarSatisDB();
       
        public ActionResult Index()
        {
            return View(db.Kategoriler.Where(x => x.Silinmis == false).ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult _Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Kategori model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Kategoriler.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Kategori");

                }
                catch
                {
                    ViewBag.mesaj = "Bir hata oluştu";
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                Kategori c = db.Kategoriler.Find(id);
                if (c != null)
                {
                    return View(c);
                }
            }
            return RedirectToAction("Index", "Kategori");

        }
        [HttpPost]
        public ActionResult Edit(Kategori model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["mesaj"] = "Kategori güncelleme başarılı";
                    return RedirectToAction("Index", "Kategori");
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
            if (id != null)
            {
                Kategori c = db.Kategoriler.Find(id);
                if (c != null)
                {
                    c.Silinmis = true;
                    c.Durum = false;
                    db.SaveChanges();
                    TempData["mesaj"] = "Kategori silme işlemi başarılı";
                }
            }
            return RedirectToAction("Index", "Kategori");
        }
    }
}