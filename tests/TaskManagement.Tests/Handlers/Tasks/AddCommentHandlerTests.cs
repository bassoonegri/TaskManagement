using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using TaskManagement.Application.Entities;
using TaskManagement.Application.Repositories;
using TaskManagement.Application.UseCases.Tasks.AddComment;
using Xunit;

namespace TaskManagement.Tests.Handlers.Tasks;

public class AddCommentHandlerTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly AddCommentHandler _handler;

    public AddCommentHandlerTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _handler = new AddCommentHandler(_taskRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Add_Comment_When_Task_Exists()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var task = new ProjectTask { Id = taskId };

        _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync(task);

        var request = new AddCommentRequest
        {
            TaskId = taskId,
            Content = "Novo comentário",
            CreatedByUserId = Guid.NewGuid()
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Content.Should().Be(request.Content);

        _taskRepositoryMock.Verify(x => x.AddCommentAsync(It.IsAny<Comment>()), Times.Once);
        _taskRepositoryMock.Verify(x => x.AddTaskHistoryAsync(It.IsAny<TaskHistory>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Task_Not_Found()
    {
        // Arrange
        var taskId = Guid.NewGuid();

        _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync((ProjectTask)null);

        var request = new AddCommentRequest
        {
            TaskId = taskId,
            Content = "Comentário inválido",
            CreatedByUserId = Guid.NewGuid()
        };

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Tarefa não encontrada.");
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Content_Is_Empty()
    {
        // Arrange
        _taskRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new ProjectTask { Id = Guid.NewGuid() });

        var request = new AddCommentRequest
        {
            TaskId = Guid.NewGuid(),
            Content = "", 
            CreatedByUserId = Guid.NewGuid()
        };

        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("O conteúdo do comentário é obrigatório.");
    }

}
