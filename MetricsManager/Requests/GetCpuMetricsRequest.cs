using MetricsManager.Models;

namespace MetricsManager.Requests
{
    public class GetCpuMetricsRequest
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Uri Uri { get; set; } = default!;
    }
}
