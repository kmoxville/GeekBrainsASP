namespace MetricsManager.Models
{
    public class AgentInfo
    {
        public int Id { get; set; }
        public Uri Uri { get; set; } = null!;
        public bool Enabled { get; set; }
    }
}
