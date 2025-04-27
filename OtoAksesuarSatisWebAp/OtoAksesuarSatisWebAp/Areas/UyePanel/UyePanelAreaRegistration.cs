using System.Web.Mvc;

namespace OtoAksesuarSatisWebAp.Areas.UyePanel
{
    public class UyePanelAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "UyePanel";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "UyePanel_default",
                "UyePanel/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}