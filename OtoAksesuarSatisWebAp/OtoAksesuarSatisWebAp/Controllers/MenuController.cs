using OtoAksesuarSatisWebAp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OtoAksesuarSatisWebAp.Controllers
{
    public class MenuController : Controller
    {
        OtoAksesuarSatisDB db = new OtoAksesuarSatisDB();
        public ActionResult _Index()
        {
            return View(db.Kategoriler.Where(x => x.Silinmis == false).ToList());
        }
    }
}