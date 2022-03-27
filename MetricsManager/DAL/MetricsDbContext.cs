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
        public DbSet<AgentInfo> AgentInfos { get; set; } = null!;
    }
}
