using MetricsManager.Models;

namespace MetricsManager.DAL
{
    public class RamRepository : GenericRepository<RamMetric>, IRamRepository
    {
        public RamRepository(MetricsDbContext context) : base(context)
        {
            _dbSet = context.RamMetrics;
        }
    }

    public interface IRamRepository : IGenericRepository<RamMetric>
    {

    }
}
