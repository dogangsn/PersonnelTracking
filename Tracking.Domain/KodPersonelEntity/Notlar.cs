namespace SednaPersonel.Domain.KodPersonelEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("dogantestdb.Notlar")]
    public partial class Notlar
    {
        public int id { get; set; }

        public int KayitId { get; set; }

        public DateTime? Tarih { get; set; }

        public string Not { get; set; }

        public virtual KayitliPersonel KayitliPersonel { get; set; }
    }
}
