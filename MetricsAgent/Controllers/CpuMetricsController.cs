using AutoMapper;
using MetricsManager.DAL;
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
        private readonly ICpuRepository _repository;
        private readonly IMapper _mapper;

        public CpuMetricsController(ICpuRepository repository, 
            ILogger<CpuMetricsController> logger,  
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetrics([FromRoute] DateTime fromTime, [FromRoute] DateTime toTime)
        {
            if (toTime < fromTime)
                return BadRequest();

            var metrics = _repository
                .GetAll()
                .Where(metric => metric.Time >= fromTime && metric.Time < toTime);

            var response = new CpuMetricsResponse();

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
            }

            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var metrics = _repository.GetAll();

            var response = new CpuMetricsResponse();

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
            }

            return Ok(response);
        }
    }
}
