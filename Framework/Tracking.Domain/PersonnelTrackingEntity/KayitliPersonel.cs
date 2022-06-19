namespace Tracking.Domain.PersonnelTrackingEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("dogantestdb.KayitliPersonel")]
    public partial class KayitliPersonel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KayitliPersonel()
        {
            Izinler = new HashSet<Izinler>();
            Hareketler = new HashSet<Hareketler>();
            Notlar = new HashSet<Notlar>();
        }

        [Key]
        public int KId { get; set; }

        [StringLength(50)]
        public string KartID { get; set; }

        [StringLength(50)]
        public string PAdSoyad { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DogumTarihi { get; set; }

        [StringLength(50)]
        public string EPosta { get; set; }

        [StringLength(50)]
        public string CepNum { get; set; }

        public string Adres { get; set; }

        [Column(TypeName = "date")]
        public DateTime? IseBaslamaTarihi { get; set; }

        [StringLength(100)]
        public string FotoYol { get; set; }

        public int? FingerID { get; set; }

        public int? DepartmanID { get; set; }

        public bool? Silindi { get; set; }

        public string QRPath { get; set; }

        [StringLength(50)]
        public string MobilePass { get; set; }

        public virtual Departman Departman { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Izinler> Izinler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Hareketler> Hareketler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Notlar> Notlar { get; set; }
    }
}
