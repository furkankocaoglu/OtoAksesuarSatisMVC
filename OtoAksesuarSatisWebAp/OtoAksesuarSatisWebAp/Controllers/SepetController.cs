using OtoAksesuarSatisWebAp.Filters;
using OtoAksesuarSatisWebAp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace OtoAksesuarSatisWebAp.Controllers
{
    [UyeLoginRequiredFilter]
    public class SepetController : Controller
    {
        OtoAksesuarSatisDB db = new OtoAksesuarSatisDB();
        public ActionResult Index()
        {
            int mid = (Session["uye"] as Uye).UyeID;
            return View(db.Sepetler.Where(x => x.UyeID == mid).ToList());
        }
        public ActionResult Add(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "AnaSayfa");
            }
            Urun u = db.Urunler.Find(id);
            if (u == null)
            {
                return RedirectToAction("Index", "AnaSayfa");
            }
            int mid = (Session["uye"] as Uye).UyeID;
            int count = db.Sepetler.Count(x => x.UyeID == mid && x.UrunID == id);
            if (count == 0)
            {
                Sepet c = new Sepet();
                c.UrunID = Convert.ToInt32(id);
                c.UyeID = mid;
                c.Adet = 1;
                db.Sepetler.Add(c);
                db.SaveChanges();
                Session["cartcount"] = Convert.ToInt32(Session["cartcount"]) + 1;
            }
            else
            {
                int cid = db.Sepetler.FirstOrDefault(x => x.UyeID == mid && x.UrunID == id).ID;
                Sepet c = db.Sepetler.Find(cid);
                c.Adet = c.Adet + 1;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Sepet");
        }
        public ActionResult DetailAdd(int? id, int quantity)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "AnaSayfa");
            }

            Urun u = db.Urunler.Find(id);
            if (u == null)
            {
                return RedirectToAction("Index", "AnaSayfa");
            }

            int mid = (Session["uye"] as Uye).UyeID;
            int count = db.Sepetler.Count(x => x.UyeID == mid && x.UrunID == id);
            if (count == 0)
            {
                Sepet c = new Sepet();
                c.UrunID = Convert.ToInt32(id);
                c.UyeID = mid;
                c.Adet = quantity;
                db.Sepetler.Add(c);
                db.SaveChanges();
                Session["cartcount"] = Convert.ToInt32(Session["cartcount"]) + 1;
            }
            else
            {
                int cid = db.Sepetler.FirstOrDefault(x => x.UyeID == mid && x.UrunID == id).ID;
                Sepet c = db.Sepetler.Find(cid);
                c.Adet = c.Adet + quantity;
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Sepet");
        }
        public ActionResult Increase(int? id)
        {
            Sepet c = db.Sepetler.Find(id);
            c.Adet = c.Adet + 1;
            db.SaveChanges();
            return RedirectToAction("Index", "Sepet");
        }
        public ActionResult Decrease(int? id)
        {
            Sepet c = db.Sepetler.Find(id);
            if (c.Adet > 1)
            {
                c.Adet = c.Adet - 1;
            }
            else
            {
                db.Sepetler.Remove(c);
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Sepet");
        }
        public ActionResult Remove(int? id)
        {
            Sepet c = db.Sepetler.Find(id);
            db.Sepetler.Remove(c);
            db.SaveChanges();
            return RedirectToAction("Index", "Sepet");
        }
        public ActionResult xmlAdd(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "AnaSayfa");
            }
            XMLUrun u = db.XMLUrunler.Find(id);
            if (u == null)
            {
                return RedirectToAction("Index", "AnaSayfa");
            }
            int mid = (Session["uye"] as Uye).UyeID;
            int count = db.Sepetler.Count(x => x.UyeID == mid && x.XmlUrunID == id);
            if (count == 0)
            {
                Sepet c = new Sepet();
                c.XmlUrunID = Convert.ToInt32(id);
                c.UyeID = mid;
                c.Adet = 1;
                db.Sepetler.Add(c);
                db.SaveChanges();
                Session["cartcount"] = Convert.ToInt32(Session["cartcount"]) + 1;
            }
            else
            {
                int cid = db.Sepetler.FirstOrDefault(x => x.UyeID == mid && x.XmlUrunID == id).ID;
                Sepet c = db.Sepetler.Find(cid);
                c.Adet = c.Adet + 1;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Sepet");
        }
        public ActionResult xmlDetailAdd(int? id, int quantity)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "AnaSayfa");
            }

            XMLUrun u = db.XMLUrunler.Find(id);
            if (u == null)
            {
                return RedirectToAction("Index", "AnaSayfa");
            }

            int mid = (Session["uye"] as Uye).UyeID;
            int count = db.Sepetler.Count(x => x.UyeID == mid && x.XmlUrunID == id);
            if (count == 0)
            {
                Sepet c = new Sepet();
                c.XmlUrunID = Convert.ToInt32(id);
                c.UyeID = mid;
                c.Adet = quantity;
                db.Sepetler.Add(c);
                db.SaveChanges();
                Session["cartcount"] = Convert.ToInt32(Session["cartcount"]) + 1;
            }
            else
            {
                int cid = db.Sepetler.FirstOrDefault(x => x.UyeID == mid && x.XmlUrunID == id).ID;
                Sepet c = db.Sepetler.Find(cid);
                c.Adet = c.Adet + quantity;
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Sepet");
        }
        public ActionResult xmlIncrease(int? id)
        {
            Sepet c = db.Sepetler.Find(id);
            c.Adet = c.Adet + 1;
            db.SaveChanges();
            return RedirectToAction("Index", "Sepet");
        }
        public ActionResult xmlDecrease(int? id)
        {
            Sepet c = db.Sepetler.Find(id);
            if (c.Adet > 1)
            {
                c.Adet = c.Adet - 1;
            }
            else
            {
                db.Sepetler.Remove(c);
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Sepet");
        }
        public ActionResult xmlRemove(int? id)
        {
            Sepet c = db.Sepetler.Find(id);
            db.Sepetler.Remove(c);
            db.SaveChanges();
            return RedirectToAction("Index", "Sepet");
        }
    }
}