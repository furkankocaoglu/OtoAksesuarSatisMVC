using OtoAksesuarSatisWebAp.Filters;
using OtoAksesuarSatisWebAp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OtoAksesuarSatisWebAp.Controllers
{
    public class AnaSayfaController : Controller
    {
        OtoAksesuarSatisDB db = new OtoAksesuarSatisDB();
        public ActionResult Index()
        {
            var urunler = db.Urunler.Where(x => x.Silinmis == false && x.AktifMi == true).ToList();

           
            return View(urunler);
        }

        [UyeLoginRequiredFilter]
        public ActionResult _GetCartCount()
        {
            int mid = (Session["uye"] as Uye).UyeID;
            int count = db.Sepetler.Count(x => x.UyeID == mid);
            ViewBag.count = count;
            return View();
        }
    }
}