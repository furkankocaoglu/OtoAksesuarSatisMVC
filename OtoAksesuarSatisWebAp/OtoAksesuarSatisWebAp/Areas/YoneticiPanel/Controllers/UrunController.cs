using OtoAksesuarSatisWebAp.Areas.YoneticiPanel.Filters;
using OtoAksesuarSatisWebAp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OtoAksesuarSatisWebAp.Areas.YoneticiPanel.Controllers
{
    [YoneticiLoginRequiredFilter]
    public class UrunController : Controller
    {
        OtoAksesuarSatisDB db = new OtoAksesuarSatisDB();
        public ActionResult Index()
        {
            return View(db.Urunler.Where(x => x.Silinmis == false).ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Kategori_ID = new SelectList(db.Kategoriler.Where(x => !x.Silinmis), "KategoriID", "KategoriAdi");
            ViewBag.Marka_ID = new SelectList(db.Markalar.Where(x => !x.Silinmis), "MarkaID", "MarkaAdi");
            return View();
        }
        [HttpPost]
        public ActionResult Create(Urun Model, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool isvalidimage = true;
                    if (image != null)
                    {
                        FileInfo fi = new FileInfo(image.FileName);
                        string extension = fi.Extension;
                        if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                        {
                            string name = Guid.NewGuid().ToString() + extension;
                            Model.ResimYolu = name;
                            image.SaveAs(Server.MapPath("~/Assets/ProductImages/" + name));
                        }
                        else
                        {
                            isvalidimage = false;
                            ViewBag.mesaj = "Resim uzantısı .jpg, .jpeg, .png olabilir";
                        }
                    }
                    else
                    {
                        Model.ResimYolu = "none.jpg";
                    }
                    if (isvalidimage)
                    {
                        Model.EklenmeTarihi = DateTime.Now;
                        db.Urunler.Add(Model);
                        db.SaveChanges();
                        TempData["mesaj"] = "Ürün ekleme başarılı";
                        return RedirectToAction("Index", "Urun");
                    }
                }
                catch
                {
                    ViewBag.mesaj = "Ürün eklenirken bir hata oluştu";
                }
            }
            ViewBag.Kategori_ID = new SelectList(db.Kategoriler.Where(x => !x.Silinmis), "KategoriID", "KategoriAdi");
            ViewBag.Marka_ID = new SelectList(db.Markalar.Where(x => !x.Silinmis), "MarkaID", "MarkaAdi");
            return View(Model);
        }
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                Urun u = db.Urunler.Find(id);
                if (u != null)
                {
                    if (!u.Silinmis)
                    {
                        ViewBag.Kategori_ID = new SelectList(db.Kategoriler.Where(x => !x.Silinmis), "KategoriID", "KategoriAdi");
                        ViewBag.Marka_ID = new SelectList(db.Markalar.Where(x => !x.Silinmis), "MarkaID", "MarkaAdi");
                        return View(u);
                    }
                    else
                    {
                        TempData["systemerror"] = "Ürün kaldırılmış";
                        return RedirectToAction("Error", "Sistem");
                    }
                }
                else
                {
                    TempData["systemerror"] = "Ürün Bulunamadı";
                    return RedirectToAction("Error", "Sistem");
                }
            }
            else
            {
                return RedirectToAction("Index", "Urun");
            }
        }
        [HttpPost]
        public ActionResult Edit(Urun Model, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool isvalidimage = true;
                    if (image != null)
                    {
                        FileInfo fi = new FileInfo(image.FileName);
                        string extension = fi.Extension;
                        if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                        {
                            string name = Guid.NewGuid().ToString() + extension;
                            Model.ResimYolu = name;
                            image.SaveAs(Server.MapPath("~/Assets/ProductImages/" + name));
                        }
                        else
                        {
                            isvalidimage = false;
                            ViewBag.mesaj = "Resim uzantısı .jpg, .jpeg, .png olabilir";
                        }
                    }
                    if (isvalidimage)
                    {
                        db.Entry(Model).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        TempData["mesaj"] = "Ürün Düzenleme başarılı";
                        return RedirectToAction("Index", "Urun");
                    }
                }
                catch
                {
                    ViewBag.mesaj = "Ürün düzenlenirken bir hata oluştu";
                }
            }
            return View(Model);

        }
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                Urun u = db.Urunler.Find(id);
                if (u != null)
                {
                    u.Silinmis = true;
                    u.AktifMi = false;
                    db.SaveChanges();
                    TempData["mesaj"] = "Ürün silindi";
                    return RedirectToAction("Index", "Urun");
                }
                else
                {
                    TempData["systemerror"] = "Ürün Bulunamadı";
                    return RedirectToAction("Error", "Sistem");
                }
            }
            else
            {
                return RedirectToAction("Index", "Urun");
            }
        }
    }
}