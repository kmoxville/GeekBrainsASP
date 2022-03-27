using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl;

namespace MetricsAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ManagementController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPut("enable")]
        public async Task<IActionResult> Enable()
        {
            ISchedulerFactory schfack = new StdSchedulerFactory();
            var scheduler = await schfack.GetScheduler("default");
            await scheduler?.ResumeAll()!;

            _config["Enabled"] = bool.TrueString;

            return Ok();
        }

        [HttpPut("disable")]
        public async Task<IActionResult> Disable()
        {
            ISchedulerFactory schfack = new StdSchedulerFactory();
            var scheduler = await schfack.GetScheduler("default");
            await scheduler?.PauseAll()!;

            _config["Enabled"] = bool.FalseString;

            return Ok();
        }
    }
}
