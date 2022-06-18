namespace SednaPersonel.Domain.KodPersonelEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("dogantestdb.Hareketler")]
    public partial class Hareketler
    {
        public int Id { get; set; }

        public int? KayitId { get; set; }

        public DateTime? Tarih { get; set; }

        [StringLength(10)]
        public string Islem { get; set; }

        public int? operation { get; set; }

        public bool? Tipi { get; set; }

        public virtual KayitliPersonel KayitliPersonel { get; set; }
    }
}
