using OtoAksesuarSatisWebAp.Filters;
using OtoAksesuarSatisWebAp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OtoAksesuarSatisWebAp.Areas.UyePanel.Controllers
{
    [UyeLoginRequiredFilter]
    public class UyeController : Controller
    {
        OtoAksesuarSatisDB db= new OtoAksesuarSatisDB();
        public ActionResult Profil()
        {
            var uye = Session["uye"] as Uye;
            if (uye == null)
            {
                return RedirectToAction("Login", "Uye");
            }

            return View(uye); 
        }
        public ActionResult UyeLogout()
        {
            Session["uye"] = null;
            return RedirectToAction("Login", "Uye", new { area = "" });
        }
    }
}
