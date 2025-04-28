using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using TaskManagement.Application.Entities;
using TaskManagement.Application.Repositories;
using TaskManagement.Application.UseCases.Projects.GetAllProjects;
using Xunit;
using System.Linq;

namespace TaskManagement.Tests.Handlers.Projects
{
    public class GetAllProjectsHandlerTests
    {
        private readonly Mock<IProjectRepository> _projectRepositoryMock;
        private readonly GetAllProjectsHandler _handler;

        public GetAllProjectsHandlerTests()
        {
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _handler = new GetAllProjectsHandler(_projectRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_Projects_When_UserId_Is_Valid()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _projectRepositoryMock.Setup(x => x.GetProjectsByUserAsync(userId))
                .ReturnsAsync(new List<Project>
                {
                    new Project { Id = Guid.NewGuid(), Name = "Projeto 1", UserId = userId, CreatedAt = DateTime.UtcNow },
                    new Project { Id = Guid.NewGuid(), Name = "Projeto 2", UserId = userId, CreatedAt = DateTime.UtcNow }
                });

            var request = new GetAllProjectsRequest { UserId = userId };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Projects.Should().NotBeNullOrEmpty();
            result.Projects.Should().HaveCount(2);

            var projectNames = result.Projects.Select(x => x.Name).ToList();
            projectNames.Should().Contain("Projeto 1");
            projectNames.Should().Contain("Projeto 2");
        }

        [Fact]
        public async Task Handle_Should_Throw_When_UserId_Is_Empty()
        {
            // Arrange
            var request = new GetAllProjectsRequest { UserId = Guid.Empty };

            // Act
            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("UserId é obrigatório.");
        }
    }
}
