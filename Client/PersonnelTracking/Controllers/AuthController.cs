using PersonnelTracking.App_Data;
using PersonnelTracking.MobileModel;
using PersonnelTracking.ViewModel;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Tracking.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace PersonnelTracking.Controllers
{

    public class AuthController : Controller
    {
        PersonnelTrackingDb db = new PersonnelTrackingDb();

        [HttpPost]
        public ActionResult Login(LoginDTO model)
        {
            App_Data.KayitliPersonel uye = db.KayitliPersonel.Where(m => m.EPosta == model.KAdi && m.MobilePass == model.Sifre).SingleOrDefault();
            //App_Data.KayitliPersonel uye = db.KayitliPersonel.Where(m => m.KId == model.Id).SingleOrDefault();

            if (uye == null)
            {
                Response.StatusCode = 401;
                return Json("", JsonRequestBehavior.AllowGet);
                //return HttpUnauthorizedResult()
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(WebConfigurationManager.AppSettings["Token"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.NameIdentifier,uye.KId.ToString()),
                       // new Claim(ClaimTypes.Name,uye.MobilePass)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            string Islems = "";
            double totalWorkTime = 0;
            double totalNotWorkingTime = 0;
            CalculateWorkTimee(uye.Hareketler.Where(s => s.Tarih.Value.Date == DateTime.Today).ToList(), out totalWorkTime, out totalNotWorkingTime);
            //PersonelDurumu(hareket, Islems);
            try
            {
                var hareket = uye.Hareketler.Where(s => s.Tarih.Value.Date == DateTime.Today && s.KayitId == uye.KId).ToList();
                if (hareket == null)
                {
                    Islems = "GELMEDİ";
                }
                if (hareket.Count == 0)
                {
                    Islems = "GELMEDİ";
                }

                int? durum = hareket.OrderByDescending(p => p.Tarih).ToList().FirstOrDefault().operation;
                Boolean? tip = hareket.OrderByDescending(p => p.Tarih).ToList().FirstOrDefault().Tipi;
                if ((durum ?? 1) == 0)
                {

                    Islems = "İÇERDE";
                }
                else if ((durum ?? 0) == 1 && tip == true)
                {
                    Islems = "MÜŞTERİDE";
                }
                else
                {
                    Islems = "DIŞARDA";
                }
            }
            catch
            {
            }


            string girisSaat = uye.Hareketler.Where(s => s.Tarih.Value.Date == DateTime.Today && s.Islem == "Giris     ").FirstOrDefault().Tarih.ToString();
            string cikisSaat = uye.Hareketler.OrderByDescending(p => p.Tarih).Where(p => p.Tarih.Value.Date == DateTime.Today && p.Islem == "Cikis     ").FirstOrDefault().Tarih.ToString();

            return Json(new Personel
            {
                Token = tokenString,
                KId = uye.KId,
                PAdSoyad = uye.PAdSoyad,
                EPosta = uye.EPosta,
                Adres = uye.Adres,
                DepartmanID = uye.DepartmanID,
                CepNum = uye.CepNum,
                QRPath = uye.QRPath,
                FotoYol = uye.FotoYol,
                IseBaslamaTarihi = uye.IseBaslamaTarihi,
                CalismaSaati = totalWorkTime,
                MolaSaati = totalNotWorkingTime,
                Durumu = Islems,
                GirisSaati = girisSaat,
                CikisSaati = cikisSaat
            },JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PersonelListe()
        {
            List<PersonelListesi> prs = new List<PersonelListesi>();
            var personeller = db.KayitliPersonel.Where(s => s.Silindi == false || s.Silindi == null).Select(s => new
            {
                KartID = s.KartID,
                PAdSoyad = s.PAdSoyad
            }).ToList();

            foreach (var item in personeller)
            {
                prs.Add(new PersonelListesi { KartId = item.KartID, Name = item.PAdSoyad });
            }
            return Json(prs, JsonRequestBehavior.AllowGet);
        }

        void CalculateWorkTimee(List<Hareketler> hareket, out double totalWorkTime, out double tatalNotWorkedTime)
        {
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



    }

}