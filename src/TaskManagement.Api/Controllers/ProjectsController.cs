using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.UseCases.Projects.CreateProject;
using TaskManagement.Application.UseCases.Projects.DeleteProject;
using TaskManagement.Application.UseCases.Projects.GetAllProjects;

namespace TaskManagement.Api.Controllers;

[ApiController]
[Route("projects")]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet]
    public async Task<IActionResult> GetProjectsByUser([FromQuery] Guid userId)
    {
        if (userId == Guid.Empty)
            return BadRequest("UserId é obrigatório.");

        var result = await _mediator.Send(new GetAllProjectsRequest { UserId = userId });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
    {
        if (request == null)
            return BadRequest("Dados do projeto inválidos.");

        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpDelete("{projectId}")]
    public async Task<IActionResult> DeleteProject(Guid projectId)
    {
        try
        {
            var response = await _mediator.Send(new DeleteProjectRequest { ProjectId = projectId });

            return response.Success ? NoContent() : BadRequest("Erro ao excluir projeto.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

}

