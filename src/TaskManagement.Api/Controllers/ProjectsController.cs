using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Entities;
using TaskManagement.Infrastructure.Services;

namespace TaskManagement.Api.Controllers;

[ApiController]
[Route("projects")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProjectsByUser([FromQuery] Guid userId)
    {
        if (userId == Guid.Empty)
            return BadRequest("UserId is required.");

        var projects = await _projectService.GetProjectsByUserAsync(userId);
        return Ok(projects);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] Project project)
    {
        if (project == null || string.IsNullOrWhiteSpace(project.Name) || project.UserId == Guid.Empty)
            return BadRequest("Invalid project data.");

        var createdProject = await _projectService.CreateProjectAsync(project);
        return CreatedAtAction(nameof(GetProjectsByUser), new { userId = createdProject.UserId }, createdProject);
    }

    [HttpDelete("{projectId}")]
    public async Task<IActionResult> DeleteProject(Guid projectId)
    {
        try
        {
            var result = await _projectService.DeleteProjectAsync(projectId);
            if (!result)
                return NotFound("Projeto não encontrado.");

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

