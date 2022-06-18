using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kod_Yazilim_Personel.Models
{
    public class AylikPuantajModel
    {

        public int? Gun { get; set; }
        public puantajKontrol? PuantajControl { get; set; }


        public enum puantajKontrol
        {
            Geldi = 0,
            Gelmedi = 1,
            Izinli = 2
        }
    }
}