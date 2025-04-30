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
    public class HomePanelController : Controller
    {
        
        public ActionResult Index()
        {
           
            var yonetici = Session["YoneticiSession"] as Yonetici;

            if (yonetici == null)
            {
                return RedirectToAction("Index", "YoneticiGiris");
            }

            var urunler = Session["Urunler"] as List<Urun>;

            if (urunler != null)
            {
                ViewBag.Urunler = urunler;
            }
            else
            {
                ViewBag.Urunler = "XML dosyasına ulaşılamadı.";
            }

            return View();
        }
    
    }

    
    
}