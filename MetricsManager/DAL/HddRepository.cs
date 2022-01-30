using MetricsManager.Models;

namespace MetricsManager.DAL
{
    public class HddRepository : GenericRepository<HddMetric>, IHddRepository
    {
        public HddRepository(MetricsDbContext context) : base(context)
        {
            _dbSet = context.HddMetrics;
        }
    }

    public interface IHddRepository : IGenericRepository<HddMetric>
    {

    }
}
