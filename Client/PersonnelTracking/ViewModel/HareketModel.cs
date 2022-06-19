using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonnelTracking.ViewModel
{
    public class HareketModel
    {
        public int Id { get; set; }
        public string KartID { get; set; }
        public string PAdSoyad { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Tarih { get; set; }
        public int gun { get; set; }
        public int ay { get; set; }
        public int yil { get; set; }
        public int saat { get; set; }
        public int dakika { get; set; }
        public string Islem { get; set; }
        public string TotalCalisma { get; set; } 
        public string TotalMola { get; set; }
        public string Notu { get; set; }
        public int? Operation { get; internal set; }
        public double TotalMinute { get; internal set; }

        public Nullable<bool> SehirDisi { get; set; }

        public double? chartCalismaSaati { get; set; }

        public double? charMolaSaati { get; set; }
        public string MusteriBİlgisi { get; set; }

        public Nullable<bool> Tipi { get; set; }
    }
}