using MarketingBox.Reporting.Service.Postgres.ReadModels.Leads;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MarketingBox.Reporting.Service.Postgres
{
    public class DatabaseContext : DbContext
    {
        private static readonly JsonSerializerSettings JsonSerializingSettings =
            new() { NullValueHandling = NullValueHandling.Ignore };

        public const string Schema = "reposrting-service";

        private const string LeadTableName = "leads";

        public DbSet<Lead> Leads { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }
        public static ILoggerFactory LoggerFactory { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (LoggerFactory != null)
            {
                optionsBuilder.UseLoggerFactory(LoggerFactory).EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);

            SetLeadReadModel(modelBuilder);
           
            base.OnModelCreating(modelBuilder);
        }

        private void SetLeadReadModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lead>().ToTable(LeadTableName);
            modelBuilder.Entity<Lead>().HasKey(e => e.LeadId);
            modelBuilder.Entity<Lead>().OwnsOne(x => x.BrandInfo);
            modelBuilder.Entity<Lead>().OwnsOne(x => x.AdditionalInfo);
            modelBuilder.Entity<Lead>().HasIndex(e => new { e.TenantId, e.LeadId });
            
            //TODO: This IS NOT SUPPORTED BY EF BUT IT IS WRITTEN IN MIGRATION
            // 
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
