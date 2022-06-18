namespace SednaPersonel.Domain.KodPersonelEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Departman")]
    public partial class Departman
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Departman()
        {
            KayitliPersonel = new HashSet<KayitliPersonel>();
        }

        public int DepartmanID { get; set; }

        [StringLength(20)]
        public string DepartmanName { get; set; }

        public virtual Departman Departman1 { get; set; }

        public virtual Departman Departman2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KayitliPersonel> KayitliPersonel { get; set; }
    }
}
