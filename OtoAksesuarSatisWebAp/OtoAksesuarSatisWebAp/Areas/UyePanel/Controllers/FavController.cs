using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OtoAksesuarSatisWebAp.Areas.UyePanel.Controllers
{
    public class FavController : Controller
    {
        // GET: UyePanel/Fav
        public ActionResult RedirectToFavori()
        {
            return RedirectToAction("Index", "Favori");
        }
    }
}