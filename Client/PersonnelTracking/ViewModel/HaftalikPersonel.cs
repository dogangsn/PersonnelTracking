using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonnelTracking.ViewModel
{
    public class HaftalikPersonel
    {

        public HaftalikPersonel()
        {
            Hareketler = new List<HareketModel>();
        }
        public string PAdSoyad { get; set; }

        public string PazartesiCalisma { get; set; }

        public string HaftalikTotal { get; set; }

        public string GunlukTotalCalisma { get; set; }

        public string IzinSaati { get; set; }

        public DateTime BaslangicTarihi { get; set; }

        public DateTime BitisTarihi { get; set; }

        public List<HareketModel> Hareketler { get; set; }

        public Dictionary<string, string> WorkTime { get; set; }

    }
}