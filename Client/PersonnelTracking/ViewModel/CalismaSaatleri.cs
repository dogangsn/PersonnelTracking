using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonnelTracking.ViewModel
{
    public class CalismaSaatleri
    {
        public int Dakika { get; set; }
        public String Sure { get;  set;}

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Hareketler> Hareketler { get; set; }
    }
}