using OtoSatisWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OtoSatisWebApp.Controllers
{
    public class DefaultController : Controller
    {
        private OtoSatis_DB db = new OtoSatis_DB();
        public ActionResult Index()
        {
            var altBayiler = db.AltBayilerr.ToList();
            return View(altBayiler);
        }
    }
}