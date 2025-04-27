using OtoAksesuarSatisWebAp.Models;
using OtoAksesuarSatisWebAp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OtoAksesuarSatisWebAp.Controllers
{

    public class UyeController : Controller
    {
        OtoAksesuarSatisDB db = new OtoAksesuarSatisDB();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Uye model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int count = db.Uyeler.Count(x => x.Mail == model.Mail);
                    if (count == 0)
                    {
                        model.KayitTarihi = DateTime.Now;
                        db.Uyeler.Add(model);
                        db.SaveChanges();
                        ViewBag.basarili = "Üyelik işlemi Başarılı";
                    }
                    else
                    {
                        ViewBag.basarisiz = "Bu mail daha önceden kayıt edilmiş";
                    }
                }
                catch
                {
                    ViewBag.basarisiz = "Üyelik işlemi Başarısız";
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(MemberLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Uye u = db.Uyeler.FirstOrDefault(x => x.Mail == model.Eposta && x.Sifre == model.Sifre);
                    if ( u!= null)
                    {
                        if (u.AktifMi)
                        {
                            Session["uye"] = u;                        
                            return RedirectToAction("Index", "AnaSayfa");
                        }
                        else
                        {
                            ViewBag.basarisiz = "Hesabınız askıya alınmıştır";
                        }
                    }
                    else
                    {
                        ViewBag.basarisiz = "Kullanıcı bulunamadı";
                    }
                }
                catch
                {
                    ViewBag.basarisiz = "Bir Hata Oluştu";
                }
            }
            return View(model);
        }
        public ActionResult LogOut()
        {

            Session["uye"] = null;
            return RedirectToAction("Login", "Uye");
        }
    }
}