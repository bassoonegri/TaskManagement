using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using System.Threading;
using System;
using TaskManagement.Application.Entities;
using TaskManagement.Application.Repositories;
using TaskManagement.Application.UseCases.Tasks.CreateTask;
using Xunit;

namespace TaskManagement.Tests.Handlers.Tasks
{
    public class CreateTaskHandlerTests
    {
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly CreateTaskHandler _handler;

        public CreateTaskHandlerTests()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _handler = new CreateTaskHandler(_taskRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Create_Task_When_Valid()
        {
            // Arrange
            var request = new CreateTaskRequest
            {
                ProjectId = Guid.NewGuid(),
                Title = "Nova Tarefa",
                Priority = 1,
                DueDate = DateTime.UtcNow.AddDays(7)
            };

            _taskRepositoryMock.Setup(x => x.GetTaskCountByProjectAsync(request.ProjectId))
                .ReturnsAsync(5);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.ProjectId.Should().Be(request.ProjectId);
            result.Title.Should().Be(request.Title);

            _taskRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<ProjectTask>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Task_Limit_Exceeded()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            _taskRepositoryMock.Setup(x => x.GetTaskCountByProjectAsync(projectId))
                .ReturnsAsync(20);

            var request = new CreateTaskRequest { ProjectId = projectId, Title = "Task", Priority = 1 };

            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Limite de 20 tarefas por projeto atingido.");
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Title_Is_Empty()
        {
            // Arrange
            var request = new CreateTaskRequest
            {
                ProjectId = Guid.NewGuid(),
                Title = "",
                Priority = 1,
                DueDate = DateTime.UtcNow.AddDays(7)
            };

            // Act
            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Título da tarefa é obrigatório.");
        }
    }
}
