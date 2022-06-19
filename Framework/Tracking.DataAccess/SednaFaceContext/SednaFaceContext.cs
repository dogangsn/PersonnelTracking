namespace Tracking.DataAccess.SednaFaceContext
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SednaFaceContext : DbContext
    {
        public SednaFaceContext()
            : base("name=SednaFaceContext")
        {
        }

        public virtual DbSet<Devices> Devices { get; set; }
        public virtual DbSet<InputOutputRecord> InputOutputRecord { get; set; }
        public virtual DbSet<Parameter> Parameter { get; set; }
        public virtual DbSet<RecordPersonel> RecordPersonel { get; set; }
        public virtual DbSet<TempIORecord> TempIORecord { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
