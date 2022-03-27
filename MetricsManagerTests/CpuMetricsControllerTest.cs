using AutoMapper;
using MetricsManager.Controllers;
using MetricsManager.DAL;
using MetricsManager.MetricsAgentClient;
using MetricsManager.Models;
using MetricsManager.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MetricsManagerTests
{
    public class CpuMetricsControllerTest
    {
        private readonly CpuMetricsController _controller;
        private readonly Mock<ICpuRepository> _mockCpuRepository;
        private readonly Mock<ILogger<CpuMetricsController>> _mockLogger;
        private readonly Mock<IAgentsRepository> _mockAgentsRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IMetricsAgentClient> _mockMetricsAgentClient;

        public CpuMetricsControllerTest()
        {
            _mockCpuRepository = new Mock<ICpuRepository>();
            _mockLogger = new Mock<ILogger<CpuMetricsController>>();
            _mockAgentsRepository = new Mock<IAgentsRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockMetricsAgentClient = new Mock<IMetricsAgentClient>();

            _controller = new CpuMetricsController(_mockCpuRepository.Object, 
                _mockLogger.Object, 
                _mockAgentsRepository.Object, 
                _mockMapper.Object, 
                _mockMetricsAgentClient.Object);
        }

        [Fact]
        public void GetCpuMetrics_ReturnsOk()
        {
            int agentId = 1;
            var fromTime = DateTime.MinValue;
            var toTime = DateTime.MaxValue;

            _mockCpuRepository.Setup(repository => repository.GetEnumerator()).Returns((new List<CpuMetric>()).GetEnumerator());
            _mockAgentsRepository.Setup(repository => repository.GetEnumerator()).Returns((new List<AgentInfo>()).GetEnumerator());

            var result = _controller.GetMetrics(agentId, fromTime, toTime);

            Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Theory]
        [InlineData("29.03.2022 23:58:56", "28.03.2022 23:58:56")]
        public void NeedToFetchData_ShouldReturn_True(string lastDateStr, string checkDateStr)
        {
            var lastDate = DateTime.Parse(lastDateStr);
            var checkDate = DateTime.Parse(checkDateStr);
            var data = new List<CpuMetric>();
            data.Add(new CpuMetric() { Time = lastDate });

            _mockCpuRepository.Setup(repository => repository.GetEnumerator()).Returns(data.GetEnumerator());
            _mockCpuRepository.Setup((repository) => repository.Insert(new CpuMetric() { AgentId = 1, Time = lastDate }))
                .Verifiable();

            Assert.True(_controller.NeedToFetchData(1, checkDate));
        }
    }
}