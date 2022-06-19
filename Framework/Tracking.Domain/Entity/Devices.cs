namespace Tracking.DataAccess.SednaFaceContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    public partial class Devices
    {
        [Key]
        public int RecId { get; set; }

        public string DeviceName { get; set; }

        public int? DeviceId { get; set; }

        [StringLength(50)]
        public string DeviceIP { get; set; }

        public int? DevicePort { get; set; }

        [StringLength(50)]
        public string DevicePassword { get; set; }

        public int? Check { get; set; }

        public int? DeviceRankId { get; set; }

        [StringLength(50)]
        public string DeviceModel { get; set; }

        public int? TimeOut { get; set; }
    }
}
