using OtoAksesuarSatisWebAp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OtoAksesuarSatisWebAp.Controllers
{
    public class xmlUrunController : Controller
    {
        OtoAksesuarSatisDB db = new OtoAksesuarSatisDB();

        public ActionResult Index()
        { 
            try
            { 
                var urunler = db.XMLUrunler.ToList();

                if (urunler == null || urunler.Count == 0)
                {
                    TempData["Mesaj"] = "Veritabanında hiç ürün bulunamadı!";
                }
                return View(urunler);
            }
            catch (Exception ex)
            {     
                TempData["Mesaj"] = "Bir hata oluştu: " + ex.Message;
                return View();
            }
        }
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "AnaSayfa");
            }
            XMLUrun u = db.XMLUrunler.Find(id);
            if (u == null)
            {
                return RedirectToAction("Index", "AnaSayfa");
            }
            return View(u);
        }
    }
}