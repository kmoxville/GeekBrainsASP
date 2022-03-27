namespace MetricsManager.Responses
{
    public class AgentsGetAllResponse
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<AgentInfoDto> Agents { get; set; } = new List<AgentInfoDto>();
    }

    public class AgentInfoDto
    {
        public int Id { get; set; }
        public Uri Uri { get; set; } = null!;
        public bool Enabled { get; set; }
    }
}
