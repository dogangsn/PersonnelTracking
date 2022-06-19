namespace Tracking.DataAccess.SednaFaceContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("InputOutputRecord")]
    public partial class InputOutputRecord
    {
        [Key]
        public int RecId { get; set; }

        public int? CihazNo { get; set; }

        public int? UserId { get; set; }

        public DateTime? Tarih { get; set; }

        [StringLength(50)]
        public string Readingtype { get; set; }

        public int? Operation { get; set; }

        public bool? Creator { get; set; }
    }
}
