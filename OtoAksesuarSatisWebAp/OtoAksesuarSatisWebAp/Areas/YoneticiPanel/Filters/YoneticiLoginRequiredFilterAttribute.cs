using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Filters;
using System.Web.Mvc;
using OtoAksesuarSatisWebAp.Models;

namespace OtoAksesuarSatisWebAp.Areas.YoneticiPanel.Filters
{
    public class YoneticiLoginRequiredFilterAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        OtoAksesuarSatisDB db = new OtoAksesuarSatisDB();
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var managerSession = filterContext.HttpContext.Session["YoneticiSession"];

            if (managerSession == null)
            {
               
                filterContext.Result = new RedirectResult("~/YoneticiPanel/YoneticiGiris/Index");
            }
            else
            {
                Yonetici m = managerSession as Yonetici;
                if (m != null && !m.AktifMi)
                {
                    
                    filterContext.Result = new RedirectResult("~/ManagerPanel/YoneticiGiris/Index");
                }
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectResult("~/ManagerPanel/YoneticiGiris/Index");
            }
        }
    }
}