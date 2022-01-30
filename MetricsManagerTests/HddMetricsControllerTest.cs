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
    public class HddMetricsControllerTest
    {
        private HddMetricsController controller;
        private Mock<IHddRepository> mockRepository;
        private Mock<ILogger<HddMetricsController>> mockLogger;

        public HddMetricsControllerTest()
        {
            mockRepository = new Mock<IHddRepository>();
            mockLogger = new Mock<ILogger<HddMetricsController>>();

            controller = new HddMetricsController(mockRepository.Object, mockLogger.Object);
        }

        [Fact]
        public void GetHddMetrics_ReturnsOk()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            var result = controller.GetMetrics(fromTime, toTime);

            Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void Insert_ShouldCall_Create_From_Repository()
        {
            mockRepository.Setup(repository => repository.Insert(It.IsAny<HddMetric>())).Verifiable();

            var result = controller.Insert(new HddMetricCreateRequest { Time = TimeSpan.FromSeconds(1), Value = 50 });

            mockRepository.Verify(repository => repository.Insert(It.IsAny<HddMetric>()), Times.AtMostOnce());
        }
    }
}