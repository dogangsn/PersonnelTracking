namespace Tracking.Domain.PersonnelTrackingEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ayarlar")]
    public partial class Ayarlar
    {
        public int id { get; set; }

        [StringLength(50)]
        public string Value { get; set; }

        public bool? Key { get; set; }
    }
}
