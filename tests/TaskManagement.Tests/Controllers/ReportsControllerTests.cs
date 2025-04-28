using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TaskManagement.Api.Controllers;
using TaskManagement.Application.UseCases.Reports.GetPerformanceReport;
using Xunit;

namespace TaskManagement.Tests.Controllers
{
    public class ReportsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ReportsController _controller;

        public ReportsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ReportsController(_mediatorMock.Object);
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
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPerformanceReportRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetPerformanceReportResponse { AverageCompletedTasks = 5.5 });

            // Act
            var result = await _controller.GetPerformanceReport("manager");

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(new { AverageCompletedTasks = 5.5 });
        }
    }
}
