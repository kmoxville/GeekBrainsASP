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
    public class NetworkMetricsControllerTest
    {
        private NetworkMetricsController controller;
        private Mock<INetworkRepository> mockRepository;
        private Mock<ILogger<NetworkMetricsController>> mockLogger;

        public NetworkMetricsControllerTest()
        {
            mockRepository = new Mock<INetworkRepository>();
            mockLogger = new Mock<ILogger<NetworkMetricsController>>();

            controller = new NetworkMetricsController(mockRepository.Object, mockLogger.Object);
        }

        [Fact]
        public void GetNetworkMetrics_ReturnsOk()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            var result = controller.GetMetrics(fromTime, toTime);

            Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void Insert_ShouldCall_Create_From_Repository()
        {
            mockRepository.Setup(repository => repository.Insert(It.IsAny<NetworkMetric>())).Verifiable();

            var result = controller.Insert(new NetworkMetricCreateRequest { Time = TimeSpan.FromSeconds(1), Value = 50 });

            mockRepository.Verify(repository => repository.Insert(It.IsAny<NetworkMetric>()), Times.AtMostOnce());
        }
    }
}