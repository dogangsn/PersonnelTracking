//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PersonnelTracking.App_Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class PersonnelTrackingDb : DbContext
    {
        public PersonnelTrackingDb()
            : base("name=PersonnelTrackingDb")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<Login> Login { get; set; }
        public virtual DbSet<Hareketler> Hareketler { get; set; }
        public virtual DbSet<KayitliPersonel> KayitliPersonel { get; set; }
        public virtual DbSet<Notlar> Notlar { get; set; }
        public virtual DbSet<Izinler> Izinler { get; set; }
        public virtual DbSet<Departman> Departman { get; set; }
        public virtual DbSet<Ayarlar> Ayarlar { get; set; }
        public virtual DbSet<Log> Log { get; set; }
    }
}
