namespace naseNut.WebApi.Models.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model : DbContext
    {
        public Model()
            : base("name=NaseNEntities")
        {
        }

        public virtual DbSet<Cylinder> Cylinders { get; set; }
        public virtual DbSet<Grill> Grills { get; set; }
        public virtual DbSet<GrillIssue> GrillIssues { get; set; }
        public virtual DbSet<Humidity> Humidities { get; set; }
        public virtual DbSet<NutType> NutTypes { get; set; }
        public virtual DbSet<Producer> Producers { get; set; }
        public virtual DbSet<Reception> Receptions { get; set; }
        public virtual DbSet<ReceptionEntry> ReceptionEntries { get; set; }
        public virtual DbSet<Remission> Remissions { get; set; }
        public virtual DbSet<Sampling> Samplings { get; set; }
        public virtual DbSet<Variety> Varieties { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Grill>()
                .HasMany(e => e.Receptions)
                .WithMany(e => e.Grills)
                .Map(m => m.ToTable("Reception_Grill").MapLeftKey("grillId").MapRightKey("receptionId"));

            modelBuilder.Entity<GrillIssue>()
                .HasMany(e => e.Grills)
                .WithOptional(e => e.GrillIssue)
                .HasForeignKey(e => e.GrillIssuesId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ReceptionEntry>()
                .HasOptional(e => e.Cylinder)
                .WithRequired(e => e.ReceptionEntry);

            modelBuilder.Entity<ReceptionEntry>()
                .HasMany(e => e.Humidities)
                .WithRequired(e => e.ReceptionEntry)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReceptionEntry>()
                .HasMany(e => e.Receptions)
                .WithRequired(e => e.ReceptionEntry)
                .WillCascadeOnDelete(false);
        }
    }
}
