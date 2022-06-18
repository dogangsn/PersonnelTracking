namespace SednaPersonel.DataAccess.SednaFaceContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("RecordPersonel")]
    public partial class RecordPersonel
    {
        [Key]
        public int RecId { get; set; }

        public int? PersonelId { get; set; }

        [StringLength(50)]
        public string PerAdi { get; set; }

        [StringLength(50)]
        public string PerGrup { get; set; }

        public int? PerYetki { get; set; }

        public string paImData0 { get; set; }

        public string paImData1 { get; set; }

        public string photo { get; set; }

        public string faceData { get; set; }

        public string cardData { get; set; }

        public string pwdData { get; set; }

        [StringLength(50)]
        public string cardStr { get; set; }

        [StringLength(50)]
        public string pwdStr { get; set; }

        public int? enableFlag { get; set; }

        public int? mnCurPass { get; set; }

        public bool? Sec { get; set; }

        public string FP0 { get; set; }

        public int? IKId { get; set; }
    }
}
