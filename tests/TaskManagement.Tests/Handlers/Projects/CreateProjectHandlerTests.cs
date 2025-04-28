using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using System.Threading;
using System;
using TaskManagement.Application.Entities;
using TaskManagement.Application.Repositories;
using TaskManagement.Application.UseCases.Projects.CreateProject;
using Xunit;

namespace TaskManagement.Tests.Handlers.Projects;

public class CreateProjectHandlerTests
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly CreateProjectHandler _handler;

    public CreateProjectHandlerTests()
    {
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _handler = new CreateProjectHandler(_projectRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_Project_When_Valid()
    {
        // Arrange
        var request = new CreateProjectRequest
        {
            Name = "Projeto Teste",
            UserId = Guid.NewGuid()
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Projeto Teste");
        result.UserId.Should().Be(request.UserId);

        _projectRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Project>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Name_Is_Empty()
    {
        // Arrange
        var request = new CreateProjectRequest
        {
            Name = "",
            UserId = Guid.NewGuid()
        };

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Nome do projeto é obrigatório.");
    }

    [Fact]
    public async Task Handle_Should_Throw_When_UserId_Is_Empty()
    {
        // Arrange
        var request = new CreateProjectRequest
        {
            Name = "Projeto Teste",
            UserId = Guid.Empty
        };

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Usuário do projeto é obrigatório.");
    }
}
