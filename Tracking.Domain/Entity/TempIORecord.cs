namespace SednaPersonel.DataAccess.SednaFaceContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("TempIORecord")]
    public partial class TempIORecord
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

        public bool? UnSend { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? OperationDate { get; set; }

        public string Remark { get; set; }
    }
}
