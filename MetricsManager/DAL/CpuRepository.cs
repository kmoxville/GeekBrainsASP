using MetricsManager.Models;

namespace MetricsManager.DAL
{
    public class CpuRepository : GenericRepository<CpuMetric>, ICpuRepository
    {
        public CpuRepository(MetricsDbContext context) : base(context)
        {
            _dbSet = context.CpuMetrics;
        }
    }

    public interface ICpuRepository : IGenericRepository<CpuMetric>
    {

    }
}
