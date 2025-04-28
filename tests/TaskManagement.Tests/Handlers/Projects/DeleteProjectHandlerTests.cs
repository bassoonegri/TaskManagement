using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using TaskManagement.Application.Entities;
using TaskManagement.Application.Repositories;
using TaskManagement.Application.UseCases.Projects.DeleteProject;
using Xunit;

namespace TaskManagement.Tests.Handlers.Projects
{
    public class DeleteProjectHandlerTests
    {
        private readonly Mock<IProjectRepository> _projectRepositoryMock;
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly DeleteProjectHandler _handler;

        public DeleteProjectHandlerTests()
        {
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _handler = new DeleteProjectHandler(_projectRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Delete_Project_When_No_Pending_Tasks()
        {
            var projectId = Guid.NewGuid();
            _projectRepositoryMock.Setup(x => x.GetByIdAsync(projectId))
                .ReturnsAsync(new Project
                {
                    Id = projectId,
                    Tasks = new List<ProjectTask>()  
                });

            var handler = new DeleteProjectHandler(_projectRepositoryMock.Object);

            var result = await handler.Handle(new DeleteProjectRequest { ProjectId = projectId }, CancellationToken.None);

            result.Success.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Project_Has_Pending_Tasks()
        {
            var projectId = Guid.NewGuid();
            _projectRepositoryMock.Setup(x => x.GetByIdAsync(projectId))
                .ReturnsAsync(new Project
                {
                    Id = projectId,
                    Tasks = new List<ProjectTask>
                    {
                new ProjectTask { Status = TaskManagement.Application.Entities.TaskStatus.Pending }
                    }
                });

            var handler = new DeleteProjectHandler(_projectRepositoryMock.Object);

            var act = async () => await handler.Handle(new DeleteProjectRequest { ProjectId = projectId }, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Não é possível excluir um projeto com tarefas pendentes.");
        }


    }
}
