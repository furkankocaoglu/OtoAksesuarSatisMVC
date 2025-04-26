using OtoSatisWebApp.Models;
using OtoSatisWebApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OtoSatisWebApp.Controllers
{
    public class Default1Controller : Controller
    {
        OtoSatis_DB db = new OtoSatis_DB();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string altBayiID, string segment)
        {
            int bayiIDInt;
            bool isValidBayiID = int.TryParse(altBayiID, out bayiIDInt);

            if (!isValidBayiID)
            {
                ViewBag.Hata = "Geçersiz Alt Bayi ID formatı";
                return View();
            }

            var bayi = db.AltBayilerr
                           .FirstOrDefault(b => b.AltBayiID == bayiIDInt && b.Segment == segment);

            if (bayi != null)
            {
                
                return RedirectToAction("Indexx", "Default");
            }
            else
            {

                ViewBag.Hata = "Geçersiz Alt Bayi ID veya Segment";
                return View();
            }
        }
        
    }
       
    

        
}