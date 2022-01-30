using MetricsManager.DAL;
using MetricsManager.Models;
using MetricsManager.Requests;
using MetricsManager.Responses;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MetricsManager.Controllers
{
    [Route("api/metrics/Network")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {
        private readonly INetworkRepository _repository;
        private readonly ILogger<NetworkMetricsController> _logger;

        public NetworkMetricsController(INetworkRepository repository, ILogger<NetworkMetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("api/metrics/Network/left/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = _repository.GetAll();

            var response = new AllNetworkMetricsResponse()
            {
                Metrics = new List<NetworkMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new NetworkMetricDto { Time = metric.Time, Value = metric.Value, Id = metric.Id });
            }

            return Ok(response);
        }

        [HttpPost("create")]
        public IActionResult Insert([FromBody] NetworkMetricCreateRequest request)
        {
            _logger.LogInformation($"Insert called: {request}");

            _repository.Insert(new NetworkMetric
            {
                Time = request.Time,
                Value = request.Value
            });

            _repository.Save();

            return Ok();
        }
    }
}
