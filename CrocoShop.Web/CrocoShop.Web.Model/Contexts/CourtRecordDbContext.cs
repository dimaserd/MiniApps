using CrocoShop.Web.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrocoShop.Web.Model.Contexts
{
    public class CourtRecordDbContext : DbContext
    {
        public CourtRecordDbContext(DbContextOptions<CourtRecordDbContext> options) : base(options)
        {
        }

        public DbSet<CourtRecord> CourtRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourtRecord>()
                .OwnsOne(p => p.Tenant);

            modelBuilder.Entity<CourtRecord>()
                .OwnsOne(p => p.Court);

            base.OnModelCreating(modelBuilder);
        }
    }
}