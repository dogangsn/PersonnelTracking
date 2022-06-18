namespace SednaPersonel.Domain.KodPersonelEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Izinler")]
    public partial class Izinler
    {
        public int id { get; set; }

        public int? KId { get; set; }

        public DateTime? BaslangicTarihi { get; set; }

        public DateTime? BitisTarihi { get; set; }

        [StringLength(50)]
        public string IzinTuru { get; set; }

        public string Aciklama { get; set; }

        public virtual KayitliPersonel KayitliPersonel { get; set; }
    }
}
