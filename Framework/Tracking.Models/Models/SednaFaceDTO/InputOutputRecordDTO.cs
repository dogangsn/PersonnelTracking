using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracking.Models.Models.SednaFaceDTO
{
    public class InputOutputRecordDTO
    {
        public int RecId { get; set; }

        public int? CihazNo { get; set; }

        public int? UserId { get; set; }

        public DateTime? Tarih { get; set; }

        public string Readingtype { get; set; }

        public int? Operation { get; set; }

        public bool? Creator { get; set; }
    }
}
