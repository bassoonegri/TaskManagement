using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskManagement.Api.Controllers;
using TaskManagement.Application.UseCases.Projects.DeleteProject;
using Xunit;

namespace TaskManagement.Tests.Controllers
{
    public class ProjectsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ProjectsController _controller;

        public ProjectsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ProjectsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task CreateProject_Should_Return_BadRequest_When_Request_Is_Null()
        {
            var result = await _controller.CreateProject(null);

            result.Should().BeOfType<BadRequestObjectResult>();
        }


        [Fact]
        public async Task GetProjectsByUser_Should_Return_BadRequest_When_UserId_Is_Empty()
        {
            var result = await _controller.GetProjectsByUser(Guid.Empty);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteProject_Should_Return_NotFound_When_Project_Does_Not_Exist()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProjectRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.DeleteProject(Guid.NewGuid());

            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
