﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace naseNut.WebApi.Models.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class NaseNEntities : DbContext
    {
        public NaseNEntities()
            : base("name=NaseNEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<GrillIssue> GrillIssues { get; set; }
        public virtual DbSet<Humidity> Humidities { get; set; }
        public virtual DbSet<Sampling> Samplings { get; set; }
        public virtual DbSet<ReceptionEntry> ReceptionEntries { get; set; }
        public virtual DbSet<NutSize> NutSizes { get; set; }
        public virtual DbSet<Grill> Grills { get; set; }
        public virtual DbSet<Cylinder> Cylinders { get; set; }
        public virtual DbSet<Producer> Producers { get; set; }
        public virtual DbSet<Variety> Varieties { get; set; }
        public virtual DbSet<Batch> Batches { get; set; }
        public virtual DbSet<Field> Fields { get; set; }
        public virtual DbSet<Box> Boxes { get; set; }
        public virtual DbSet<Reception> Receptions { get; set; }
        public virtual DbSet<Remission> Remissions { get; set; }
        public virtual DbSet<NutType> NutTypes { get; set; }
    }
}
