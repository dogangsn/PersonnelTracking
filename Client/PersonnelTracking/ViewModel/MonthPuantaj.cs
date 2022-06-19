using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonnelTracking.ViewModel
{
    public class MonthPuantaj
    {

        public string PAdSoyad { get; set; }

        public string CalismaZamani { get; set; }

        public string TotalAylikCalisma { get; set; }

        public string TotalAylikMola { get; set; }

        public List<HareketModel> Hareketler { get; set; }

        public List<PuantajModel> MonthTime { get; set; }
    }
}