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
    public class RamMetricsControllerTest
    {
        private RamMetricsController controller;
        private Mock<IRamRepository> mockRepository;
        private Mock<ILogger<RamMetricsController>> mockLogger;

        public RamMetricsControllerTest()
        {
            mockRepository = new Mock<IRamRepository>();
            mockLogger = new Mock<ILogger<RamMetricsController>>();

            controller = new RamMetricsController(mockRepository.Object, mockLogger.Object);
        }

        [Fact]
        public void GetRamMetrics_ReturnsOk()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            var result = controller.GetMetrics(fromTime, toTime);

            Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void Insert_ShouldCall_Create_From_Repository()
        {
            mockRepository.Setup(repository => repository.Insert(It.IsAny<RamMetric>())).Verifiable();

            var result = controller.Insert(new RamMetricCreateRequest { Time = TimeSpan.FromSeconds(1), Value = 50 });

            mockRepository.Verify(repository => repository.Insert(It.IsAny<RamMetric>()), Times.AtMostOnce());
        }
    }
}