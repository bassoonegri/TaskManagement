using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using TaskManagement.Application.Entities;
using TaskManagement.Application.Repositories;
using TaskManagement.Application.UseCases.Tasks.DeleteTask;
using Xunit;

namespace TaskManagement.Tests.Handlers.Tasks
{
    public class DeleteTaskHandlerTests
    {
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly DeleteTaskHandler _handler;

        public DeleteTaskHandlerTests()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _handler = new DeleteTaskHandler(_taskRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Task_Not_Found()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
                .ReturnsAsync((ProjectTask)null);

            var act = async () => await _handler.Handle(new DeleteTaskRequest { TaskId = taskId }, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Tarefa não encontrada.");
        }

        [Fact]
        public async Task Handle_Should_Delete_Task_When_Exists()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
                .ReturnsAsync(new ProjectTask { Id = taskId });

            // Act
            var result = await _handler.Handle(new DeleteTaskRequest { TaskId = taskId }, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
        }
    }
}
