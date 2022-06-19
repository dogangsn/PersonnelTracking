using DocumentFormat.OpenXml.Drawing;
using PersonnelTracking.App_Data;
using PersonnelTracking.Models;
using PersonnelTracking.ViewModel;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using Tracking.Models.Models.Entity;
using Tracking.Models.Models.PersonnelMonthlyInquiry;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace PersonnelTracking.Controllers
{
    public class AdminController : Controller
    {
        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
        Tracking.Services.Services.CryptographyService _Cryptography;
        PersonnelTrackingDb db = new PersonnelTrackingDb();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["uyeOturum"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            if (Session["uyeTuru"].ToString() != "1")
            {
                Session.Abandon();
                return RedirectToAction("Index", "User");
            }
            DateTime d = Convert.ToDateTime(DateTime.Now);
            int gun = d.Day;
            int ay = d.Month;
            int yil = d.Year;
            List<ChartModel> kayitliPersonel = new List<ChartModel>();//db.KayitliPersonel.ToList();
            var personel = db.KayitliPersonel
                     .OrderBy(s => s.PAdSoyad)
                     .Where(x => x.KId != 89 && x.KId != 107 && x.KId != 106 && x.Silindi != true)
                     .Select(r => new
                     {
                         r.KId,
                         r.Adres,
                         r.PAdSoyad,
                         r.KartID,
                         r.CepNum,
                         r.EPosta,
                         r.DogumTarihi,
                         //Izinler = r.Izinler.Where(s => s.BaslangicTarihi <= d && s.BitisTarihi >= d).ToList(),
                         Hareketler = r.Hareketler.Where(s => s.Tarih.Value.Day == gun && s.Tarih.Value.Month == ay && s.Tarih.Value.Year == yil).OrderBy(x => x.Tarih).ToList()
                         //Notlar = r.Notlar.Where(s => s.Tarih == d).ToList()
                     }).ToList();

            foreach (var item in personel)
            {
                var count = 0;
                double totalWorkTime = 0;
                double tatalNotWorkedTime = 0;
                CalculateWorkTimee(item.Hareketler.Where(k => k.Tarih.Value.Date == DateTime.Now.Date).ToList(), out totalWorkTime, out tatalNotWorkedTime);
                double totalCalisma = totalWorkTime;
                double totalMola = tatalNotWorkedTime;
                double MolaSaat = totalMola / 60;
                double Calismasaat = totalCalisma / 60;
                double calismadakika = totalCalisma - (Calismasaat * 60);
                double totalZaman = Calismasaat;
                double totalMolaa = MolaSaat;
                foreach (var item2 in item.Hareketler)
                {
                    if (item2.Tarih.Value.Day == DateTime.Now.Day && item2.Tarih.Value.Month == DateTime.Now.Month && item2.Tarih.Value.Year == DateTime.Now.Year && item2.KayitId == item.KId && item2.operation == 1)
                    {
                        count++;
                    }
                }
                kayitliPersonel.Add(new ChartModel { IsimSoyisim = item.PAdSoyad, CikisSayisi = count, CalismaSaati = totalZaman, MolaSaati = totalMolaa });


            }
            ViewBag.PadSoyad = JsonConvert.SerializeObject(kayitliPersonel);
            return View(db.KayitliPersonel.OrderBy(s => s.PAdSoyad).Where(s => s.Silindi == false || s.Silindi == null).ToList());
        }

        public ActionResult PersonelBilgileri()
        {
            if (Session["uyeOturum"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            if (Session["uyeTuru"].ToString() != "1")
            {
                Session.Abandon();
                return RedirectToAction("Index", "User");
            }
            //var kayitliPersonel = db.KayitliPersonel.Where(s => s.Silindi == false || s.Silindi == null).Select(r => new
            //{
            //    r.Adres,
            //    r.CepNum,
            //    r.Departman,
            //    r.DogumTarihi,
            //    r.EPosta,
            //    r.FingerID,
            //    r.FotoYol,
            //    r.IseBaslamaTarihi,
            //    r.KartID,
            //    r.KId,
            //    r.PAdSoyad,
            //    r.Silindi,
            //    r.QRPath,
            //    r.MobilePass
            //}).ToList();
            List<App_Data.KayitliPersonel> kayitliPersonel = db.KayitliPersonel.Where(s => s.Silindi == false || s.Silindi == null).ToList();
            return View(kayitliPersonel);
        }
        [HttpGet]
        public ActionResult SilinenPersonller()
        {
            var Silinenkayit = db.KayitliPersonel.Where(s => s.Silindi == true).Select(s => new
            {

                KId = s.KId,
                KartID = s.KartID,
                PAdSoyad = s.PAdSoyad

            }).ToList();



            string value = string.Empty;
            value = JsonConvert.SerializeObject(Silinenkayit, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Kullanicilar()
        {
            if (Session["uyeOturum"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            if (Session["uyeTuru"].ToString() != "1")
            {
                Session.Abandon();
                return RedirectToAction("Index", "User");
            }
            List<App_Data.Login> loginKayit = db.Login.ToList();
            return View(loginKayit);
        }

        public ActionResult HaftalikGirisCikis()
        {
            if (Session["uyeOturum"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            if (Session["uyeTuru"].ToString() != "1")
            {
                Session.Abandon();
                return RedirectToAction("Index", "User");
            }
            return View();
        }

        public ActionResult PersonelCalistigiZaman()
        {
            if (Session["uyeOturum"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            if (Session["uyeTuru"].ToString() != "1")
            {
                Session.Abandon();
                return RedirectToAction("Index", "User");
            }
            //DateTime tarih = Convert.ToDateTime(DateTime.Now.ToString("MM-dd-yyyy hh:mm"));
            string ATarih = DateTime.Now.Month + "." + DateTime.Now.Day + "." + DateTime.Now.Year + " 00:00";
            string BTarih = DateTime.Now.Month + "." + DateTime.Now.Day + "." + DateTime.Now.Year + " 23:59";
            string query = "Select X.KayitId,kp.PAdSoyad, Min(X.BeginDate) as GirisSaati,Max(X.EndDate) as CikisSaati, Sum(Datediff(MINUTE,X.BeginDate,X.EndDate)) as Dakika from(select A.KayitId, A.Tarih as BeginDate, (Select Top 1 B.Tarih From dogantestdb.Hareketler B Where operation = 1 and B.KayitId=A.KayitId and B.Tarih > A.Tarih  and B.Tarih <= '" + BTarih + "') as EndDate from dogantestdb.Hareketler A Where Tarih >= '" + ATarih + "' and Tarih<= '" + BTarih + "'  and KayitId = KayitId and operation = 0 )X Left outer Join dogantestdb.KayitliPersonel kp on X.KayitId = kp.KId  where X.EndDate is not null Group By X.KayitId,kp.PAdSoyad";
            List<ViewModel.CalismaSaatleri> saatler = ((IObjectContextAdapter)db).ObjectContext.ExecuteStoreQuery<ViewModel.CalismaSaatleri>(query).ToList();
            foreach (var item in saatler)
            {
                int totalSaat = item.Dakika;
                int saat = totalSaat / 60;
                int dakika = totalSaat - (saat * 60);
                item.Sure = saat.ToString() + " s:" + dakika.ToString() + " d";
            }
            return View(saatler);
        }

        [HttpGet]
        public ActionResult PersonelRecord(int KayitId)
        {
            var model = db.KayitliPersonel.Where(x => x.KId == KayitId).Select(s => new
            {
                PAdSoyad = s.PAdSoyad,
                Adres = s.Adres,
                CepNum = s.CepNum,
                EPosta = s.EPosta,
                DogumTarihi = s.DogumTarihi,
                IseBaslamaTarihi = s.IseBaslamaTarihi,
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

        public ActionResult AddUser(string KullaniciAdi, string Parola)
        {
            _Cryptography = new Tracking.Services.Services.CryptographyService();
            Login newuser = new Login();

            newuser.KAdi = KullaniciAdi;
            //newuser.Sifre = Parola;
            //newuser.Sifre = ConvertStringToMD5(Parola);
            newuser.Sifre = _Cryptography.ConvertStringToMD5(Parola);
            newuser.uyeTuru = 0;
            db.Login.Add(newuser);
            db.SaveChanges();

            return RedirectToAction("Kullanicilar");

        }
        [HttpPost]
        public ActionResult KullaniciSil(int? id)
        {
            App_Data.Login kayit = db.Login.Where(k => k.Id == id).FirstOrDefault();
            db.Login.Remove(kayit);
            db.SaveChanges();
            ViewBag.sonuc = "Kayıt Sİlindi.";
            return Json(kayit, JsonRequestBehavior.AllowGet);
        }

        public JsonResult KullaniciBilgileri(int KId)
        {
            App_Data.Login model = db.Login.Where(x => x.Id == KId).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateUser(App_Data.Login model)
        {
            var result = false;
            try
            {
                if (model.Id > 0)
                {
                    App_Data.Login kullanici = db.Login.SingleOrDefault(x => x.Id == model.Id);
                    kullanici.KAdi = model.KAdi;
                    kullanici.Sifre = model.Sifre;
                    kullanici.uyeTuru = model.uyeTuru;
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PersonelBilgiListe(int KId)
        {
            List<App_Data.KayitliPersonel> kysPers = db.KayitliPersonel.Where(s => s.KId == KId).ToList();
            return View(kysPers);
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
                    kytprs.PAdSoyad = model.PAdSoyad;
                    kytprs.DogumTarihi = model.DogumTarihi;
                    kytprs.EPosta = model.EPosta;
                    kytprs.CepNum = model.CepNum;
                    kytprs.Adres = model.Adres;
                    kytprs.IseBaslamaTarihi = model.IseBaslamaTarihi;
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
                string uzanti = System.IO.Path.GetExtension(model.ImageFile.FileName).ToLower();

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

        public ActionResult PersonelSil(int? id)
        {
            //App_Data.KayitliPersonel model = new App_Data.KayitliPersonel();
            App_Data.KayitliPersonel kayit = db.KayitliPersonel.Where(k => k.KId == id).FirstOrDefault();
            //db.KayitliPersonel.Remove(kayit);
            kayit.Silindi = true;
            db.SaveChanges();
            ViewBag.sonuc = "Kayıt Sİlindi.";
            // return RedirectToAction("PersonelListe");
            return Json(kayit, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SayfaYonetimi()
        {
            if (Session["uyeOturum"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            if (Session["uyeTuru"].ToString() != "1")
            {
                Session.Abandon();
                return RedirectToAction("Index", "User");
            }
            //Ayarlar ayar = db.Ayarlar.SingleOrDefault();
            List<App_Data.Ayarlar> ayar = db.Ayarlar.ToList();
            return View(ayar);
        }

        public ActionResult AylikPuantaj()
        {
            if (Session["uyeOturum"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            if (Session["uyeTuru"].ToString() != "1")
            {
                Session.Abandon();
                return RedirectToAction("Index", "User");
            }
            return View();
        }

        public ActionResult PersonelAylikSorgulama()
        {
            if (Session["uyeOturum"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            if (Session["uyeTuru"].ToString() != "1")
            {
                Session.Abandon();
                return RedirectToAction("Index", "User");
            }
            List<App_Data.KayitliPersonel> model = db.KayitliPersonel.ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult WeekFilter(string gelen)
        {
            DateTime date = Convert.ToDateTime(gelen);
            DateTime dt = date.StartOfWeek(DayOfWeek.Monday);

            DateTime bitisTarih = dt.AddDays(5);

            var kayitListe = db.KayitliPersonel.Distinct().Where(s => s.Silindi == false || s.Silindi == null).ToList();
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
                double HaftalikTotal = 0;
                double ToplamIzinSaati = 0;
                workTime.Clear();
                int gunSayisi = 0;
                for (int i = 0; i < 7; i++)
                {

                    DateTime day = dt.AddDays(i);
                    int gun = day.Day;
                    int ay = day.Month;
                    int yil = day.Year;
                    var harekets = item.Hareketler.Where(s => s.Tarih.Value.Day == gun && s.Tarih.Value.Month == ay && s.Tarih.Value.Year == yil).ToList();
                    var Izin = item.Izinler.Where(x=> x.BaslangicTarihi.Value.Day == gun && x.BaslangicTarihi.Value.Month == ay && x.BaslangicTarihi.Value.Year == yil && x.KId == item.KId).FirstOrDefault();
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
                    //var result = (TimeSpan?)null;
                    //var dResults;
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

                    CalculateWorkTime(harekets, out totalWorkTime, out tatalNotWorkedTime, Convert.ToString(day));
                    int totalCalisma = Convert.ToInt32(totalWorkTime);
                    int Calismasaat = totalCalisma / 60;
                    int calismadakika = totalCalisma - (Calismasaat * 60);

                    HaftalikTotal += totalWorkTime;

                    int haftaliktotalZaman = Convert.ToInt32(HaftalikTotal);
                    double haftalikcalismaSaat = haftaliktotalZaman / 60;
                    double haftalikDakika = haftaliktotalZaman - (haftalikcalismaSaat * 60);
                    kyt.HaftalikTotal = Convert.ToString((haftalikcalismaSaat<10 ? "0"+haftalikcalismaSaat+"s:" : haftalikcalismaSaat + "s:") + (haftalikDakika<10 ? "0"+haftalikDakika+"dk" : haftalikDakika + "dk"));
                    //kyt.HaftalikTotal = Convert.ToString(TimeSpan.FromMinutes(haftaliktotalZaman).ToString(@"hh\s\:mm\d\k"));
                    double totalgun;
                    double totaldakika;
                    if (gunSayisi == 0)
                    {
                        //continue;
                    }
                    else
                    {
                        totalgun = haftalikcalismaSaat / gunSayisi;
                        totaldakika = totalgun * 60;
                        kyt.GunlukTotalCalisma = Convert.ToString(TimeSpan.FromMinutes(totaldakika).ToString(@"hh\s\:mm\d\k"));
                    }
                    int totalMola = Convert.ToInt32(tatalNotWorkedTime);
                    int molaSaat = totalMola / 60;
                    int molaDakika = totalMola - (molaSaat * 60);


                    //kyt.PazartesiCalisma = Convert.ToString(Calismasaat + "s:" + calismadakika.ToString().PadLeft(2, '0') + "dk");
                    kyt.PazartesiCalisma = Convert.ToString(TimeSpan.FromMinutes(totalWorkTime).ToString(@"hh\s\:mm\d\k"));
                    //kyt.IzinSaati = Convert.ToString(TimeSpan.FromMinutes().ToString(@"hh\s\:mm\d\k"));
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

        void CalculateWorkTime(List<Hareketler> hareket, out double totalWorkTime, out double tatalNotWorkedTime, string date)
        {
            DateTime d = Convert.ToDateTime(date);
            int gun = d.Day;
            int ay = d.Month;
            int yil = d.Year;

            var cikisSaati = db.Ayarlar.Where(s => s.id == 4).FirstOrDefault();
            int saat = cikisSaati.Saats.Value.Hours;
            int dakika = cikisSaati.Saats.Value.Minutes;
            int saniye = cikisSaati.Saats.Value.Seconds;
            var girisSaati = db.Ayarlar.Where(s => s.id == 3).FirstOrDefault();
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
                TimeSpan ts = new TimeSpan(9, 0, 0);
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
                    Tarih = new DateTime(yil, ay, gun, saat, dakika, saniye),
                    Operation = 1
                });
            }

            for (int i = 0; i < kayitListe.Count; i++)
            {

                if (i < kayitListe.Count)
                {
                    if (i < kayitListe.Count)
                    {

                        DateTime normalTime = d;
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

            }
            totalWorkTime = kayitListe.Where(r => r.Operation == 1).Sum(r => r.TotalMinute);
            var customerTime = kayitListe.Where(r => r.Operation == 0 && r.Tipi.GetValueOrDefault()).Sum(r => r.TotalMinute);
            totalWorkTime += customerTime;
            tatalNotWorkedTime = kayitListe.Where(r => r.Operation == 0 && !r.Tipi.GetValueOrDefault()).Sum(r => r.TotalMinute);



            foreach (var item in kayitListe)
            {
                int totalCalisma = Convert.ToInt32(totalWorkTime);
                int Calismasaat = totalCalisma / 60;
                int calismadakika = totalCalisma - (Calismasaat * 60);
                item.TotalCalisma = Calismasaat.ToString() + " s:" + calismadakika.ToString() + " dk";
            }
        }

        //public ActionResult DetailsReport()
        //{
        //    LocalReport localReport = new LocalReport();
        //    localReport.ReportPath = Server.MapPath("~/Views/Admin/Report1.rdlc");
        //    return View(localReport);
        //}

        public ActionResult KaydiSilinenPersonel()
        {
            return View();
        }

        public ActionResult AylikPuantajListele(string donem)
        {
            DateTime d = Convert.ToDateTime(donem);
            int ay = d.Month;
            int yil = d.Year;
            int days = DateTime.DaysInMonth(yil, ay);
            var kayitListe = db.KayitliPersonel.Distinct().Where(s => s.Silindi == false || s.Silindi == null).ToList();
            var kayit = new List<AylikPersonel>();
            //List<bool> puantajList = new List<bool>();
            AylikPuantajModel.puantajKontrol puantajControl = 0;

            //Dictionary<string, string> workTime = new Dictionary<string, string>();
            foreach (var item in kayitListe)
            {
                var kyt = new AylikPersonel
                {
                    PAdSoyad = item.PAdSoyad,
                    WorkTime = new List<AylikPuantajModel>()
                };
                //****
                bool Izinli = false;
                for (int i = 0; i <= days; i++)
                {
                    DateTime day = d.AddDays(i);
                    int gun = day.Day;
                    int ayy = day.Month;
                    int yill = day.Year;
                    var harekets = item.Hareketler.Where(s => s.Tarih.Value.Day == gun && s.Tarih.Value.Month == ayy && s.Tarih.Value.Year == yill).ToList();
                    //var izinler = item.Izinler.Where(s => (s.BaslangicTarihi.Value.Day >= gun && s.BaslangicTarihi.Value.Month >= ay && s.BaslangicTarihi.Value.Year >= yil )&& (s.BitisTarihi.Value.Day <= gun && s.BitisTarihi.Value.Month <= ay && s.BitisTarihi.Value.Year <= yil)).ToList();
                    if (item.Izinler.Any(s => s.BaslangicTarihi.Value.Date == day.Date))
                    {
                        Izinli = true;

                    }
                    if (Izinli)
                    {
                        kyt.GunSayisi++;
                        puantajControl = AylikPuantajModel.puantajKontrol.Izinli;
                        kyt.WorkTime.Add(new AylikPuantajModel { Gun = gun, PuantajControl = puantajControl });
                    }
                    else
                    {
                        if (harekets.Count > 0)
                        {
                            kyt.GunSayisi++;
                            puantajControl = AylikPuantajModel.puantajKontrol.Geldi;
                            kyt.WorkTime.Add(new AylikPuantajModel { Gun = gun, PuantajControl = puantajControl });
                        }
                        else
                        {

                            puantajControl = AylikPuantajModel.puantajKontrol.Gelmedi;
                            kyt.WorkTime.Add(new AylikPuantajModel { Gun = gun, PuantajControl = puantajControl });

                        }
                    }
                    if (item.Izinler.Any(s => s.BitisTarihi.Value.Date == day.Date))
                    {
                        Izinli = false;
                    }

                    //if (day.DayOfWeek == DayOfWeek.Sunday)
                    //{
                    //    kyt.Pazar = 1;
                    //}

                    puantajControl = 0;
                }
                kayit.Add(kyt);
            }

            string value = string.Empty;
            value = JsonConvert.SerializeObject(kayit, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
            //return View();
            
        }

        void CalculateWorkTimee(List<Hareketler> hareket, out double totalWorkTime, out double tatalNotWorkedTime)
        {
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
                TimeSpan ts = new TimeSpan(9, 0, 0);
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

                    if (i < kayitListe.Count)
                    {

                        DateTime normalTime = DateTime.Today;
                        TimeSpan ts = new TimeSpan(9, 0, 0);
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

            }
            totalWorkTime = kayitListe.Where(r => r.Operation == 1).Sum(r => r.TotalMinute);
            var customerTime = kayitListe.Where(r => r.Operation == 0 && r.Tipi.GetValueOrDefault()).Sum(r => r.TotalMinute);
            totalWorkTime += customerTime;
            tatalNotWorkedTime = kayitListe.Where(r => r.Operation == 0 && !r.Tipi.GetValueOrDefault()).Sum(r => r.TotalMinute);
        }

        [HttpGet]
        public JsonResult PersonelPuantaj(int id)
        {

            App_Data.KayitliPersonel kayit = db.KayitliPersonel.Where(s => s.KId == id).SingleOrDefault();

            string value = string.Empty;
            value = JsonConvert.SerializeObject(kayit, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            var json = Json(value, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        [HttpGet]
        public ActionResult PersonelIzinleri(int id, string donem)
        {
            DateTime d = Convert.ToDateTime(donem);
            int ay = d.Month;
            int yil = d.Year;
            List<App_Data.Izinler> kayit = db.Izinler.Where(s => s.KId == id && s.BaslangicTarihi.Value.Month == ay && s.BaslangicTarihi.Value.Year == yil).ToList();

            string value = string.Empty;
            value = JsonConvert.SerializeObject(kayit, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            var json = Json(value, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult PersonelAylikCalisma(string donem)
        {



            return View();
        }
        public ActionResult BugunGelenPersonel()
        {

            DateTime dt = DateTime.Now;
            int gun = dt.Day;
            int ay = dt.Month;
            int yil = dt.Year;

            var kayt = db.KayitliPersonel.ToList();
            List<MyModel> kayit = new List<MyModel>();
            int KisiSayisi = 0;
            foreach (var item in kayt)
            {
                var hareket = item.Hareketler.Where(s => s.Tarih.Value.Day == gun && s.Tarih.Value.Month == ay && s.Tarih.Value.Year == yil && s.KayitId == item.KId).FirstOrDefault();
                //kayit.Add(new MyModel { KisiSayisi = hareket });
            }

            return Json(kayit, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AylikGirisCikis(int id, string donem)
        {
            DateTime d = Convert.ToDateTime(donem);
            int ay = d.Month;
            int yil = d.Year;
            int days = DateTime.DaysInMonth(yil, ay);
            var KayitListesi = db.KayitliPersonel.Distinct().Where(s => s.Silindi == false || s.Silindi == null && s.KId == id).ToList();
            var kayit = new List<Personels>();


            foreach (var item in KayitListesi)
            {
                var kyt = new Personels
                {
                    PersonelGirisCikis = new List<PersonelGirisCikis>()
                };
                for (int i = 0; i < days; i++)
                {
                    DateTime day = d.AddDays(i);
                    int gun = day.Day;
                    int ayy = day.Month;
                    int yill = day.Year;
                    var harekets = item.Hareketler.Where(s => s.Tarih.Value.Day == gun && s.Tarih.Value.Month == ayy && s.Tarih.Value.Year == yill).ToList();

                    if (harekets.Count > 0)
                    {
                        var girisSaati = harekets.FirstOrDefault().Tarih;
                        var cikis = harekets.LastOrDefault().Tarih;
                        kyt.PersonelGirisCikis.Add(new PersonelGirisCikis { Tarih = day, GirisSaati = girisSaati, CikisSaati = cikis });
                    }
                    else
                    {
                        kyt.PersonelGirisCikis.Add(new PersonelGirisCikis { Tarih = day });
                    }
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


        public ActionResult MobileUsers()
        {
            if (Session["uyeOturum"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            List<App_Data.KayitliPersonel> MobilUser = db.KayitliPersonel.ToList();
            return View(MobilUser);
        }


        [HttpPost]
        public ActionResult MesaiBaslangicUpdate(App_Data.Ayarlar model)
        {

            App_Data.Ayarlar ayar = db.Ayarlar.SingleOrDefault(s => s.id == 3);

            ayar.Saats = model.Saats;
            db.SaveChanges();

            return Json("", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult MesaiBitisUpdate(App_Data.Ayarlar model)
        {

            App_Data.Ayarlar ayar = db.Ayarlar.SingleOrDefault(s => s.id == 4);

            ayar.Saats = model.Saats;
            db.SaveChanges();

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult MobileUser(int KId)
        {
            App_Data.KayitliPersonel model = db.KayitliPersonel.Where(x => x.KId == KId).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult UpdateMobileUser(App_Data.KayitliPersonel model)
        {
            var result = false;
            try
            {
                if (model.KId > 0)
                {
                    App_Data.KayitliPersonel kullanici = db.KayitliPersonel.SingleOrDefault(x => x.KId == model.KId);
                    kullanici.EPosta = model.EPosta;
                    kullanici.MobilePass = model.MobilePass;
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult QRCodeList()
        {
            if (Session["uyeOturum"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            if (Session["uyeTuru"].ToString() != "1")
            {
                Session.Abandon();
                return RedirectToAction("Index", "User");
            }
            return View();
        }

        public ActionResult AylikCalismaZamani()
        {
            if (Session["uyeOturum"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            List<App_Data.KayitliPersonel> model = db.KayitliPersonel.ToList();
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

                    CalculateWorkTime(harekets, out totalWorkTime, out tatalNotWorkedTime, Convert.ToString(day));
                    AylikTotal += totalWorkTime;
                    AylikMolaTotal += tatalNotWorkedTime;
                    kyt.CalismaZamani = Convert.ToString(TimeSpan.FromMinutes(totalWorkTime).ToString(@"hh\s\:mm\d\k"));
                    kyt.TotalAylikCalisma = Convert.ToString(TimeSpan.FromMinutes(AylikTotal).TotalHours);
                    kyt.TotalAylikMola = Convert.ToString(TimeSpan.FromMinutes(AylikMolaTotal).TotalHours);
                    kyt.MonthTime.Add(new ViewModel.PuantajModel { Gun = Convert.ToInt32(day.Date.ToString("dd")) , CalismaZamani = kyt.CalismaZamani});
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

        public ActionResult SalaryCalculator(int id , string donem, double Miktar)
        {

            DateTime d = Convert.ToDateTime(donem);
            double _maas = 0;
            double _netPrice = 0;
            _maas = Miktar;
            int ay = d.Month;
            int yil = d.Year;
            int days = DateTime.DaysInMonth(yil, ay);
            var KayitListesi = db.KayitliPersonel.Distinct().Where(s => s.Silindi == false || s.Silindi == null && s.KId == id).ToList();
            var Kayit = new List<SalaryCalculation>();

            foreach (var item in KayitListesi)
            {

                var kyt = new Tracking.Models.Models.PersonnelMonthlyInquiry.SalaryCalculation
                {
                    salaryCalculationLists = new List<Tracking.Models.Models.PersonnelMonthlyInquiry.SalaryCalculaterDetail>()
                };
                for (int i = 0; i < days; i++)
                {
                    DateTime day = d.AddDays(i);
                    _maas = Miktar;
                    int gun = day.Day;
                    int ayy = day.Month;
                    int yill = day.Year;
                    var harekets = item.Hareketler.Where(s => s.Tarih.Value.Day == gun && s.Tarih.Value.Month == ayy && s.Tarih.Value.Year == yill).ToList();

                    double totalWorkTime = 0;
                    double tatalNotWorkedTime = 0;
                    CalculateWorkTime(harekets, out totalWorkTime, out tatalNotWorkedTime, Convert.ToString(day));
                    TimeSpan CalismaSaati = TimeSpan.FromMinutes((double)totalWorkTime);
                    string saat = CalismaSaati.TotalHours.ToString("0") + "," + CalismaSaati.TotalMinutes.ToString("0");

                    _maas = _maas / days;
                    _maas = (_maas / Convert.ToDouble(saat));
                    _maas = (_maas * Convert.ToDouble(saat));
                    if (_maas == Double.NaN)
                    {
                        continue;
                    }
                    else
                    {
                        _netPrice += _maas;
                    }
                    

                    kyt.salaryCalculationLists.Add( new Tracking.Models.Models.PersonnelMonthlyInquiry.SalaryCalculaterDetail { NetPrice = _netPrice});


                    //kyt.CalismaZamani = TimeSpan.FromMinutes((double)totalWorkTime);
                }

                Kayit.Add(kyt);
            }


            string value = string.Empty;
            value = JsonConvert.SerializeObject(Kayit, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Help()
        {
            if (Session["uyeOturum"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            if (Session["uyeTuru"].ToString() != "1")
            {
                Session.Abandon();
                return RedirectToAction("Index", "User");
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Help(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("dogan@kod.com.tr"));  // replace with valid value 
                message.From = new MailAddress("dogan@kod.com.tr");  // replace with valid value
                message.Subject = "Your email subject";
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "dogan@kod.com.tr",  // replace with valid value
                        Password = "c4e128b141aFb"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp-mail.outlook.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    //return RedirectToAction("Sent");
                }
            }
            return View(model);
        }


        public ActionResult PersonelMusteri()
        {
            if (Session["uyeOturum"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            if (Session["uyeTuru"].ToString() != "1")
            {
                Session.Abandon();
                return RedirectToAction("Index", "User");
            }
            List<Hareketler> hareket = db.Hareketler.Where(x=>x.Tipi == true && x.Tarih.Value.Day == DateTime.Now.Day && x.Tarih.Value.Month == DateTime.Now.Month && x.Tarih.Value.Year == DateTime.Now.Year).ToList();
            return View(hareket);
        }


        [HttpGet]
        public ActionResult PersonelMusteriHistory(string date)
        {
            DateTime d = Convert.ToDateTime(date);
            List<HareketModel> kayitListe = db.Hareketler.Where(s => s.Tarih.Value.Day == d.Day && s.Tarih.Value.Month == d.Month && s.Tarih.Value.Year == d.Year && s.Tipi == true).Select(x => new HareketModel()
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

            string value = string.Empty;
            value = JsonConvert.SerializeObject(kayitListe, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Log()
        {
            if (Session["uyeOturum"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            if (Session["uyeTuru"].ToString() != "1")
            {
                Session.Abandon();
                return RedirectToAction("Index", "User");
            }
            List<Log> logKayitlari = db.Log.Where(x => x.Date.Value.Day == DateTime.Now.Day && x.Date.Value.Month == DateTime.Now.Month && x.Date.Value.Year == DateTime.Now.Year).ToList();

            return View(logKayitlari);
        }


        public ActionResult SednaFaceParametre()
        {
            return View();
        }


    }
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}