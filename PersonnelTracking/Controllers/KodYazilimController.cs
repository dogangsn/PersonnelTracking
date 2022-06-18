//DoganGunes
using Kod_Yazilim_Personel.App_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PagedList;
using Kod_Yazilim_Personel.ViewModel;
using System.Web.Script.Serialization;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using Kod_Yazilim_Personel.Models;
using System.Data.Entity.Infrastructure;
using Kod_Yazilim_Personel.MobileModel;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using SednaPersonel.Models.Models.Entity;

namespace Kod_Yazilim_Personel.Controllers
{
    public class KodYazilimController : Controller
    {

        KodPersonelDbEntities db = new KodPersonelDbEntities();

        private App_Data.Ayarlar cikisSaati;
        private App_Data.Ayarlar girisSaati;

        public KodYazilimController()
        {
            cikisSaati = db.Ayarlar.Where(s => s.id == 4).FirstOrDefault();
            girisSaati = db.Ayarlar.Where(s => s.id == 3).FirstOrDefault();
        }
        // GET: KodYazilim
        public ActionResult Index(int sayfa = 1)
        {

            if (Session["uyeOturum"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            if (Session["uyeTuru"].ToString() != "1" && Session["uyeTuru"].ToString() != "0")
            {
                Session.Abandon();
                return RedirectToAction("Index", "User");
            }
            DateTime d = Convert.ToDateTime(DateTime.Now);
            int gun = d.Day;
            int ay = d.Month;
            int yil = d.Year;
            List<App_Data.Hareketler> kayitListe = db.Hareketler.OrderByDescending(s => s.Id).Where(s => s.Tarih.Value.Day == gun && s.Tarih.Value.Month == ay && s.Tarih.Value.Year == yil).ToList();
            return View(kayitListe);

        }

        [HttpGet]
        public ActionResult KayitEkle(string KartID)
        {
            var kp = db.KayitliPersonel.Where(k => k.KartID == (KartID)).Select(r => new { r.KartID, r.KId , r.PAdSoyad}).SingleOrDefault();
            if (kp != null)
            {
                int gun = DateTime.Now.Day;
                int ay = DateTime.Now.Month;
                int yil = DateTime.Now.Year;
                string adi = "";
                int kayitSay = db.Hareketler.Where(s => s.Tarih.Value.Day == gun && s.Tarih.Value.Month == ay && s.Tarih.Value.Year == yil && s.KayitId == kp.KId).Count();
                string islem = "";
                int operation = 0;
                if (kayitSay == 0)
                {
                    islem = "Giris";
                    operation = 0;
                }
                else
                {
                    if (kayitSay % 2 == 0)
                    {
                        islem = "Giris";
                        operation = 0;
                    }
                    else
                    {
                        islem = "Cikis";
                        operation = 1;
                    }
                }
                App_Data.Hareketler prs = new App_Data.Hareketler();
                prs.KayitId = kp.KId;
                prs.Tarih = DateTime.Now;
                prs.Islem = islem;
                prs.operation = operation;
                adi = kp.PAdSoyad;
                db.Hareketler.Add(prs);
                db.SaveChanges();
                



                return Json(adi.Substring(0, 1) + "." + adi.Substring(adi.IndexOf(" ") + 1, 1) + " " + islem + " Yapti  ", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Tanimsiz Kart  ", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult FingerKayit(int FingerId)
        {
            var kp = db.KayitliPersonel.Where(k => k.FingerID == (FingerId)).Select(r => new { r.KartID, r.KId, r.PAdSoyad }).SingleOrDefault();
            if (kp != null)
            {
                int gun = DateTime.Now.Day;
                int ay = DateTime.Now.Month;
                int yil = DateTime.Now.Year;
                string adi = "";
                int kayitSay = db.Hareketler.Where(s => s.Tarih.Value.Day == gun && s.Tarih.Value.Month == ay && s.Tarih.Value.Year == yil && s.KayitId == kp.KId).Count();
                string islem = "";
                int operation = 0;
                if (kayitSay == 0)
                {
                    islem = "Giris";
                    operation = 0;
                }
                else
                {
                    if (kayitSay % 2 == 0)
                    {
                        islem = "Giris";
                        operation = 0;
                    }
                    else
                    {
                        islem = "Cikis";
                        operation = 1;
                    }
                }
                App_Data.Hareketler prs = new App_Data.Hareketler();
                prs.KayitId = kp.KId;
                prs.Tarih = DateTime.Now;
                prs.Islem = islem;
                prs.operation = operation;
                adi = kp.PAdSoyad;
                db.Hareketler.Add(prs);
                db.SaveChanges();
                return Json(adi.Substring(0, 1) + "." + adi.Substring(adi.IndexOf(" ") + 1, 1) + " " + islem + " Yapti  ", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Tanimsiz Kart  ", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult PersonelKayit(string KartId)
        {
            Boolean islem;
            var kp = db.KayitliPersonel.Where(k => k.KartID == (KartId)).Select(r => new { r.KartID, r.KId, r.PAdSoyad }).SingleOrDefault();
            if (kp == null)
            {
                App_Data.KayitliPersonel newPrs = new App_Data.KayitliPersonel();
                newPrs.KartID = KartId;
                //newPrs.PAdSoyad = kp.PAdSoyad;
                db.KayitliPersonel.Add(newPrs);
                db.SaveChanges();
                islem = true;
            }
            else
            {
                islem = false;
                return Json("Sistemde Tanimli  ", JsonRequestBehavior.AllowGet);
            }
            return Json(" Kayit Edildi   ", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ManuekPersonelKayit(string KartId, string PAdSoyad)
        {
            QRCodeEngine engine = new QRCodeEngine();
            Boolean islem;
            var kp = db.KayitliPersonel.Where(k => k.KartID == (KartId)).Select(r => new { r.KartID, r.KId, r.PAdSoyad }).SingleOrDefault();
            if (kp == null)
            {
                App_Data.KayitliPersonel newPrs = new App_Data.KayitliPersonel();
                newPrs.KartID = KartId;
                newPrs.PAdSoyad = PAdSoyad;
                newPrs.QRPath = engine.generateQRCode(KartId);
                db.KayitliPersonel.Add(newPrs);
                db.SaveChanges();
                islem = true;
            }
            else
            {
                islem = false;
                return Json("Sistemde Tanimli  ", JsonRequestBehavior.AllowGet);
            }
            return Json(" Kayit Edildi   ", JsonRequestBehavior.AllowGet);
        }

        public ActionResult PersonelListe()
        {
            if (Session["uyeOturum"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            if (Session["uyeTuru"].ToString() != "1" && Session["uyeTuru"].ToString() != "0")
            {
                Session.Abandon();
                return RedirectToAction("Index", "User");
            }
            List<App_Data.KayitliPersonel> personeller = db.KayitliPersonel.Where(s => s.Silindi == false || s.Silindi == null).ToList();
            return View(personeller);
        }

        public ActionResult PersonelSil(int? id)
        {
            //KullanılmıyorSonraKullanılabilir
            App_Data.KayitliPersonel kayit = db.KayitliPersonel.Where(k => k.KId == id).FirstOrDefault();
            db.KayitliPersonel.Remove(kayit);
            //db.SaveChanges();
            ViewBag.sonuc = "Kayıt Sİlindi.";
            return Json(kayit, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GirisCikisDetaySil(int? id)
        {
            App_Data.Hareketler kayit = db.Hareketler.Where(k => k.Id == id).FirstOrDefault();
            db.Hareketler.Remove(kayit);
            db.SaveChanges();
            ViewBag.sonuc = "Kayıt Sİlindi.";
            return Json(kayit, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteSelected(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                ModelState.AddModelError("", "Silmek için hiçbir öğe seçilmedi");
                return RedirectToAction("Index");
            }
            List<int> TaskIds = ids.Select(x => Int32.Parse(x)).ToList();
            for (var i = 0; i < TaskIds.Count(); i++)
            {
                var todo = db.Hareketler.Find(TaskIds[i]);
                db.Hareketler.Remove(todo);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public JsonResult GetPersonelEdit(int PersonelId)
        {
            var model = db.KayitliPersonel.Where(x => x.KId == PersonelId).Select(s => new {
                FingerID = s.FingerID,
                EPosta = s.EPosta,
                DogumTarihi = s.DogumTarihi,
                PAdSoyad = s.PAdSoyad,
                Silindi = s.Silindi,
                KartID = s.KartID
            }).SingleOrDefault();

            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDateFilitre(string date) // GirisCikisHareketleri
        {
            DateTime d = Convert.ToDateTime(date);
            int gun = d.Day;
            int ay = d.Month;
            int yil = d.Year;
            List<HareketModel> kayitListe = db.Hareketler.Where(s => s.Tarih.Value.Day == gun && s.Tarih.Value.Month == ay && s.Tarih.Value.Year == yil).Select(x => new HareketModel()
            {
                Id = x.Id,
                Islem = x.Islem,
                KartID = x.KayitliPersonel.KartID,
                PAdSoyad = x.KayitliPersonel.PAdSoyad,
                //Tarih = (DateTime)x.Tarih,
                gun = x.Tarih.Value.Day,
                ay = x.Tarih.Value.Month,
                yil = x.Tarih.Value.Year,
                saat = x.Tarih.Value.Hour,
                dakika = x.Tarih.Value.Minute
            }).ToList();
            return Json(kayitListe, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDateGirisCikisFilitre(string date)
        {
            DateTime d = Convert.ToDateTime(date);
            int gun = d.Day;
            int ay = d.Month;
            int yil = d.Year;
            var personel = db.KayitliPersonel
                             .Where(s => (s.Silindi == false || s.Silindi == null) && s.KId != 89 && s.KId != 107)
                             .Select(r => new
                             {
                                 r.KId,
                                 r.Adres,
                                 r.PAdSoyad,
                                 r.KartID,
                                 r.CepNum,
                                 r.EPosta,
                                 r.DogumTarihi,
                                 Izinler = r.Izinler.Where(s => s.BaslangicTarihi <= d && s.BitisTarihi >= d).ToList(),
                                 Hareketler = r.Hareketler.Where(s => s.Tarih.Value.Day == gun && s.Tarih.Value.Month == ay && s.Tarih.Value.Year == yil).OrderBy(x=>x.Tarih).ToList(),
                                 Notlar = r.Notlar.Where(s => s.Tarih == d).ToList()
                             }).ToList();
            //var kytListe = db.KayitliPersonel.Distinct().Where(s => s.Silindi == false || s.Silindi == null).ToList();


            var results = new List<KayitliPersonelModel>();


            foreach (var item in personel)
            {
                string islem = "";
                var Izin = item.Izinler.Where(k => k.BaslangicTarihi <= d && k.BitisTarihi >= d && k.IzinTuru == "YILLIK İZİN" && k.KId == item.KId).ToList();
                if (Izin.Count > 0)
                {
                    islem = item.Izinler.FirstOrDefault().IzinTuru;
                }
                var it = new KayitliPersonelModel
                {

                    Adres = item.Adres,
                    PAdSoyad = item.PAdSoyad,
                    KartID = item.KartID,
                    CepNum = item.CepNum,
                    EPosta = item.EPosta,
                    DogumTarihi = item.DogumTarihi,
                    KId = item.KId,
                    Islem = islem
                };

                //item.Hareketler = item.Hareketler.Where(s => s.Tarih.Value.Day == gun && s.Tarih.Value.Month == ay && s.Tarih.Value.Year == yil).ToList();
                //item.Hareketler = item.Hareketler.ToList();
                //item.Hareketler = item.Hareketler.Take(item.Hareketler.Count).ToList();
                var notlar = item.Notlar.Where(s => s.Tarih.Value.Day == gun && s.Tarih.Value.Month == ay && s.Tarih.Value.Year == yil).ToList();
                it.Hareketler = item.Hareketler.Select(x => new HareketModel()
                {
                    Id = x.Id,
                    Islem = x.Islem,
                    Tarih = (DateTime)x.Tarih,
                    Operation = x.operation
                }).ToList();
                double totalWorkTime = 0;
                double tatalNotWorkedTime = 0;
                GetDateCalculater(item.Hareketler.ToList(), out totalWorkTime, out tatalNotWorkedTime, date);
                it.TotalCalisma = Convert.ToString(TimeSpan.FromMinutes(totalWorkTime).ToString(@"hh\s\:mm\d\k"));
                it.TotalMola = Convert.ToString(TimeSpan.FromMinutes(tatalNotWorkedTime).ToString(@"hh\s\:mm\d\k"));
                if (notlar.Any())
                {
                    it.Note = notlar.LastOrDefault().Not;
                }
                results.Add(it);
            }
            results = results.OrderBy(s => s.PAdSoyad).ToList();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(results, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveDataInDatabase(App_Data.KayitliPersonel model)
        {
            var result = false;
            try
            {
                if (model.KId > 0)
                {
                    App_Data.KayitliPersonel Stu = db.KayitliPersonel.SingleOrDefault(x => x.KId == model.KId);
                    Stu.KartID = model.KartID;
                    Stu.PAdSoyad = model.PAdSoyad;
                    Stu.FingerID = model.FingerID;
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    App_Data.KayitliPersonel Stu = new App_Data.KayitliPersonel();
                    Stu.KartID = model.KartID;
                    Stu.PAdSoyad = model.PAdSoyad;
                    Stu.FingerID = model.FingerID;
                    db.KayitliPersonel.Add(Stu);
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPartialData(int KayitId)
        {
            var model = db.KayitliPersonel.Where(x => x.KId == KayitId).Select(s => new {
                PAdSoyad = s.PAdSoyad,
                DogumTarihi = s.DogumTarihi,
                EPosta = s.EPosta,
                CepNum = s.CepNum,
                IseBaslamaTarihi = s.IseBaslamaTarihi,
                Adres = s.Adres,
                FotoYol = s.FotoYol

            }).SingleOrDefault();
            Session["FotoYol"] = model.FotoYol;
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PersonelGirisCikis()
        {
            if (Session["uyeOturum"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            DateTime d = Convert.ToDateTime(DateTime.Now);
            int gun = d.Day;
            int ay = d.Month;
            int yil = d.Year;
            //var kytListe = db.KayitliPersonel.Distinct().Where(s => s.Silindi == false || s.Silindi == null).ToList();
            //x => x.KId != 89 && x.KId != 107
            var personel = db.KayitliPersonel
                       .Where(s => (s.Silindi == false || s.Silindi == null) && s.KId != 89 && s.KId != 107)
                       .Select(r => new
                       {
                           r.KId,
                           r.Adres,
                           r.PAdSoyad,
                           r.KartID,
                           r.CepNum,
                           r.EPosta,
                           r.DogumTarihi,
                           Izinler = r.Izinler.Where(s => s.BaslangicTarihi <= d && s.BitisTarihi >= d).ToList(),
                           Hareketler = r.Hareketler.Where(s => s.Tarih.Value.Day == gun && s.Tarih.Value.Month == ay && s.Tarih.Value.Year == yil).OrderBy(x=>x.Tarih).ToList(),
                           Notlar = r.Notlar.Where(s => s.Tarih.Value.Day == gun && s.Tarih.Value.Month == ay && s.Tarih.Value.Year == yil).OrderBy(x=>x.Tarih).ToList()
                       }).ToList();

            var results = new List<KayitliPersonelModel>();

            foreach (var item in personel)
            {
                string islem = "";
                var hareket = item.Hareketler.Where(k => k.Tarih.Value.Day == gun && k.Tarih.Value.Month == ay && k.Tarih.Value.Year == yil && k.KayitId == item.KId).ToList();
                var itemIzinler = item.Izinler.Where(k => k.BitisTarihi >= d && k.KId == item.KId).ToList();

                if (hareket == null)
                {
                    if (itemIzinler.Count > 0)
                    {
                        islem = item.Izinler.OrderByDescending(s => s.id).FirstOrDefault().IzinTuru;
                    }
                    else
                    {
                        islem = "GELMEDİ";
                    }

                }
                if (hareket.Count == 0)
                {
                    if (itemIzinler.Count > 0)
                    {
                        islem = item.Izinler.OrderByDescending(s => s.id).FirstOrDefault().IzinTuru;
                    }
                    else
                    {
                        islem = "GELMEDİ";
                    }
                }
                try
                {
                    int? durum = hareket.OrderByDescending(p => p.Tarih).ToList().FirstOrDefault().operation;
                    Boolean? tip = hareket.OrderByDescending(p => p.Tarih).ToList().FirstOrDefault().Tipi;
                    Boolean? SehirDisi = hareket.OrderByDescending(p => p.Tarih).ToList().FirstOrDefault().SehirDisi;
                    if ((durum ?? 1) == 0)
                    {
                        if (SehirDisi == true)
                        {
                            islem = "ŞEHİR DIŞI - MÜŞTERİ";
                        }
                        else
                        {
                            islem = "İÇERDE";
                        }
                    }
                    else if ((durum ?? 0) == 1 && tip == true)
                    {
                        islem = "MÜŞTERİDE";
                    }
                    else
                    {
                        if (itemIzinler.Count > 0)
                        {
                            islem = item.Izinler.OrderByDescending(s => s.id).FirstOrDefault().IzinTuru;
                        }
                        else
                        {
                            islem = "DIŞARDA";
                        }

                    }
                }
                catch
                {
                    //islem = "DIŞARDA";
                }
                var it = new KayitliPersonelModel
                {
                    Adres = item.Adres,
                    PAdSoyad = item.PAdSoyad,
                    KartID = item.KartID,
                    CepNum = item.CepNum,
                    EPosta = item.EPosta,
                    DogumTarihi = item.DogumTarihi,
                    KId = item.KId,
                    Islem = islem
                    
                };
                //item.Hareketler = item.Hareketler.Where(s => s.Tarih.Value.Day == gun && s.Tarih.Value.Month == ay && s.Tarih.Value.Year == yil).ToList();
                var notlar = item.Notlar.Where(s => s.Tarih.Value.Day == gun && s.Tarih.Value.Month == ay && s.Tarih.Value.Year == yil).ToList();
                it.Hareketler = item.Hareketler.Select(x => new HareketModel()
                {
                    Id = x.Id,
                    Islem = x.Islem,
                    Tarih = (DateTime)x.Tarih,
                    Operation = x.operation,
                    SehirDisi = x.SehirDisi,
                    MusteriBİlgisi = x.MusteriBİlgisi
                }).ToList();
                double totalWorkTime = 0;
                double tatalNotWorkedTime = 0;
                CalculateWorkTime(item.Hareketler.ToList(), out totalWorkTime, out tatalNotWorkedTime);
                it.TotalCalisma = Convert.ToString(TimeSpan.FromMinutes(totalWorkTime).ToString(@"hh\s\:mm\d\k"));
                it.TotalMola = Convert.ToString(TimeSpan.FromMinutes(tatalNotWorkedTime).ToString(@"hh\s\:mm\d\k"));
                if (notlar.Any())
                {
                    it.Note = notlar.LastOrDefault().Not;
                }
                if (it.Hareketler.Any())
                {
                    it.MusteriBİlgisi = it.Hareketler.LastOrDefault().MusteriBİlgisi;
                }
                results.Add(it);
                results = results.OrderBy(s => s.PAdSoyad).ToList();
            }
            return View(results);
        }

        void CalculateWorkTime(List<Hareketler> hareket, out double totalWorkTime, out double tatalNotWorkedTime)
        {
            //var girisSaati = db.Ayarlar.Where(s => s.id == 3).FirstOrDefault();
            int BSaat = girisSaati.Saats.Value.Hours;
            int Bdakika = girisSaati.Saats.Value.Minutes;
            int Bsaniye = girisSaati.Saats.Value.Seconds;
            var kayitListe = hareket.Select(x => new HareketModel()
            {
                Id = x.Id,
                Islem = x.Islem,
                Tarih = (DateTime)x.Tarih,
                Operation = x.operation,
                Tipi = x.Tipi

            }).ToList();

            var lastRecord = kayitListe.LastOrDefault();
            var firstRecord = kayitListe.FirstOrDefault();

            if (firstRecord != null && firstRecord.Operation == 0)
            {
                DateTime normalTime = firstRecord.Tarih;
                TimeSpan ts = new TimeSpan(BSaat, Bdakika, Bsaniye);

                normalTime = normalTime.Date + ts;
                if (firstRecord.Tarih > normalTime)
                {
                    var s = (firstRecord.Tarih - normalTime).TotalMinutes;
                    firstRecord.TotalMinute = s;
                }
            }
            if (lastRecord != null && lastRecord.Operation == 0)
            {
                kayitListe.Add(new HareketModel
                {
                    Id = 0,
                    Islem = "Şuan",
                    KartID = lastRecord.KartID,
                    PAdSoyad = lastRecord.PAdSoyad,
                    Tarih = DateTime.Now,
                    Operation = 1
                });
            }
            if (lastRecord != null && lastRecord.Operation == 1 && lastRecord.Tipi.GetValueOrDefault())
            {
                kayitListe.Add(new HareketModel
                {
                    Id = 0,
                    Islem = "Müşteride",
                    KartID = lastRecord.KartID,
                    PAdSoyad = lastRecord.PAdSoyad,
                    Tarih = DateTime.Now,
                    Operation = 1
                });
            }

            for (int i = 0; i < kayitListe.Count; i++)
            {

                if (i < kayitListe.Count)
                {

                    DateTime normalTime = DateTime.Today;
                    TimeSpan ts = new TimeSpan(BSaat, Bdakika, Bsaniye);
                    normalTime = normalTime.Date + ts;
                    if ((i + 1) < kayitListe.Count && kayitListe[i + 1].Operation == 1)
                    {


                        if (kayitListe[i + 1].Tarih < normalTime)
                        {
                            kayitListe[i + 1].TotalMinute = 0;
                            continue;
                        }
                        if (kayitListe[i].Tarih < normalTime)
                        {
                            kayitListe[i].Tarih = normalTime;
                        }
                        var s = (kayitListe[i + 1].Tarih - kayitListe[i].Tarih).TotalMinutes;
                        kayitListe[i + 1].TotalMinute = s;
                    }

                    if ((i + 1) < kayitListe.Count && kayitListe[i + 1].Operation == 0)
                    {
                        if (kayitListe[i + 1].Tarih < normalTime)
                        {
                            kayitListe[i + 1].TotalMinute = 0;
                            continue;
                        }
                        if (kayitListe[i].Tarih < normalTime)
                        {
                            kayitListe[i].Tarih = normalTime;
                        }
                        var s = (kayitListe[i + 1].Tarih - kayitListe[i].Tarih).TotalMinutes;
                        kayitListe[i + 1].TotalMinute = s;
                    }

                    if ((i + 1) < kayitListe.Count && kayitListe[i + 1].Operation == 0 && kayitListe[i].Tipi.GetValueOrDefault())
                    {

                        kayitListe[i + 1].Tipi = true;
                    }


                }

            }
            totalWorkTime = kayitListe.Where(r => r.Operation == 1).Sum(r => r.TotalMinute);
            var customerTime = kayitListe.Where(r => r.Operation == 0 && r.Tipi.GetValueOrDefault()).Sum(r => r.TotalMinute);
            totalWorkTime += customerTime;
            tatalNotWorkedTime = kayitListe.Where(r => r.Operation == 0 && !r.Tipi.GetValueOrDefault()).Sum(r => r.TotalMinute);
        }

        public ActionResult PersonelRecord(int KayitId, string date)
        {

            //List<DetayChart> detaychart = new List<DetayChart>();
            DateTime d = Convert.ToDateTime(date);
            int gun = d.Day;
            int ay = d.Month;
            int yil = d.Year;
            //var girisSaati = db.Ayarlar.Where(s => s.id == 3).FirstOrDefault();
            int BSaat = girisSaati.Saats.Value.Hours;
            int Bdakika = girisSaati.Saats.Value.Minutes;
            int Bsaniye = girisSaati.Saats.Value.Seconds;
            //var cikisSaati = db.Ayarlar.Where(s => s.id == 4).FirstOrDefault();
            int saat = cikisSaati.Saats.Value.Hours;
            int dakika = cikisSaati.Saats.Value.Minutes;
            int saniye = cikisSaati.Saats.Value.Seconds;

            List<HareketModel> kayitListe = db.Hareketler.Where(k => k.KayitId == KayitId && k.Tarih.Value.Day == gun && k.Tarih.Value.Month == ay && k.Tarih.Value.Year == yil).Select(x => new HareketModel()
            {
                Id = x.Id,
                Islem = x.Islem,
                KartID = x.KayitliPersonel.KartID,
                PAdSoyad = x.KayitliPersonel.PAdSoyad,
                Tarih = (DateTime)x.Tarih,
                Operation = x.operation,
                Tipi = x.Tipi

            }).ToList();
            DateTime currentDate = DateTime.Now;


            var lastRecord = kayitListe.LastOrDefault();
            var firstRecord = kayitListe.FirstOrDefault();

            //DateTime EndDates = lastRecord.Tarih;
            //TimeSpan tss = new TimeSpan(saat, dakika, saniye);
            //EndDates = EndDates.Date + tss;

            if (firstRecord != null && firstRecord.Operation == 0)
            {
                DateTime normalTime = firstRecord.Tarih;
                //TimeSpan ts = new TimeSpan(BSaat, Bdakika, Bsaniye);
                // normalTime = normalTime.Date + ts;
               
                if (normalTime < new DateTime(2018, 12, 10))
                {
                    TimeSpan ts = new TimeSpan(09, 00, 00);
                    normalTime = normalTime.Date + ts;
                }
                else
                {
                    TimeSpan ts = new TimeSpan(BSaat, Bdakika, Bsaniye);
                    normalTime = normalTime.Date + ts;
                }
                if (firstRecord.Tarih > normalTime)
                {
                    var s = (firstRecord.Tarih - normalTime).TotalMinutes;
                    firstRecord.TotalMinute = s;
                }
            }


            //if (d.Date != DateTime.Today)
            //{
            //    currentDate = d;
            //    currentDate += time;
            //}


            if (lastRecord != null && lastRecord.Operation == 0 && d.Date != DateTime.Today /* && lastRecord.Tarih < EndDates && DateTime.Now.Date != EndDates.Date*/)
            {
                kayitListe.Add(new HareketModel
                {
                    Id = 0,
                    Islem = "Çıkış",
                    KartID = lastRecord.KartID,
                    PAdSoyad = lastRecord.PAdSoyad,
                    Tarih = new DateTime(yil, ay, gun, saat, dakika, saniye),
                    Operation = 0
                });

            }
            if (lastRecord != null && lastRecord.Operation == 0 && d.Date == DateTime.Today /*&& lastRecord.Tarih < EndDates && DateTime.Now.Date == EndDates.Date*/)
            {
                kayitListe.Add(new HareketModel
                {
                    Id = 0,
                    Islem = "Şuan",
                    KartID = lastRecord.KartID,
                    PAdSoyad = lastRecord.PAdSoyad,
                    Tarih = DateTime.Now,
                    Operation = 1
                });
            }
            if (lastRecord != null && lastRecord.Operation == 1 && lastRecord.Tipi.GetValueOrDefault())
            {
                kayitListe.Add(new HareketModel
                {
                    Id = 0,
                    Islem = "Müşteride",
                    KartID = lastRecord.KartID,
                    PAdSoyad = lastRecord.PAdSoyad,
                    Tarih = DateTime.Now,
                    Operation = 1
                });
            }


            for (int i = 0; i < kayitListe.Count; i++)
            {
                if (i < kayitListe.Count)
                {

                    DateTime normalTime = d;
                    if (normalTime < new DateTime(2018, 12, 10))
                    {
                        TimeSpan ts = new TimeSpan(09, 00, 00);
                        normalTime = normalTime.Date + ts;
                    }
                    else
                    {
                        TimeSpan ts = new TimeSpan(BSaat, Bdakika, Bsaniye);
                        normalTime = normalTime.Date + ts;
                    }
                    if ((i + 1) < kayitListe.Count && kayitListe[i + 1].Operation == 1)
                    {


                        if (kayitListe[i + 1].Tarih < normalTime)
                        {
                            kayitListe[i + 1].TotalMinute = 0;
                            continue;
                        }
                        if (kayitListe[i].Tarih < normalTime)
                        {
                            kayitListe[i].Tarih = normalTime;
                        }
                        var s = (kayitListe[i + 1].Tarih - kayitListe[i].Tarih).TotalMinutes;
                        kayitListe[i + 1].TotalMinute = s;
                    }

                    if ((i + 1) < kayitListe.Count && kayitListe[i + 1].Operation == 0)
                    {
                        if (kayitListe[i + 1].Tarih < normalTime)
                        {
                            kayitListe[i + 1].TotalMinute = 0;
                            continue;
                        }
                        if (kayitListe[i].Tarih < normalTime)
                        {
                            kayitListe[i].Tarih = normalTime;
                        }
                        var s = (kayitListe[i + 1].Tarih - kayitListe[i].Tarih).TotalMinutes;
                        kayitListe[i + 1].TotalMinute = s;
                    }

                    if ((i + 1) < kayitListe.Count && kayitListe[i + 1].Operation == 0 && kayitListe[i].Tipi.GetValueOrDefault())
                    {

                        kayitListe[i + 1].Tipi = true;
                    }
                
                }
            }
            var totalWorkTime = kayitListe.Where(r => r.Operation == 1).Sum(r => r.TotalMinute);
            var customerTime = kayitListe.Where(r => r.Operation == 0 && r.Tipi.GetValueOrDefault()).Sum(r => r.TotalMinute);
            totalWorkTime += customerTime;
            var tatalNotWorkedTime = kayitListe.Where(r => r.Operation == 0 && !r.Tipi.GetValueOrDefault()).Sum(r => r.TotalMinute);
            foreach (var item in kayitListe)
            {
                item.TotalCalisma = Convert.ToString(TimeSpan.FromMinutes(totalWorkTime).ToString(@"hh\s\:mm\d\k"));
                item.TotalMola = Convert.ToString(TimeSpan.FromMinutes(tatalNotWorkedTime).ToString(@"hh\s\:mm\d\k"));

                item.charMolaSaati = totalWorkTime;
                item.chartCalismaSaati = tatalNotWorkedTime;
            }
            string value = string.Empty;
            value = JsonConvert.SerializeObject(kayitListe, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            //detaychart.Add(new DetayChart { CalismaSaati = totalWorkTime, MolaSaati = tatalNotWorkedTime });
            //ViewBag.Saatler = JsonConvert.SerializeObject(detaychart);
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PersonelNotAdd(Notlar model, int NotId)
        {
            App_Data.KayitliPersonel kp = db.KayitliPersonel.Where(k => k.KId == (model.KayitId)).SingleOrDefault();
            if (kp != null)
            {
                if (NotId > 0)
                {
                    App_Data.Notlar NotPrs = db.Notlar.SingleOrDefault(x => x.id == NotId);
                    NotPrs.Not = model.Not;
                    db.SaveChanges();
                }
                else
                {
                    Notlar notEKle = new Notlar();
                    notEKle.KayitId = model.KayitId;
                    notEKle.Tarih = model.Tarih;
                    notEKle.Not = model.Not;
                    db.Notlar.Add(notEKle);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("PersonelGirisCikis");
        }

        public ActionResult NotList(int NotId, string date)
        {

            DateTime d = Convert.ToDateTime(date);
            int gun = d.Day;
            int ay = d.Month;
            int yil = d.Year;
            App_Data.Notlar model = db.Notlar.Where(x => x.id == NotId && x.Tarih.Value.Day == gun && x.Tarih.Value.Month == ay && x.Tarih.Value.Year == yil).SingleOrDefault();
            try
            {
                if (model != null)
                {
                    string value = string.Empty;
                    value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                    return Json(value, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult NotTableListele(int PersonelId, string date)
        {
            DateTime d = Convert.ToDateTime(date);
            int gun = d.Day;
            int ay = d.Month;
            int yil = d.Year;
            List<App_Data.Notlar> model = db.Notlar.Where(x => x.KayitId == PersonelId && x.Tarih.Value.Day == gun && x.Tarih.Value.Month == ay && x.Tarih.Value.Year == yil).ToList();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            var json = Json(value, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        [HttpPost]
        public ActionResult NotDelete(int id)
        {
            App_Data.Notlar aciklama = db.Notlar.Where(k => k.id == id).SingleOrDefault();
            db.Notlar.Remove(aciklama);
            db.SaveChanges();
            return RedirectToAction("PersonelGirisCikis");
        }

        [HttpPost]
        public ActionResult DetailsUpdate(KayitliPersonelDTO model)
        {
            var result = false;
            try
            {
                if (model.KId > 0)
                {
                    App_Data.KayitliPersonel kytprs = db.KayitliPersonel.SingleOrDefault(x => x.KId == model.KId);

                    if (model.Foto != null && model.Foto.ContentLength > 0)
                    {
                        string dosya = Guid.NewGuid().ToString();
                        string uzanti = Path.GetExtension(model.Foto.FileName).ToLower();
                        string dosyaAdi = dosya + uzanti;
                        model.Foto.SaveAs(Server.MapPath("~/Content/PersonelFoto/" + dosyaAdi));
                        kytprs.FotoYol = dosyaAdi;
                    }

                    kytprs.PAdSoyad = model.PAdSoyad;
                    kytprs.DogumTarihi = model.DogumTarihi;
                    kytprs.EPosta = model.EPosta;
                    kytprs.CepNum = model.CepNum;
                    kytprs.Adres = model.Adres;
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ImageUpload(int id, ImageViewModel model)
        {
            int imgId = 0;
            var file = model.ImageFile;
            byte[] imagebyte = null;

            if (file != null)
            {
                string dosya = Guid.NewGuid().ToString();
                string uzanti = Path.GetExtension(model.ImageFile.FileName).ToLower();

                string dosyaAdi = dosya + uzanti;
                file.SaveAs(Server.MapPath("~/Content/PersonelFoto/" + dosyaAdi));
                BinaryReader reader = new BinaryReader(file.InputStream);
                imagebyte = reader.ReadBytes(file.ContentLength);
                App_Data.KayitliPersonel img = db.KayitliPersonel.SingleOrDefault(x => x.KId == id);
                img.FotoYol = dosyaAdi;
                db.SaveChanges();
            }
            return Json(imgId, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PersonelDurumu()
        {
            if (Session["uyeOturum"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            if (Session["uyeTuru"].ToString() != "1" && Session["uyeTuru"].ToString() != "0")
            {
                Session.Abandon();
                return RedirectToAction("Index", "User");
            }
            DateTime d = Convert.ToDateTime(DateTime.Now);
            int gun = d.Day;
            int ay = d.Month;
            int yil = d.Year;
            //var kayitListe = db.KayitliPersonel.Where(s => s.Silindi == false || s.Silindi == null).ToList();
            var personel = db.KayitliPersonel
                .Where(s => (s.Silindi == false || s.Silindi == null) && s.KId != 89 && s.KId != 107)
               .Select(r => new
               {
                   r.KId,
                   r.Adres,
                   r.PAdSoyad,
                   r.KartID,
                   r.CepNum,
                   r.EPosta,
                   r.DogumTarihi,
                   Izinler = r.Izinler.Where(s => s.BaslangicTarihi <= d && s.BitisTarihi >= d).ToList(),
                   Hareketler = r.Hareketler.Where(s => s.Tarih.Value.Day == gun && s.Tarih.Value.Month == ay && s.Tarih.Value.Year == yil),
                   Notlar = r.Notlar.Where(s => s.Tarih == d).ToList()
               }).ToList();
           // var izinListe = db.Izinler.ToList();
            List<KayitDurumu> kayitlar = new List<KayitDurumu>();
            foreach (var item in personel)
            {
                var itemhareketleri = item.Hareketler.Where(k => k.Tarih.Value.Day == gun && k.Tarih.Value.Month == ay && k.Tarih.Value.Year == yil && k.KayitId == item.KId).ToList();
                var itemIzinler = item.Izinler.Where(k => k.BitisTarihi >= d && k.KId == item.KId).ToList();

                if (itemhareketleri == null)
                {
                    if (itemIzinler.Count > 0)
                    {
                        KayitDurumu yenikayit1 = new KayitDurumu();
                        yenikayit1.Id = item.KId;
                        yenikayit1.PAdSoyad = item.PAdSoyad;
                        yenikayit1.Islem = item.Izinler.OrderByDescending(s => s.id).FirstOrDefault().IzinTuru;
                        kayitlar.Add(yenikayit1);
                        kayitlar = kayitlar.OrderBy(s => s.PAdSoyad).ToList();
                        continue;
                    }
                    else
                    {
                        KayitDurumu yenikayit1 = new KayitDurumu();
                        yenikayit1.Id = item.KId;
                        yenikayit1.PAdSoyad = item.PAdSoyad;
                        yenikayit1.Islem = "GELMEDİ";
                        kayitlar.Add(yenikayit1);
                        kayitlar = kayitlar.OrderBy(s => s.PAdSoyad).ToList();
                        continue;
                    }
                }
                if (itemhareketleri.Count == 0)
                {
                    if (itemIzinler.Count > 0)
                    {
                        KayitDurumu yenikayit1 = new KayitDurumu();
                        //Izinler izin = new Izinler();
                        yenikayit1.Id = item.KId;
                        yenikayit1.PAdSoyad = item.PAdSoyad;
                        yenikayit1.Islem = item.Izinler.OrderByDescending(s => s.id).FirstOrDefault().IzinTuru;
                        kayitlar.Add(yenikayit1);
                        kayitlar = kayitlar.OrderBy(s => s.PAdSoyad).ToList();
                        continue;
                    }
                    else
                    {
                        KayitDurumu yenikayit1 = new KayitDurumu();
                        yenikayit1.Id = item.KId;
                        yenikayit1.PAdSoyad = item.PAdSoyad;
                        yenikayit1.Islem = "GELMEDİ";
                        kayitlar.Add(yenikayit1);
                        kayitlar = kayitlar.OrderBy(s => s.PAdSoyad).ToList();
                        continue;
                    }
                }
                try
                {
                    int? durum = itemhareketleri.OrderByDescending(p => p.Tarih).ToList().FirstOrDefault().operation;
                    Boolean? tip = itemhareketleri.OrderByDescending(p => p.Tarih).ToList().FirstOrDefault().Tipi;
                    Boolean? SehirDisi = itemhareketleri.OrderByDescending(p => p.Tarih).ToList().FirstOrDefault().SehirDisi;
                    KayitDurumu yenikayit = new KayitDurumu();
                    if ((durum ?? 1) == 0)
                    {
                        if (SehirDisi == true)
                        {
                            yenikayit.Id = item.KId;
                            yenikayit.PAdSoyad = item.PAdSoyad;
                            yenikayit.Islem = "	ŞEHİR DIŞI - MÜŞTERİ";
                            kayitlar.Add(yenikayit);
                            kayitlar = kayitlar.OrderBy(s => s.PAdSoyad).ToList();
                        }
                        else
                        {
                            yenikayit.Id = item.KId;
                            yenikayit.PAdSoyad = item.PAdSoyad;
                            yenikayit.Islem = "İÇERDE";
                            kayitlar.Add(yenikayit);
                            kayitlar = kayitlar.OrderBy(s => s.PAdSoyad).ToList();
                        }

                    }
                    else if ((durum ?? 0) == 1 && tip == true)
                    {
                        yenikayit.Id = item.KId;
                        yenikayit.PAdSoyad = item.PAdSoyad;
                        yenikayit.Islem = "MÜŞTERİDE";
                        kayitlar.Add(yenikayit);
                        kayitlar = kayitlar.OrderBy(s => s.PAdSoyad).ToList();
                    }
                    else
                    {
                        if (itemIzinler.Count > 0)
                        {
                            yenikayit.Id = item.KId;
                            yenikayit.PAdSoyad = item.PAdSoyad;
                            yenikayit.Islem = item.Izinler.OrderByDescending(s => s.id).FirstOrDefault().IzinTuru;
                            kayitlar.Add(yenikayit);
                            kayitlar = kayitlar.OrderBy(s => s.PAdSoyad).ToList();
                        }
                        else
                        {
                            yenikayit.Id = item.KId;
                            yenikayit.PAdSoyad = item.PAdSoyad;
                            yenikayit.Islem = "DIŞARDA";
                            kayitlar.Add(yenikayit);
                            kayitlar = kayitlar.OrderBy(s => s.PAdSoyad).ToList();
                        }
                    }
                }
                catch
                {

                    KayitDurumu yenikayit1 = new KayitDurumu();
                    yenikayit1.Id = item.KId;
                    yenikayit1.PAdSoyad = item.PAdSoyad;
                    yenikayit1.Islem = "DIŞARDA";
                    kayitlar.Add(yenikayit1);
                    kayitlar = kayitlar.OrderBy(s => s.PAdSoyad).ToList();
                    continue;
                }
            }
            kayitlar = kayitlar.OrderBy(s => s.Islem).ToList();
            return View(kayitlar);
        }
        [HttpGet]
        public ActionResult DurumTipi(int id, string aciklama)
        {
            App_Data.Hareketler kayit = db.Hareketler.Where(k => k.Id == id).SingleOrDefault();
            if (kayit != null)
            {
                if (kayit.Tipi == false || kayit.Tipi == null)
                {
                    kayit.Tipi = true;
                    kayit.MusteriBİlgisi = aciklama;
                    kayit.MusteriTipiDateTime = DateTime.Now;
                    db.SaveChanges();
                }
                else
                {
                    kayit.Tipi = false;
                    kayit.MusteriBİlgisi = aciklama;
                    kayit.MusteriTipiDateTime = DateTime.Now;
                    db.SaveChanges();
                }
            }
            return Json(new { success = true, responseText = "Your message successfuly sent!" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ManuelKayit(string CardId)
        {
            App_Data.KayitliPersonel kp = db.KayitliPersonel.Where(k => k.KartID == (CardId)).SingleOrDefault();
            if (kp != null)
            {
                int gun = DateTime.Now.Day;
                int ay = DateTime.Now.Month;
                int yil = DateTime.Now.Year;
                string adi = "";
                int kayitSay = db.Hareketler.Where(s => s.Tarih.Value.Day == gun && s.Tarih.Value.Month == ay && s.Tarih.Value.Year == yil && s.KayitId == kp.KId).Count();
                string islem = "";
                int operation = 0;
                if (kayitSay == 0)
                {
                    islem = "Giris";
                    operation = 0;
                }
                else
                {
                    if (kayitSay % 2 == 0)
                    {
                        islem = "Giris";
                        operation = 0;
                    }
                    else
                    {
                        islem = "Cikis";
                        operation = 1;
                    }
                }
                App_Data.Hareketler prs = new App_Data.Hareketler();
                prs.KayitId = kp.KId;
                prs.Tarih = DateTime.Now;
                prs.Islem = islem;
                prs.operation = operation;
                adi = kp.PAdSoyad;
                db.Hareketler.Add(prs);
                //db.SaveChanges();
                return RedirectToAction("PersonelGirisCikis");
            }
            else
            {
                //ViewBag.Hata = "Tanımsız Kart";
                return ViewBag.Hata = "TanımsızKart";
            }

        }

        public ActionResult PersonelIzinler()
        {
            if (Session["uyeOturum"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            if (Session["uyeTuru"].ToString() != "1" && Session["uyeTuru"].ToString() != "0")
            {
                Session.Abandon();
                return RedirectToAction("Index", "User");
            }
            List<App_Data.KayitliPersonel> KayitListe = db.KayitliPersonel.ToList();
            return View(KayitListe);
        }

        [HttpGet]
        public ActionResult PersonelIzinBilgi(int KayitId)
        {
            var prs = db.KayitliPersonel.Where(s => s.KId == KayitId).Select(x => new
            {
                PAdSoyad = x.PAdSoyad,
                Adres = x.Adres,
                CepNum = x.CepNum,
                EPosta = x.EPosta,
                DogumTarihi = x.DogumTarihi,
                IseBaslamaTarihi = x.IseBaslamaTarihi,
                FotoYol = x.FotoYol

            }).SingleOrDefault();

            string value = string.Empty;
            value = JsonConvert.SerializeObject(prs, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult IzinKaydet(Izinler model)
        {
            if (model.BitisTarihi != null && model.BaslangicTarihi != null)
            {
                App_Data.KayitliPersonel kp = db.KayitliPersonel.Where(k => k.KId == (model.KId)).SingleOrDefault();
                if (kp != null)
                {
                    Izinler KayitEkle = new Izinler();
                    KayitEkle.KId = model.KId;
                    KayitEkle.BaslangicTarihi = model.BaslangicTarihi;
                    KayitEkle.BitisTarihi = model.BitisTarihi;
                    KayitEkle.IzinTuru = model.IzinTuru;
                    KayitEkle.Aciklama = model.Aciklama;
                    db.Izinler.Add(KayitEkle);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("PersonelIzinler");
        }

        [HttpGet]
        public JsonResult IzinListesi(int kaytId)
        {
            try
            {
                var model = db.Izinler.OrderByDescending(s => s.id).Where(k => k.KId == kaytId).Select(x => new
                {
                    PAdSoyad = x.KayitliPersonel.PAdSoyad,
                    id = x.id,
                    kaytId = x.KId,
                    BaslangicTarihi = x.BaslangicTarihi,
                    BitisTarihi = x.BitisTarihi,
                    IzinTuru = x.IzinTuru,
                    Aciklama = x.Aciklama
                }).ToList();

                if (model != null)
                {
                    string value = string.Empty;
                    value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                    var json = Json(value, JsonRequestBehavior.AllowGet);
                    json.MaxJsonLength = int.MaxValue;
                    return json;
                }
                else
                {
                    return Json("");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult IzinDelete(int id)
        {
            App_Data.Izinler İzinSil = db.Izinler.Where(k => k.id == id).SingleOrDefault();
            db.Izinler.Remove(İzinSil);
            db.SaveChanges();
            return RedirectToAction("PersonelIzinler");
        }

        public ActionResult IzinUpdateList(int id)
        {
            App_Data.Izinler model = db.Izinler.Where(x => x.id == id).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            ViewBag.Secim = model.IzinTuru;
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IzinTuruList()
        {
            List<IzinTurlerics> turler = new List<IzinTurlerics>();
            turler.Add(new IzinTurlerics { Value = "1", Name = "YILLIK İZİN" });
            turler.Add(new IzinTurlerics { Value = "2", Name = "SAĞLIK RAPORU" });
            turler.Add(new IzinTurlerics { Value = "3", Name = "MAZARET İZNİ" });
            turler.Add(new IzinTurlerics { Value = "4", Name = "RESMİ TATİL İZNİ" });
            turler.Add(new IzinTurlerics { Value = "5", Name = "DOĞUM İZNİ" });
            turler.Add(new IzinTurlerics { Value = "6", Name = "BABALIK İZNİ" });
            turler.Add(new IzinTurlerics { Value = "7", Name = "ÖLÜM İZNİ" });

            string value = string.Empty;
            value = JsonConvert.SerializeObject(turler, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult IzinUpdate(Izinler model, int NotId)
        {
            App_Data.Izinler Izin = db.Izinler.Where(x => x.id == NotId).SingleOrDefault();
            Izin.Aciklama = model.Aciklama;
            Izin.BaslangicTarihi = model.BaslangicTarihi;
            Izin.BitisTarihi = model.BitisTarihi;
            Izin.IzinTuru = model.IzinTuru;
            db.SaveChanges();
            return Json("Güncellendi", JsonRequestBehavior.AllowGet);
        }

        void GetDateCalculater(List<Hareketler> hareket, out double totalWorkTime, out double tatalNotWorkedTime, string date)
        {

            DateTime d = Convert.ToDateTime(date);
            int gun = d.Day;
            int ay = d.Month;
            int yil = d.Year;

            //var cikisSaati = db.Ayarlar.Where(s => s.id == 4).FirstOrDefault();
            int saat = cikisSaati.Saats.Value.Hours;
            int dakika = cikisSaati.Saats.Value.Minutes;
            int saniye = cikisSaati.Saats.Value.Seconds;
            //var girisSaati = db.Ayarlar.Where(s => s.id == 3).FirstOrDefault();
            int BSaat = girisSaati.Saats.Value.Hours;
            int Bdakika = girisSaati.Saats.Value.Minutes;
            int Bsaniye = girisSaati.Saats.Value.Seconds;

            var kayitListe = hareket.Select(x => new HareketModel()
            {
                Id = x.Id,
                Islem = x.Islem,
                Tarih = (DateTime)x.Tarih,
                Operation = x.operation,
                Tipi = x.Tipi
            }).ToList();

            var lastRecord = kayitListe.LastOrDefault();
            var firstRecord = kayitListe.FirstOrDefault();

            //if (firstRecord != null && firstRecord.Operation == 0)
            //{
            //    DateTime normalTime = firstRecord.Tarih;
            //    //if (normalTime < new DateTime(2018,12,10))
            //    //{
            //    //    TimeSpan ts = new TimeSpan(09, 00, 00);
            //    //    normalTime = normalTime.Date + ts;
            //    //}
            //    //else
            //    //{
            //    //    TimeSpan ts = new TimeSpan(BSaat, Bdakika, Bsaniye);
            //    //    normalTime = normalTime.Date + ts;
            //    //}

            //    if (firstRecord.Tarih > normalTime)
            //    {
            //        var s = (firstRecord.Tarih - normalTime).TotalMinutes;
            //        firstRecord.TotalMinute = s;
            //    }
            //}
            if (firstRecord != null && firstRecord.Operation == 0)
            {
                DateTime normalTime = firstRecord.Tarih;
                //TimeSpan ts = new TimeSpan(BSaat, Bdakika, Bsaniye);
                // normalTime = normalTime.Date + ts;

                if (normalTime < new DateTime(2018, 12, 10))
                {
                    TimeSpan ts = new TimeSpan(09, 00, 00);
                    normalTime = normalTime.Date + ts;
                }
                else
                {
                    TimeSpan ts = new TimeSpan(BSaat, Bdakika, Bsaniye);
                    normalTime = normalTime.Date + ts;
                }
                if (firstRecord.Tarih > normalTime)
                {
                    var s = (firstRecord.Tarih - normalTime).TotalMinutes;
                    firstRecord.TotalMinute = s;
                }
            }
            if (lastRecord != null && lastRecord.Operation == 0)
            {
                kayitListe.Add(new HareketModel
                {
                    Id = 0,
                    Islem = "Şuan",
                    KartID = lastRecord.KartID,
                    PAdSoyad = lastRecord.PAdSoyad,
                    Tarih = new DateTime(yil, ay, gun, saat, dakika, saniye),
                    Operation = 1
                });
            }
            if (lastRecord != null && lastRecord.Operation == 1 && lastRecord.Tipi.GetValueOrDefault())
            {
                kayitListe.Add(new HareketModel
                {
                    Id = 0,
                    Islem = "Müşteride",
                    KartID = lastRecord.KartID,
                    PAdSoyad = lastRecord.PAdSoyad,
                    //Tarih = new DateTime(yil, ay, gun, 19, 00, 00),
                    Tarih = new DateTime(yil, ay, gun, saat, dakika, saniye),
                    Operation = 1
                });
            }

            for (int i = 0; i < kayitListe.Count; i++)
            {

                if (i < kayitListe.Count)
                {

                    DateTime normalTime = d;
                    if (normalTime < new DateTime(2018, 12, 10))
                    {
                        TimeSpan ts = new TimeSpan(09, 00, 00);
                        normalTime = normalTime.Date + ts;
                    }
                    else
                    {
                        TimeSpan ts = new TimeSpan(BSaat, Bdakika, Bsaniye);
                        normalTime = normalTime.Date + ts;
                    }
                    if ((i + 1) < kayitListe.Count && kayitListe[i + 1].Operation == 1)
                    {


                        if (kayitListe[i + 1].Tarih < normalTime)
                        {
                            kayitListe[i + 1].TotalMinute = 0;
                            continue;
                        }
                        if (kayitListe[i].Tarih < normalTime)
                        {
                            kayitListe[i].Tarih = normalTime;
                        }
                        var s = (kayitListe[i + 1].Tarih - kayitListe[i].Tarih).TotalMinutes;
                        kayitListe[i + 1].TotalMinute = s;
                    }

                    if ((i + 1) < kayitListe.Count && kayitListe[i + 1].Operation == 0)
                    {
                        if (kayitListe[i + 1].Tarih < normalTime)
                        {
                            kayitListe[i + 1].TotalMinute = 0;
                            continue;
                        }
                        if (kayitListe[i].Tarih < normalTime)
                        {
                            kayitListe[i].Tarih = normalTime;
                        }
                        var s = (kayitListe[i + 1].Tarih - kayitListe[i].Tarih).TotalMinutes;
                        kayitListe[i + 1].TotalMinute = s;
                    }

                    if ((i + 1) < kayitListe.Count && kayitListe[i + 1].Operation == 0 && kayitListe[i].Tipi.GetValueOrDefault())
                    {

                        kayitListe[i + 1].Tipi = true;
                    }


                }
            }
            totalWorkTime = kayitListe.Where(r => r.Operation == 1).Sum(r => r.TotalMinute);
            var customerTime = kayitListe.Where(r => r.Operation == 0 && r.Tipi.GetValueOrDefault()).Sum(r => r.TotalMinute);
            totalWorkTime += customerTime;
            tatalNotWorkedTime = kayitListe.Where(r => r.Operation == 0 && !r.Tipi.GetValueOrDefault()).Sum(r => r.TotalMinute);
        }

        public ActionResult SehirDisinda(int id)
        {
            int gun = DateTime.Now.Day;
            int ay = DateTime.Now.Month;
            int yil = DateTime.Now.Year;
            var kayit = db.KayitliPersonel.Where(x => x.KId == id).Select(s => new { s.KartID, s.KId, s.PAdSoyad }).SingleOrDefault();
            if (kayit != null)
            {
                int kayitSay = db.Hareketler.Where(s => s.Tarih.Value.Day == gun && s.Tarih.Value.Month == ay && s.Tarih.Value.Year == yil && s.KayitId == kayit.KId).Count();
                var ensonkayit = db.Hareketler.OrderByDescending(x=>x.Tarih).FirstOrDefault();
                if (ensonkayit != null)
                {
                    App_Data.Hareketler newHareket = new App_Data.Hareketler();
                    newHareket.KayitId = kayit.KId;
                    newHareket.Tarih = new DateTime(yil, ay, gun, 08, 00, 00);
                    newHareket.Islem = "Giris";
                    newHareket.operation = 0;
                    newHareket.SehirDisi = true;
                    newHareket.SourceId = ensonkayit.SourceId;

                    db.Hareketler.Add(newHareket);
                    db.SaveChanges(); 
                }
            }
            return RedirectToAction("PersonelGirisCikis");
        }

        public ActionResult UserHaftalik()
        {
            if (Session["uyeOturum"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            return View();
        }

        [HttpGet]
        public ActionResult WeekFilter(string gelen)
        {
            DateTime date = Convert.ToDateTime(gelen);
            DateTime dt = date.StartOfWeek(DayOfWeek.Monday);

            DateTime bitisTarih = dt.AddDays(5);

            var kayitListe = db.KayitliPersonel.Distinct().Where(s => (s.Silindi == false || s.Silindi == null) && s.KId != 89 && s.KId != 107).ToList();
            var kayit = new List<HaftalikPersonel>();

            DateTime d = Convert.ToDateTime(dt);

            Dictionary<string, string> workTime = new Dictionary<string, string>();
            foreach (var item in kayitListe)
            {
                var kyt = new HaftalikPersonel
                {
                    PAdSoyad = item.PAdSoyad,
                    BaslangicTarihi = dt,
                    BitisTarihi = bitisTarih,
                    WorkTime = new Dictionary<string, string>()
                };
                double ToplamIzinSaati = 0;
                double HaftalikTotal = 0;
                workTime.Clear();
                int gunSayisi = 0;
                for (int i = 0; i < 7; i++)
                {

                    DateTime day = dt.AddDays(i);
                    int gun = day.Day;
                    int ay = day.Month;
                    int yil = day.Year;
                    var harekets = item.Hareketler.Where(s => s.Tarih.Value.Day == gun && s.Tarih.Value.Month == ay && s.Tarih.Value.Year == yil).ToList();
                    var Izin = item.Izinler.Where(x => x.BaslangicTarihi.Value.Day == gun && x.BaslangicTarihi.Value.Month == ay && x.BaslangicTarihi.Value.Year == yil  && x.KId == item.KId).FirstOrDefault();

                    if (harekets.Count > 0)
                    {
                        gunSayisi++;
                    }
                    kyt.Hareketler = harekets.Select(x => new HareketModel()
                    {
                        Id = x.Id,
                        Islem = x.Islem,
                        Tarih = (DateTime)x.Tarih,
                        Operation = x.operation
                    }).ToList();

                    double totalWorkTime = 0;
                    double tatalNotWorkedTime = 0;


                    string IzinSaati = "";

                    if (Izin != null)
                    {

                        DateTime? IzinBaslangic = Izin.BaslangicTarihi;
                        DateTime? IzinBitis = Izin.BitisTarihi;
                        TimeSpan ts = (IzinBaslangic - IzinBitis) ?? TimeSpan.Zero;

                        //IzinSaati = Convert.ToString(ts.ToString(@"hh\s\:mm\d\k"));
                        //dResults = (IzinBaslangic - bitisTarih).Value.TotalHours;
                        ToplamIzinSaati += ts.TotalMinutes;


                    }

                    //ToplamIzinSaati += IzinSaati;


                    GetDateCalculater(harekets, out totalWorkTime, out tatalNotWorkedTime, Convert.ToString(day));
                    int totalCalisma = Convert.ToInt32(totalWorkTime);
                    int Calismasaat = totalCalisma / 60;
                    int calismadakika = totalCalisma - (Calismasaat * 60);

                    HaftalikTotal += totalWorkTime;

                    int haftaliktotalZaman = Convert.ToInt32(HaftalikTotal);
                    double haftalikcalismaSaat = haftaliktotalZaman / 60;
                    double haftalikDakika = haftaliktotalZaman - (haftalikcalismaSaat * 60);
                    kyt.HaftalikTotal = Convert.ToString(haftalikcalismaSaat + "s:" + haftalikDakika + "dk");
                    if (gunSayisi == 0)
                    {
                        //continue;
                    }
                    else
                    {
                        //kyt.GunlukTotalCalisma = haftalikcalismaSaat / gunSayisi;

                    }
                    int totalMola = Convert.ToInt32(tatalNotWorkedTime);
                    int molaSaat = totalMola / 60;
                    int molaDakika = totalMola - (molaSaat * 60);


                    //kyt.PazartesiCalisma = Convert.ToString(Calismasaat + "s:" + calismadakika.ToString().PadLeft(2, '0') + "dk");
                    kyt.PazartesiCalisma = Convert.ToString(TimeSpan.FromMinutes(totalWorkTime).ToString(@"hh\s\:mm\d\k"));
                    kyt.IzinSaati = Convert.ToString(TimeSpan.FromMinutes(ToplamIzinSaati).ToString(@"hh\s\:mm\d\k"));
                    kyt.WorkTime.Add(day.DayOfWeek.ToString(), kyt.PazartesiCalisma);

                }
                kayit.Add(kyt);

            }
            string value = string.Empty;
            value = JsonConvert.SerializeObject(kayit, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AylikDetay()
        {
            if (Session["uyeOturum"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            List<App_Data.KayitliPersonel> model = db.KayitliPersonel.Where(s => (s.Silindi == false || s.Silindi == null) && s.KId != 89 && s.KId != 107).ToList();
            return View(model);
           
        }

        public ActionResult AylikTotalCalisma(string donem, int id)
        {
            DateTime d = Convert.ToDateTime(donem);
            int ay = d.Month;
            int yil = d.Year;
            int days = DateTime.DaysInMonth(yil, ay);
            var personelId = db.KayitliPersonel.Distinct().Where(s => s.Silindi == false || s.Silindi == null && s.KId == id).ToList();
            //var personelId = db.KayitliPersonel.Distinct().Where(s => s.Silindi == false || s.Silindi == null && s.KId == id).Select(r => new
            //{
            //    r.KId,
            //    r.Adres,
            //    r.PAdSoyad,
            //    r.KartID,
            //    r.CepNum,
            //    r.EPosta,
            //    r.DogumTarihi
            //}).ToList();
            var kayit = new List<MonthPuantaj>();
            //Dictionary<string, string> monthTime = new Dictionary<string, string>();
            List<MonthPuantaj> puantaj = new List<MonthPuantaj>();
            foreach (var item in personelId)
            {
                var kyt = new MonthPuantaj
                {
                    PAdSoyad = item.PAdSoyad,
                    //MonthTime = new Dictionary<string, string>()
                    MonthTime = new List<ViewModel.PuantajModel>()
                };
                puantaj.Clear();

                int gunSayisi = 0;
                double AylikTotal = 0;
                double AylikMolaTotal = 0;
                for (int i = 0; i < days; i++)
                {
                    DateTime day = d.AddDays(i);
                    int gun = day.Day;
                    int ayy = day.Month;
                    int yill = day.Year;
                    var harekets = item.Hareketler.Where(s => s.Tarih.Value.Day == gun && s.Tarih.Value.Month == ayy && s.Tarih.Value.Year == yill).ToList();
                    kyt.Hareketler = harekets.Select(x => new HareketModel()
                    {
                        Id = x.Id,
                        Islem = x.Islem,
                        Tarih = (DateTime)x.Tarih,
                        Operation = x.operation
                    }).ToList();

                    double totalWorkTime = 0;
                    double tatalNotWorkedTime = 0;

                    GetDateCalculater(harekets, out totalWorkTime, out tatalNotWorkedTime, Convert.ToString(day));
                    AylikTotal += totalWorkTime;
                    AylikMolaTotal += tatalNotWorkedTime;
                    kyt.CalismaZamani = Convert.ToString(TimeSpan.FromMinutes(totalWorkTime).ToString(@"hh\s\:mm\d\k"));
                    kyt.TotalAylikCalisma = Convert.ToString(TimeSpan.FromMinutes(AylikTotal).TotalHours);
                    kyt.TotalAylikMola = Convert.ToString(TimeSpan.FromMinutes(AylikMolaTotal).TotalHours);
                    kyt.MonthTime.Add(new ViewModel.PuantajModel { Gun = Convert.ToInt32(day.Date.ToString("dd")), CalismaZamani = kyt.CalismaZamani });
                }
                kayit.Add(kyt);
            }
            string value = string.Empty;
            value = JsonConvert.SerializeObject(kayit, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

    }
}