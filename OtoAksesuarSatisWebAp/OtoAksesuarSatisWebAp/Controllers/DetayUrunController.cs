using OtoAksesuarSatisWebAp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OtoAksesuarSatisWebAp.Controllers
{
    public class DetayUrunController : Controller
    {
        OtoAksesuarSatisDB db = new OtoAksesuarSatisDB();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "AnaSayfa");
            }
            Urun u = db.Urunler.Find(id);
            if (u == null)
            {
                return RedirectToAction("Index", "AnaSayfa");
            }
            return View(u);
        }
    }
}