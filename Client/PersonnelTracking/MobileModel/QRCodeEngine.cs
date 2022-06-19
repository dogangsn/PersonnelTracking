using DocumentFormat.OpenXml.Office2013.Excel;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace PersonnelTracking.MobileModel
{
    public class QRCodeEngine
    {

        public string generateQRCode(string link)
        {
            var linkWithoutIllegalChar = link.Replace("?", "").Replace("/", "").Replace("\\", "").Replace(":", "").Replace("#", "");
            string filePath = System.Web.HttpContext.Current.Server.MapPath("~/Content/PersonelFoto/Temps/QRCodes/");

            if (!File.Exists(filePath + "/" + linkWithoutIllegalChar + ".png"))
            {

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                var qrCodeImage = qrCode.GetGraphic(20);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                ImageConverter converter = new ImageConverter();
                //var bitmap = Bitmap.FromStream((byte[])converter.ConvertTo(qrCodeImage, typeof(byte[])));
                //ImageSharp.Image image = new ImageSharp.Image((byte[])converter.ConvertTo(qrCodeImage, typeof(byte[])));

                qrCodeImage.Save(filePath + "/" + linkWithoutIllegalChar + ".png", ImageFormat.Png);
            }
            return "/Temps/QRCodes/" + linkWithoutIllegalChar + ".png";
        }


    }
}