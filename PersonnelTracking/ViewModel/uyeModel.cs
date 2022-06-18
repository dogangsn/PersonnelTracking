using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kod_Yazilim_Personel.ViewModel
{
    //public class uyeModel
    //{
    //    [Required(ErrorMessage = "Kullanıcı Adı Giriniz!")]
    //    [Display(Name = "Kullanıcı Adı")]
    //    public string KAdi { get; set; }

    //    [Required(ErrorMessage = "Parola Giriniz!")]
    //    [Display(Name = "Parola")]
    //    public string Sifre { get; set; }

    //    public int KId { get; set; }
    //}

    public class KayitliPersonelModel
    {
        public KayitliPersonelModel()
        {
            Hareketler = new List<HareketModel>();
        }
        public int KId { get; set; }
        public string KartID { get; set; }
        public string PAdSoyad { get; set; }
        public Nullable<System.DateTime> DogumTarihi { get; set; }
        public string EPosta { get; set; }
        public string CepNum { get; set; }
        public string Adres { get; set; }
        public string TotalCalisma { get; set; }
        public string TotalMola { get; set; }
        public string Islem { get; set; }
        public List<HareketModel> Hareketler { get; set; }
       
        public string Note { get; internal set; }
        public string MusteriBİlgisi { get; set; }
    }
}