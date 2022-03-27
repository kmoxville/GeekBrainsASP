using AutoMapper;
using MetricsManager.DAL;
using MetricsManager.MetricsAgentClient;
using MetricsManager.Models;
using MetricsManager.Requests;
using MetricsManager.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly ICpuRepository _cpuRepository;
        private readonly IAgentsRepository _agentsRepository;
        private readonly ILogger<CpuMetricsController> _logger;
        private readonly IMapper _mapper;
        private readonly IMetricsAgentClient _metricsClient;

        public CpuMetricsController(ICpuRepository cpuRepository, 
            ILogger<CpuMetricsController> logger, 
            IAgentsRepository agentsRepository, 
            IMapper mapper,
            IMetricsAgentClient metricsClient)
        {
            _cpuRepository = cpuRepository;
            _agentsRepository = agentsRepository;
            _logger = logger;
            _mapper = mapper;
            _metricsClient = metricsClient;
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetrics([FromRoute] int agentId, [FromRoute] DateTime fromTime, [FromRoute] DateTime toTime)
        {
            var agentInfo = _agentsRepository.FirstOrDefault(agentInfo => agentInfo.Id == agentId);

            if (agentInfo == null)
            {
                return NotFound($"Agent with id {agentId} is not registered");
            }

            if (NeedToFetchData(agentId, toTime))
            {
                var lastDate = LastEntryDate(agentId);
                FetchData(agentInfo, lastDate, toTime);
            }

            var metrics = _cpuRepository
                .Where(metric => metric.AgentId == agentId && metric.Time >= fromTime && metric.Time < toTime);

            var response = new CpuMetricsResponse();

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
            }

            return Ok(response);
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var response = new CpuMetricsResponse();

            foreach (var metric in _cpuRepository)
            {
                response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
            }

            return Ok(response);
        }

        [HttpPost("create")]
        public IActionResult Insert([FromBody] CpuMetricCreateRequest request)
        {
            _logger.LogInformation($"Insert called: {request}");

            var newMetric = new CpuMetric
            {
                Time = request.Time,
                Value = request.Value
            };

            _cpuRepository.Insert(newMetric);

            _cpuRepository.Save();

            return CreatedAtAction(nameof(Insert), new { Time = request.Time, Value = request.Value }, newMetric);
        }

        private void FetchData(AgentInfo agentInfo, DateTime fromTime, DateTime toTime)
        {
            var fromTimeQuery = fromTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
            var toTimeQuery = toTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
            var metricsRequest = new GetCpuMetricsRequest()
            {
                Uri = new Uri(agentInfo.Uri.ToString() + $"api/metrics/cpu/from/{fromTimeQuery}/to/{toTimeQuery}"),
                From = fromTime,
                To = toTime
            };

            var metrics = _metricsClient.GetCpuMetrics(metricsRequest);

            if (metrics == null)
                return;

            foreach (var metric in metrics.Metrics)
            {
                _cpuRepository.Insert(new CpuMetric() { AgentId = agentInfo.Id, Time = metric.Time, Value = metric.Value });
            }

            _cpuRepository.Save();
        }

        internal bool NeedToFetchData(int agentId, DateTime toTime)
        {
            return LastEntryDate(agentId) < toTime;
        }

        internal DateTime LastEntryDate(int agentId)
        {
            return _cpuRepository.LastOrDefault(entry => entry.AgentId == agentId)?.Time - TimeSpan.FromSeconds(10) ?? DateTime.MinValue;
        }

    }
}
