using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using TaskManagement.Api.Controllers;
using TaskManagement.Infrastructure.Services;
using Xunit;

namespace TaskManagement.Tests.Controllers
{
    public class ReportsControllerTests
    {
        private readonly Mock<IReportService> _reportServiceMock;
        private readonly ReportsController _controller;

        public ReportsControllerTests()
        {
            _reportServiceMock = new Mock<IReportService>();
            _controller = new ReportsController(_reportServiceMock.Object);
        }

        [Fact]
        public async Task GetPerformanceReport_Should_Return_Forbid_When_User_Is_Not_Manager()
        {
            var result = await _controller.GetPerformanceReport("user");

            result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task GetPerformanceReport_Should_Return_Ok_When_User_Is_Manager()
        {
            _reportServiceMock.Setup(x => x.GetAverageCompletedTasksPerUserAsync())
                .ReturnsAsync(5.5);

            var result = await _controller.GetPerformanceReport("manager");

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().NotBeNull();
        }
    }
}
