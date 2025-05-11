using OtoAksesuarSatisWebAp.Areas.YoneticiPanel.Filters;
using OtoAksesuarSatisWebAp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace OtoAksesuarSatisWebAp.Areas.YoneticiPanel.Controllers
{
    [YoneticiLoginRequiredFilter]

    public class HomePanelController : Controller
    {
        OtoAksesuarSatisDB db = new OtoAksesuarSatisDB();

        public ActionResult Index()
        {
            var yonetici = Session["YoneticiSession"] as Yonetici;
            if (yonetici == null)
            {
                return RedirectToAction("Index", "YoneticiGiris");
            }

            ViewBag.Urunler = db.XMLUrunler.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult UrunleriAktar(HttpPostedFileBase[] resimDosyasi)
        {
            var yonetici = Session["YoneticiSession"] as Yonetici;
            if (yonetici == null)
                return RedirectToAction("Index", "YoneticiGiris");

            string xmlDosyaYolu = @"C:\BayilikXML\Urunler.xml";

            if (!System.IO.File.Exists(xmlDosyaYolu))
            {
                TempData["Mesaj"] = "XML dosyası bulunamadı.";
                return RedirectToAction("Index", "HomePanel");
            }

            XDocument xmlDoc = XDocument.Load(xmlDosyaYolu);

            var xmlUrunler = xmlDoc.Descendants("urun").Select(x =>
            {
                string hamResimAdi = x.Element("Resim")?.Value.Trim() ?? "resim_yok.jpg";
                string resimAdi = Path.GetFileName(hamResimAdi);

                return new XMLUrun
                {
                    UrunAdi = x.Element("UrunAdi")?.Value.Trim() ?? "Bilinmeyen",
                    Marka = x.Element("Marka")?.Value.Trim() ?? "Markasız",
                    Kategori = x.Element("Kategori")?.Value.Trim() ?? "Genel",
                    BronzFiyat = decimal.TryParse(x.Element("BronzFiyat")?.Value.Replace(",", "."), out var b) ? b : 0,
                    SilverFiyat = decimal.TryParse(x.Element("SilverFiyat")?.Value.Replace(",", "."), out var s) ? s : 0,
                    GoldFiyat = decimal.TryParse(x.Element("GoldFiyat")?.Value.Replace(",", "."), out var g) ? g : 0,
                    Stok = int.TryParse(x.Element("Stok")?.Value, out var stok) ? stok : 0,
                    Aciklama = x.Element("Aciklama")?.Value.Trim() ?? "Açıklama yok",
                    Resim = "/Images/" + resimAdi,
                    EklenmeZamani = DateTime.TryParse(x.Element("EklenmeZamani")?.Value, out var dt) ? dt : DateTime.Now
                };
            }).ToList();

            int eklenen = 0, guncellenen = 0;

            foreach (var urun in xmlUrunler)
            {
                var mevcut = db.XMLUrunler.FirstOrDefault(x =>
                    x.UrunAdi.ToLower().Trim() == urun.UrunAdi.ToLower().Trim() &&
                    x.Kategori.ToLower().Trim() == urun.Kategori.ToLower().Trim() &&
                    x.Marka.ToLower().Trim() == urun.Marka.ToLower().Trim());

                string kaynakResimYolu = Path.Combine(@"C:\Imagess\", Path.GetFileName(urun.Resim));
                string hedefResimYolu = Server.MapPath("~/Images/" + Path.GetFileName(urun.Resim)); // Proje içindeki Images klasörü

                // Hedef klasör yoksa oluştur
                if (!Directory.Exists(Server.MapPath("~/Images")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Images"));
                }

                // Kaynak dosya varsa ve hedef dosya yoksa kopyala
                if (System.IO.File.Exists(kaynakResimYolu) && !System.IO.File.Exists(hedefResimYolu))
                {
                    try
                    {
                        System.IO.File.Copy(kaynakResimYolu, hedefResimYolu);
                    }
                    catch (Exception ex)
                    {
                        TempData["Mesaj"] = "Resim kopyalama hatası: " + ex.Message;
                    }
                }

                if (mevcut != null)
                {
                    bool degisti = false;
                    if (mevcut.UrunAdi != urun.UrunAdi)
                    {
                        mevcut.UrunAdi = urun.UrunAdi;
                        degisti = true;
                    }

                    if (mevcut.BronzFiyat != urun.BronzFiyat) { mevcut.BronzFiyat = urun.BronzFiyat; degisti = true; }
                    if (mevcut.SilverFiyat != urun.SilverFiyat) { mevcut.SilverFiyat = urun.SilverFiyat; degisti = true; }
                    if (mevcut.GoldFiyat != urun.GoldFiyat) { mevcut.GoldFiyat = urun.GoldFiyat; degisti = true; }
                    if (mevcut.Stok != urun.Stok) { mevcut.Stok = urun.Stok; degisti = true; }
                    if (mevcut.Aciklama != urun.Aciklama) { mevcut.Aciklama = urun.Aciklama; degisti = true; }
                    if (mevcut.Resim != urun.Resim) { mevcut.Resim = urun.Resim; degisti = true; }
                    if (mevcut.EklenmeZamani != urun.EklenmeZamani) { mevcut.EklenmeZamani = urun.EklenmeZamani; degisti = true; }

                    if (degisti)
                        guncellenen++;
                }
                else
                {
                    db.XMLUrunler.Add(urun);
                    eklenen++;
                }
            }

            db.SaveChanges();
            TempData["Mesaj"] = $"{eklenen} ürün eklendi, {guncellenen} ürün güncellendi.";
            return RedirectToAction("Index", "HomePanel");
        }



    }



}