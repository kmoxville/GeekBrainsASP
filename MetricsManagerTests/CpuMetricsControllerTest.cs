using MetricsManager.Controllers;
using MetricsManager.DAL;
using MetricsManager.Models;
using MetricsManager.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace MetricsManagerTests
{
    public class CpuMetricsControllerTest
    {
        private CpuMetricsController controller;
        private Mock<ICpuRepository> mockRepository;
        private Mock<ILogger<CpuMetricsController>> mockLogger;

        public CpuMetricsControllerTest()
        {
            mockRepository = new Mock<ICpuRepository>();
            mockLogger = new Mock<ILogger<CpuMetricsController>>();

            controller = new CpuMetricsController(mockRepository.Object, mockLogger.Object);
        }

        [Fact]
        public void GetCpuMetrics_ReturnsOk()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            var result = controller.GetMetrics(fromTime, toTime);

            Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void Insert_ShouldCall_Create_From_Repository()
        {
            mockRepository.Setup(repository => repository.Insert(It.IsAny<CpuMetric>())).Verifiable();

            var result = controller.Insert(new CpuMetricCreateRequest { Time = TimeSpan.FromSeconds(1), Value = 50 });

            mockRepository.Verify(repository => repository.Insert(It.IsAny<CpuMetric>()), Times.AtMostOnce());
        }
    }
}