namespace SednaPersonel.Domain.KodPersonelEntity
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=KodPersonelDbContext")
        {
        }

        public virtual DbSet<Ayarlar> Ayarlar { get; set; }
        public virtual DbSet<Departman> Departman { get; set; }
        public virtual DbSet<Izinler> Izinler { get; set; }
        public virtual DbSet<Login> Login { get; set; }
        //public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Hareketler> Hareketler { get; set; }
        public virtual DbSet<KayitliPersonel> KayitliPersonel { get; set; }
        public virtual DbSet<Notlar> Notlar { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Departman>()
                .Property(e => e.DepartmanName)
                .IsFixedLength();

            modelBuilder.Entity<Departman>()
                .HasOptional(e => e.Departman1)
                .WithRequired(e => e.Departman2);

            modelBuilder.Entity<Departman>()
                .HasMany(e => e.KayitliPersonel)
                .WithOptional(e => e.Departman)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Hareketler>()
                .Property(e => e.Islem)
                .IsFixedLength();

            modelBuilder.Entity<KayitliPersonel>()
                .Property(e => e.FotoYol)
                .IsFixedLength();

            modelBuilder.Entity<KayitliPersonel>()
                .HasMany(e => e.Izinler)
                .WithOptional(e => e.KayitliPersonel)
                .WillCascadeOnDelete();

            modelBuilder.Entity<KayitliPersonel>()
                .HasMany(e => e.Hareketler)
                .WithOptional(e => e.KayitliPersonel)
                .HasForeignKey(e => e.KayitId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<KayitliPersonel>()
                .HasMany(e => e.Notlar)
                .WithRequired(e => e.KayitliPersonel)
                .HasForeignKey(e => e.KayitId);

            modelBuilder.Entity<Notlar>()
                .Property(e => e.Not)
                .IsUnicode(false);
        }
    }
}
