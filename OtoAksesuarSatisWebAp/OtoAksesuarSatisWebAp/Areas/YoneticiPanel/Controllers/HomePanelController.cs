using OtoAksesuarSatisWebAp.Areas.YoneticiPanel.Filters;
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
            return View();
        }
    }
}