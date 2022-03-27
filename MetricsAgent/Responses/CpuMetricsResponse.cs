namespace MetricsManager.Responses
{
    public class CpuMetricsResponse
    {
        public List<CpuMetricDto> Metrics { get; } = new List<CpuMetricDto>();
    }

    public class CpuMetricDto
    {
        public DateTime Time { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }
    }
}
