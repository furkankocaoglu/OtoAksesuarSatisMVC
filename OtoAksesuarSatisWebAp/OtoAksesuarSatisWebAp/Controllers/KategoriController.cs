using OtoAksesuarSatisWebAp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OtoAksesuarSatisWebAp.Controllers
{
    public class KategoriController : Controller
    {
        OtoAksesuarSatisDB db= new OtoAksesuarSatisDB();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetCategories()
        {
            var kategoriler = db.Kategoriler.Where(k => k.Silinmis == false).ToList();  
            return PartialView(kategoriler); 
        }
        public ActionResult Urunler(int? kategoriId)
        {
            if (kategoriId == null)
            {
                TempData["info"] = "Geçersiz kategori seçimi.";
                return RedirectToAction("Index","AnaSayfa");
            }

            var urunler = db.Urunler
                .Where(u => u.KategoriID == kategoriId && u.Silinmis == false)
                .ToList();

            return View(urunler);
        }
    }
}