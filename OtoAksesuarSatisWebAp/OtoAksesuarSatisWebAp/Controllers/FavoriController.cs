using OtoAksesuarSatisWebAp.Filters;
using OtoAksesuarSatisWebAp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OtoAksesuarSatisWebAp.Controllers
{
    [UyeLoginRequiredFilter]
    public class FavoriController : Controller
    {
        OtoAksesuarSatisDB db = new OtoAksesuarSatisDB();
        public ActionResult Index()
        {
            Uye u = Session["uye"] as Uye;
            return View(db.Favoriler.Where(x => x.UyeID == u.UyeID).ToList());
        }
        public ActionResult Add(int? id )
        {
            if (Session["uye"] != null)
            {
                if (id != null)
                {
                    int count = db.Urunler.Count(x => x.UrunID == id);
                    if (count > 0)
                    {
                        int mid = (Session["uye"] as Uye).UyeID;
                        int c2 = db.Favoriler.Count(x => x.UyeID == mid && x.UrunID == id);
                        if (c2 == 0)
                        {
                            Favori f = new Favori();
                            f.UyeID = (Session["uye"] as Uye).UyeID;
                            f.UrunID = Convert.ToInt32(id);
                            db.Favoriler.Add(f);
                            db.SaveChanges();
                            TempData["info"] = "Favorilere Eklendi";
                        }
                        else
                        {
                            TempData["info"] = "Favorilerinize Zaten Ekli";
                        }

                    }
                }
            }
            else
            {
                TempData["info"] = "Favorilere Eklemek İçin Giriş Yapınız";
                return RedirectToAction("Login", "Uye");
            }
            return RedirectToAction("Index", "AnaSayfa");
        }

        public ActionResult Remove(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Favorite");
            }

            Favori favorite = db.Favoriler.Find(id);
            db.Favoriler.Remove(favorite);
            db.SaveChanges();
            TempData["info"] = "Favorilerinizden çıkarıldı";
            return RedirectToAction("Index", "Favori");
        }
        

        
    }
}