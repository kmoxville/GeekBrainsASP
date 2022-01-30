namespace MetricsManager.Requests
{
    public class CpuMetricCreateRequest
    {
        public TimeSpan Time { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return $"CpuMetricCreateRequest Time: {Time}, Value: {Value}";
        }
    }
}
