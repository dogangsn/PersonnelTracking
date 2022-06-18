using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SednaPersonel.Models.Models.Entity
{
    public class HareketlerDTO
    {
        public int Id { get; set; }
        public Nullable<int> KayitId { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        public string Islem { get; set; }
        public Nullable<int> operation { get; set; }
        public Nullable<bool> Tipi { get; set; }
        public Nullable<bool> SehirDisi { get; set; }

    }
}
