using Quartz;
using MetricsManager.DAL;
using System.Diagnostics;

namespace MetricsManager.Jobs
{
    public class CpuMetricJob : IJob
    {
        private readonly ICpuRepository _repository;
        private readonly PerformanceCounter _counter;

        public CpuMetricJob(ICpuRepository repository)
        {
            _repository = repository;
            _counter = new PerformanceCounter("Процессор", "% загруженности процессора", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var cpuUsage = Convert.ToInt32(_counter.NextValue());
            var time = DateTime.UtcNow;

            _repository.Insert(new Models.CpuMetric() { Time = time, Value = cpuUsage });
            _repository.Save();

            return Task.CompletedTask;
        }
    }
}
