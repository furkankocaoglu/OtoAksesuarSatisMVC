using OtoAksesuarSatisWebAp.Filters;
using OtoAksesuarSatisWebAp.Models;
using OtoAksesuarSatisWebAp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OtoAksesuarSatisWebAp.Controllers
{
    [UyeLoginRequiredFilter]
    public class OdemeController : Controller
    {
        OtoAksesuarSatisDB db = new OtoAksesuarSatisDB();
        public ActionResult PaymentSuccess()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Payment()
        {
            int mid = (Session["uye"] as Uye).UyeID;
            ViewBag.cart = db.Sepetler.Where(x => x.UyeID == mid).ToList();
            return View();

        }
        [HttpPost]
        public ActionResult Payment(OdemeViewModel model)
        {
            int mid = (Session["uye"] as Uye).UyeID;
            List<Sepet> memberCart = db.Sepetler.Where(x => x.UyeID == mid).ToList();
            return View();
        }
    }
}