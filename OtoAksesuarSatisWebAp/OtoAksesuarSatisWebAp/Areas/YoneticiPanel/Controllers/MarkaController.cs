using OtoAksesuarSatisWebAp.Areas.YoneticiPanel.Filters;
using OtoAksesuarSatisWebAp.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OtoAksesuarSatisWebAp.Areas.YoneticiPanel.Controllers
{
    [YoneticiLoginRequiredFilter]
    public class MarkaController : Controller
    {
        OtoAksesuarSatisDB db = new OtoAksesuarSatisDB();
        public ActionResult Index()
        {
            return View(db.Markalar.Where(x => x.Silinmis == false).ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Marka model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Markalar.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Marka");
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
                Marka c = db.Markalar.Find(id);
                if (c != null)
                {
                    return View(c);
                }
            }
            return RedirectToAction("Index", "Marka");

        }
        [HttpPost]
        public ActionResult Edit(Marka model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["mesaj"] = "Marka güncelleme başarılı";
                    return RedirectToAction("Index", "Marka");
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
                Marka c = db.Markalar.Find(id);
                if (c != null)
                {
                    c.Silinmis = true;
                    c.Durum = false;
                    db.SaveChanges();
                    TempData["mesaj"] = "Marka silme işlemi başarılı";
                }
            }
            return RedirectToAction("Index", "Marka");
        }
    }
}