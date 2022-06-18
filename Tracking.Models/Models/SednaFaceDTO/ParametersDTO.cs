using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SednaPersonel.Models.Models.SednaFaceDTO
{
    public class ParametersDTO
    {
        public int RecId { get; set; }

        public bool? LogDelete { get; set; }

        public bool? PingAttach { get; set; }

        public string FilePath { get; set; }

        public string BacUpPath { get; set; }

        public int? ServiceBegin { get; set; }

        public int? ServiceEnd { get; set; }

        public TimeSpan? ShiftBegin { get; set; }

        public TimeSpan? ShiftEnd { get; set; }

        public bool? ShiftComplate { get; set; }
    }
}
