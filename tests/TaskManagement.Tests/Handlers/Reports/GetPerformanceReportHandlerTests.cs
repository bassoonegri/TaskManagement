using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using System.Threading;
using System;
using TaskManagement.Application.Repositories;
using TaskManagement.Application.UseCases.Reports.GetPerformanceReport;
using Xunit;

namespace TaskManagement.Tests.Handlers.Reports;

public class GetPerformanceReportHandlerTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly GetPerformanceReportHandler _handler;

    public GetPerformanceReportHandlerTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _handler = new GetPerformanceReportHandler(_taskRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Report_When_User_Is_Manager()
    {
        // Arrange
        _taskRepositoryMock.Setup(x => x.GetAverageCompletedTasksPerUserAsync())
            .ReturnsAsync(5.2);

        var request = new GetPerformanceReportRequest { Role = "manager" };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.AverageCompletedTasks.Should().Be(5.2);

        _taskRepositoryMock.Verify(x => x.GetAverageCompletedTasksPerUserAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_User_Is_Not_Manager()
    {
        // Arrange
        var request = new GetPerformanceReportRequest { Role = "user" };

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Acesso restrito a gerentes.");

        _taskRepositoryMock.Verify(x => x.GetAverageCompletedTasksPerUserAsync(), Times.Never);
    }
}
