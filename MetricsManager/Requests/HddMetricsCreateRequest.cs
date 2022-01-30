namespace MetricsManager.Requests
{
    public class HddMetricCreateRequest
    {
        public TimeSpan Time { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return $"CpuMetricCreateRequest Time: {Time}, Value: {Value}";
        }
    }
}
