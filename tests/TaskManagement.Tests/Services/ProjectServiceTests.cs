using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TaskManagement.Application.Entities;
using TaskManagement.Infrastructure.Context;
using TaskManagement.Infrastructure.Services;
using Xunit;

namespace TaskManagement.Tests.Services;

public class ProjectServiceTests
{
    private readonly TaskManagementDbContext _dbContext;
    private readonly ProjectService _projectService;

    public ProjectServiceTests()
    {
        var options = new DbContextOptionsBuilder<TaskManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new TaskManagementDbContext(options);
        _projectService = new ProjectService(_dbContext);
    }

    [Fact]
    public async Task CreateProjectAsync_Should_Create_Project()
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Project Test",
            UserId = Guid.NewGuid()
        };

        var result = await _projectService.CreateProjectAsync(project);

        result.Should().NotBeNull();
        (await _dbContext.Projects.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task GetProjectsByUserAsync_Should_Return_Projects()
    {
        var userId = Guid.NewGuid();

        _dbContext.Projects.Add(new Project { Id = Guid.NewGuid(), Name = "Test", UserId = userId });
        await _dbContext.SaveChangesAsync();

        var result = await _projectService.GetProjectsByUserAsync(userId);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
    }
}
