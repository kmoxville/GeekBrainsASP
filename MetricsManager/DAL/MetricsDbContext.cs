using MetricsManager.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace MetricsManager.DAL
{
    public class MetricsDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Metrics.db");
        }

        public DbSet<CpuMetric> CpuMetrics { get; set; } = null!;
        public DbSet<HddMetric> HddMetrics { get; set; } = null!;
        public DbSet<NetworkMetric> NetworkMetrics { get; set; } = null!;
        public DbSet<RamMetric> RamMetrics { get; set; } = null!;
    }
}
