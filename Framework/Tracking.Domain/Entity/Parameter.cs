namespace Tracking.DataAccess.SednaFaceContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("Parameter")]
    public partial class Parameter
    {
        [Key]
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
