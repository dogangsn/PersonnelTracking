using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracking.Models.Models.Entity
{
   public  class LoginDTO
    {
        public int Id { get; set; }
        public string KAdi { get; set; }
        public string Sifre { get; set; }
        public Nullable<int> uyeTuru { get; set; }
    }
}
