using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonnelTracking.MobileModel
{
    public class Personel
    {

        public string Token { get; set; }

        public int KId { get; set; }
        public string PAdSoyad { get; set; }
        public string EPosta { get; set; }
        public string CepNum { get; set; }
        public string Adres { get; set; }
        public string FotoYol { get; set; }
        public Nullable<System.DateTime> IseBaslamaTarihi { get; set; }
        public Nullable<int> DepartmanID { get; set; }
        public string QRPath { get; set; }
        public double CalismaSaati { get; set; }
        public double MolaSaati { get; set; }

        public string Durumu { get; set; }

        public string GirisSaati { get; set; }

        public string CikisSaati { get; set; }
    }
}