using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagement.Application.Entities;
using TaskManagement.Infrastructure.Context;
using TaskManagement.Infrastructure.Services;
using Xunit;

namespace TaskManagement.Tests.Services;

public class TaskServiceTests
{
    private readonly TaskManagementDbContext _dbContext;
    private readonly TaskService _taskService;

    public TaskServiceTests()
    {
        var options = new DbContextOptionsBuilder<TaskManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new TaskManagementDbContext(options);
        _taskService = new TaskService(_dbContext);
    }

    [Fact]
    public async Task CreateTaskAsync_Should_Create_Task()
    {
        var projectId = Guid.NewGuid();
        _dbContext.Projects.Add(new Project { Id = projectId, Name = "Test", UserId = Guid.NewGuid() });
        await _dbContext.SaveChangesAsync();

        var task = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Title = "Task Test",
            Priority = TaskPriority.Medium,
            DueDate = DateTime.UtcNow.AddDays(5)
        };

        var result = await _taskService.CreateTaskAsync(projectId, task);

        result.Should().NotBeNull();
        (await _dbContext.Tasks.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task CreateTaskAsync_Should_Throw_When_Task_Limit_Exceeded()
    {
        var projectId = Guid.NewGuid();
        _dbContext.Projects.Add(new Project { Id = projectId, Name = "Test", UserId = Guid.NewGuid() });
        for (int i = 0; i < 20; i++)
        {
            _dbContext.Tasks.Add(new ProjectTask
            {
                Id = Guid.NewGuid(),
                Title = $"Task {i}",
                Priority = TaskPriority.Low,
                ProjectId = projectId,
                DueDate = DateTime.UtcNow
            });
        }
        await _dbContext.SaveChangesAsync();

        var newTask = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Title = "New Task",
            Priority = TaskPriority.High,
            DueDate = DateTime.UtcNow
        };

        var act = async () => await _taskService.CreateTaskAsync(projectId, newTask);

        await act.Should().ThrowAsync<InvalidOperationException>()
     .WithMessage("Limite de tarefas excedido para este projeto.");

    }

    [Fact]
    public async Task UpdateTaskAsync_Should_Update_Task()
    {
        var projectId = Guid.NewGuid();
        var task = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Title = "Original",
            Priority = TaskPriority.Low,
            DueDate = DateTime.UtcNow,
            ProjectId = projectId
        };
        _dbContext.Tasks.Add(task);
        _dbContext.Projects.Add(new Project { Id = projectId, Name = "Test", UserId = Guid.NewGuid() });
        await _dbContext.SaveChangesAsync();

        var updatedTask = new ProjectTask
        {
            Title = "Updated",
            Priority = TaskPriority.Low,
            DueDate = DateTime.UtcNow.AddDays(5),
            Status = TaskManagement.Application.Entities.TaskStatus.InProgress,
            Project = new Project { UserId = Guid.NewGuid() }
        };

        var result = await _taskService.UpdateTaskAsync(task.Id, updatedTask);

        result.Title.Should().Be("Updated");
        result.Status.Should().Be(TaskManagement.Application.Entities.TaskStatus.InProgress);
    }

    [Fact]
    public async Task UpdateTaskAsync_Should_Throw_When_Changing_Priority()
    {
        var projectId = Guid.NewGuid();
        var task = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Title = "Original",
            Priority = TaskPriority.Low,
            DueDate = DateTime.UtcNow,
            ProjectId = projectId
        };
        _dbContext.Tasks.Add(task);
        await _dbContext.SaveChangesAsync();

        var updatedTask = new ProjectTask
        {
            Title = "Trying to update",
            Priority = TaskPriority.High, // tentando mudar
            DueDate = DateTime.UtcNow,
            Project = new Project { UserId = Guid.NewGuid() }
        };

        var act = async () => await _taskService.UpdateTaskAsync(task.Id, updatedTask);

        await act.Should().ThrowAsync<InvalidOperationException>()
      .WithMessage("Não é permitido alterar a prioridade da tarefa após a criação.");

    }

    [Fact]
    public async Task DeleteTaskAsync_Should_Return_False_When_Task_Not_Found()
    {
        var result = await _taskService.DeleteTaskAsync(Guid.NewGuid());
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateTaskAsync_Should_Throw_When_Task_Not_Found()
    {
        var updatedTask = new ProjectTask
        {
            Title = "NonExisting",
            Priority = TaskPriority.Low,
            DueDate = DateTime.UtcNow,
            Project = new Project { UserId = Guid.NewGuid() }
        };

        var act = async () => await _taskService.UpdateTaskAsync(Guid.NewGuid(), updatedTask);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

}
