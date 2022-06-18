using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SednaPersonel.Models.Models.Entity
{
    public class KayitliPersonelDTO
    {
        public int KId { get; set; }
        public string KartID { get; set; }
        public string PAdSoyad { get; set; }
        public Nullable<System.DateTime> DogumTarihi { get; set; }
        public string EPosta { get; set; }
        public string CepNum { get; set; }
        public string Adres { get; set; }
        public string FotoYol { get; set; }
        public Nullable<int> FingerID { get; set; }
        public Nullable<System.DateTime> IseBaslamaTarihi { get; set; }
        public Nullable<bool> Silindi { get; set; }
        public Nullable<int> DepartmanID { get; set; }
        public string QRPath { get; set; }
        public string MobilePass { get; set; }

        //DetailsUpdate-KodYazilim
        public HttpPostedFileBase Foto { get; set; }

    }
}
