namespace GATShipD365TS.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using GATShipD365TS.Models;

    public partial class WIS_Sync : DbContext
    {
        public WIS_Sync()
            : base("name=WIS_Sync")
        {
        }

        
        
        public virtual DbSet<a3SMCMapping> a3SMCMappings { get; set; }
        
        public virtual DbSet<Registry> Registries { get; set; }                            
        public virtual DbSet<a3EventJournal> a3EventJournals { get; set; }
        public virtual DbSet<Staff> Staffs { get; set; }
        public virtual DbSet<a3NominationType> a3NominationTypes { get; set; }      
        public virtual DbSet<a3DAComInvoiceSuffix> a3DAComInvoiceSuffixes { get; set; }
        public virtual DbSet<a3InvoiceSuffix> a3InvoiceSuffixes { get; set; }       
        public virtual DbSet<a3EventStage> a3EventStage { get; set; }

        public virtual DbSet<ErrorRegistry> ErrorRegistries { get; set; }
        public virtual DbSet<GSRecExported> GSRecExporteds { get; set; }
        public virtual DbSet<GATShipSupportList> GATShipSupportLists { get; set; }

        public virtual DbSet<VoyageCodeControl> VoyageCodeControls { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<a3EventStage>()
                .Property(e => e.data)
                .IsUnicode(false);

            modelBuilder.Entity<a3EventStage>()
                .Property(e => e.received_at)
                .HasPrecision(6);

            modelBuilder.Entity<a3EventStage>()
                .Property(e => e.a3LocCode)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
