using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using TaskManagement.Application.Entities;
using TaskManagement.Application.Repositories;
using TaskManagement.Application.UseCases.Tasks.GetAllTasks;
using Xunit;
using System.Linq;

namespace TaskManagement.Tests.Handlers.Tasks;

public class GetAllTasksHandlerTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly GetAllTasksHandler _handler;

    public GetAllTasksHandlerTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _handler = new GetAllTasksHandler(_taskRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Tasks_When_ProjectId_Is_Valid()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        _taskRepositoryMock.Setup(x => x.GetTasksByProjectIdAsync(projectId))
            .ReturnsAsync(new List<ProjectTask>
            {
                    new ProjectTask { Title = "Tarefa 1" },
                    new ProjectTask { Title = "Tarefa 2" }
            });

        // Act
        var result = await _handler.Handle(new GetAllTasksRequest { ProjectId = projectId }, CancellationToken.None);

        // Assert
        result.Tasks.Should().HaveCount(2);
        result.Tasks.Select(x => x.Title).Should().Contain(new[] { "Tarefa 1", "Tarefa 2" });
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_List_When_No_Tasks_Found()
    {
        // Arrange
        var projectId = Guid.NewGuid();

        _taskRepositoryMock.Setup(x => x.GetTasksByProjectIdAsync(projectId))
            .ReturnsAsync(new List<ProjectTask>());

        var request = new GetAllTasksRequest { ProjectId = projectId };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Tasks.Should().BeEmpty();
    }
}
