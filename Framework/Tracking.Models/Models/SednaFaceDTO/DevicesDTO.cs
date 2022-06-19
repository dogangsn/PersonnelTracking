using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracking.Models.Models.SednaFaceDTO
{
    public class DevicesDTO
    {
        public int RecId { get; set; }

        public string DeviceName { get; set; }

        public int? DeviceId { get; set; }

        public string DeviceIP { get; set; }

        public int? DevicePort { get; set; }

        public string DevicePassword { get; set; }

        public int? Check { get; set; }

        public int? DeviceRankId { get; set; }

        public string DeviceModel { get; set; }

        public int? TimeOut { get; set; }
    }
}
