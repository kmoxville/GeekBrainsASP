using MetricsManager.Models;

namespace MetricsManager.DAL
{
    public class AgentsRepository : GenericRepository<AgentInfo>, IAgentsRepository
    {
        public AgentsRepository(MetricsDbContext context) : base(context)
        {
            _dbSet = context.AgentInfos;
        }
    }

    public interface IAgentsRepository : IGenericRepository<AgentInfo>
    {

    }
}
