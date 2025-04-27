using OtoAksesuarSatisWebAp.Areas.YoneticiPanel.Data;
using OtoAksesuarSatisWebAp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                        return RedirectToAction("Index", "HomePanel");
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