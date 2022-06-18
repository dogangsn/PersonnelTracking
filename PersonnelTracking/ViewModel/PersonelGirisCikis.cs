using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kod_Yazilim_Personel.ViewModel
{
    public class PersonelGirisCikis
    {
        public DateTime Tarih { get; set; }

        public DateTime? GirisSaati { get; set; }

        public DateTime? CikisSaati { get; set; }
    }
}