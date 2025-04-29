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
    public class SiparisController : Controller
    {
        OtoAksesuarSatisDB db= new OtoAksesuarSatisDB();
        public ActionResult Index()
        {
            var uye = Session["uye"] as Uye;
            if (uye == null)
            {
                return RedirectToAction("Login", "Uye");
            }

            var siparisler = db.Siparisler
                               .Where(s => s.UyeID == uye.UyeID && s.Silinmis == false)
                               .ToList();

            var yorumYapilmisUrunler = db.Yorumlar
                                         .Where(y => y.UyeID == uye.UyeID && y.Silinmis == false)
                                         .Select(y => y.UrunID)
                                         .ToList();

            ViewBag.YorumYapilmisUrunler = yorumYapilmisUrunler;

            return View(siparisler);
        }
        public ActionResult ToplamSiparis()
        {
            var uye = Session["uye"] as Uye;
            if (uye == null)
            {
                return RedirectToAction("Login", "Uye");
            }

            int siparisSayisi = db.Siparisler.Count(s => s.UyeID == uye.UyeID && s.Silinmis == false);

            ViewBag.SiparisSayisi = siparisSayisi;

            return View(siparisSayisi);
        }
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                Siparis c = db.Siparisler.Find(id);
                if (c != null)
                {
                    c.Silinmis = true;
                    db.SaveChanges();
                    TempData["mesaj"] = "Sipariş kaldırıldı";
                }
            }
            return RedirectToAction("Index", "AnaSayfa");
        }
    }
}