namespace Tracking.Domain.PersonnelTrackingEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Login")]
    public partial class Login
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string KAdi { get; set; }

        [StringLength(50)]
        public string Sifre { get; set; }

        public int? uyeTuru { get; set; }
    }
}
