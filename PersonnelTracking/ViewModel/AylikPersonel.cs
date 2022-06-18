using Kod_Yazilim_Personel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kod_Yazilim_Personel.ViewModel
{
    public class AylikPersonel
    {
        public string PAdSoyad { get; set; }

        public int GunSayisi { get; set; }

        public List<AylikPuantajModel> WorkTime { get; set; }

        public int? Pazar { get; set; }

    }
}