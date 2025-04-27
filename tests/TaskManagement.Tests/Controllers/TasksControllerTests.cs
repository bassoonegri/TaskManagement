using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using TaskManagement.Api.Controllers;
using TaskManagement.Infrastructure.Services;
using Xunit;

namespace TaskManagement.Tests.Controllers
{
    public class TasksControllerTests
    {
        private readonly Mock<ITaskService> _taskServiceMock;
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _taskServiceMock = new Mock<ITaskService>();
            _controller = new TasksController(_taskServiceMock.Object);
        }

        [Fact]
        public async Task AddComment_Should_Return_BadRequest_When_Comment_Is_Null()
        {
            var result = await _controller.AddComment(Guid.NewGuid(), null);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateTask_Should_Return_BadRequest_When_Task_Is_Null()
        {
            var result = await _controller.CreateTask(Guid.NewGuid(), null);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateTask_Should_Return_BadRequest_When_Task_Is_Null()
        {
            var result = await _controller.UpdateTask(Guid.NewGuid(), null);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteTask_Should_Return_NotFound_When_Task_Does_Not_Exist()
        {
            _taskServiceMock.Setup(s => s.DeleteTaskAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            var result = await _controller.DeleteTask(Guid.NewGuid());

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
