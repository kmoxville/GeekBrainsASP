using AutoMapper;
using MetricsManager.Models;
using MetricsManager.Responses;

namespace MetricsManager.DAL
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<CpuMetric, CpuMetricDto>();
        }
    }
}
