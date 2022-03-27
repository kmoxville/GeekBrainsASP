using MetricsManager.Requests;
using MetricsManager.Responses;
using Newtonsoft.Json;

namespace MetricsManager.MetricsAgentClient
{
    public class MetricsAgentClient : IMetricsAgentClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MetricsAgentClient> _logger;

        public MetricsAgentClient(HttpClient httpClient, ILogger<MetricsAgentClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public CpuMetricsResponse? GetCpuMetrics(GetCpuMetricsRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, request.Uri);

            try
            {
                var response = _httpClient.SendAsync(httpRequest).Result;
                var stream = response.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<CpuMetricsResponse>(stream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return null;
        }
    }

    public interface IMetricsAgentClient
    {
        CpuMetricsResponse? GetCpuMetrics(GetCpuMetricsRequest request);
    }
}
