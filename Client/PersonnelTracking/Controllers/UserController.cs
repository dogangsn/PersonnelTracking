using PersonnelTracking.App_Data;
using PersonnelTracking.ViewModel;
using Microsoft.IdentityModel.Tokens;
using Tracking.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;



namespace PersonnelTracking.Controllers
{
    public class UserController : Controller
    {
        PersonnelTrackingDb db = new PersonnelTrackingDb();
        Tracking.Services.Services.CryptographyService _Cryptography;

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginDTO model, string returnUrl)
        {

            string IPAddress = "";
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = IP.ToString();
                }
            }

            if (model.Sifre == null)
            {
                ViewBag.hata = "Kullanıcı Adı Veya Parola Geçersiz";
                return View();
            }

            _Cryptography = new Tracking.Services.Services.CryptographyService();
            string CryptographyFind = _Cryptography.ConvertStringToMD5(model.Sifre);
            Login uye = db.Login.Where(m => m.KAdi == model.KAdi && m.Sifre == CryptographyFind).SingleOrDefault();

            #region Token
            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(WebConfigurationManager.AppSettings["Token"]);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new Claim[]
            //    {
            //            new Claim(ClaimTypes.NameIdentifier, uye.KAdi.ToString()),
            //           // new Claim(ClaimTypes.Name,uye.MobilePass)
            //    }),
            //    Expires = DateTime.Now.AddDays(1),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            //};

            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //var tokenString = tokenHandler.WriteToken(token);
            #endregion

            if (uye != null)
            {               
             
                //Session["Token"] = tokenString;
                Log _log = new Log();
                _log.IpAdress = IPAddress;
                _log.PcName = Hostname;
                _log.MacAdress = "";
                _log.PageName = "User";
                _log.Operation = "Giris";
                _log.Date = DateTime.Now;
                _log.UserName = uye.KAdi;
                db.Log.Add(_log);
                db.SaveChanges();
                Session["uyeOturum"] = true;
                Session["uyeKadi"] = uye.KAdi;
                Session["uyeTuru"] = uye.uyeTuru;
                if (returnUrl == null)
                {
                    return RedirectToAction(actionName: "PersonelGirisCikis", controllerName:"KodYazilim");
                } else
                {
                    return Redirect(returnUrl);
                }
               

            }
            else {
                ViewBag.hata = "Kullanıcı Adı Veya Parola Geçersiz";
                return View();
            }

        }
        public ActionResult OturumKapat(string returnUrl)
        {
            Session.Abandon();
            return Redirect(returnUrl);

        }

    }
}