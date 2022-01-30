using MetricsManager.Models;

namespace MetricsManager.DAL
{
    public class NetworkRepository : GenericRepository<NetworkMetric>, INetworkRepository
    {
        public NetworkRepository(MetricsDbContext context) : base(context)
        {
            _dbSet = context.NetworkMetrics;
        }
    }

    public interface INetworkRepository : IGenericRepository<NetworkMetric>
    {

    }
}
