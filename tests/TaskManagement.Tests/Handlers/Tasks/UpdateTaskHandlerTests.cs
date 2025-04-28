using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using TaskManagement.Application.Entities;
using TaskManagement.Application.Repositories;
using TaskManagement.Application.UseCases.Tasks.UpdateTask;
using Xunit;

namespace TaskManagement.Tests.Handlers.Tasks;

public class UpdateTaskHandlerTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly UpdateTaskHandler _handler;

    public UpdateTaskHandlerTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _handler = new UpdateTaskHandler(_taskRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Update_Task_When_Valid()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var existingTask = new ProjectTask
        {
            Id = taskId,
            Title = "Título Antigo",
            Description = "Descrição Antiga",
            DueDate = DateTime.UtcNow.AddDays(5),
            Priority = TaskManagement.Application.Entities.TaskPriority.Medium,
            Status = TaskManagement.Application.Entities.TaskStatus.Pending
        };

        var request = new UpdateTaskRequest
        {
            TaskId = taskId,
            Title = "Título Novo",
            Description = "Descrição Nova",
            DueDate = DateTime.UtcNow.AddDays(10),
            Priority = (int)TaskManagement.Application.Entities.TaskPriority.Medium,
            Status = (int)TaskManagement.Application.Entities.TaskStatus.InProgress,
        };

        _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync(existingTask);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("Título Novo"); 
        _taskRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<ProjectTask>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Task_Not_Found()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var request = new UpdateTaskRequest { TaskId = taskId };

        _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync((ProjectTask)null);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Tarefa não encontrada.");
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Changing_Priority()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync(new ProjectTask { Id = taskId, Priority = TaskManagement.Application.Entities.TaskPriority.High });

        var request = new UpdateTaskRequest { TaskId = taskId, Priority = (int)TaskManagement.Application.Entities.TaskPriority.Medium };

        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Não é permitido alterar a prioridade da tarefa.");
    }
}
