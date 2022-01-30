namespace MetricsManager.Requests
{
    public class RamMetricCreateRequest
    {
        public TimeSpan Time { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return $"CpuMetricCreateRequest Time: {Time}, Value: {Value}";
        }
    }
}
