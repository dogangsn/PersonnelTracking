using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kod_Yazilim_Personel.ViewModel
{
    public class KayitDurumu
    {

        public int Id { get; set; }
        public string KartID { get; set; }
        public string PAdSoyad { get; set; }

        public string Islem { get; set; }

        public Nullable<bool> Tipi { get; set; }


    }
}