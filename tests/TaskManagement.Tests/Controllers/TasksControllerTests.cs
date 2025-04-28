using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskManagement.Api.Controllers;
using TaskManagement.Application.UseCases.Tasks.AddComment;
using TaskManagement.Application.UseCases.Tasks.CreateTask;
using TaskManagement.Application.UseCases.Tasks.UpdateTask;
using TaskManagement.Application.UseCases.Tasks.DeleteTask;
using Xunit;
using System.Collections.Generic;

namespace TaskManagement.Tests.Controllers
{
    public class TasksControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new TasksController(_mediatorMock.Object);
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
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteTaskRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.DeleteTask(Guid.NewGuid());

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
