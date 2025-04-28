using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.UseCases.Tasks.AddComment;
using TaskManagement.Application.UseCases.Tasks.CreateTask;
using TaskManagement.Application.UseCases.Tasks.DeleteTask;
using TaskManagement.Application.UseCases.Tasks.GetAllTasks;
using TaskManagement.Application.UseCases.Tasks.UpdateTask;

namespace TaskManagement.Api.Controllers;

[ApiController]
[Route("")]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("projects/{projectId}/tasks")]
    public async Task<IActionResult> GetTasksByProject(Guid projectId)
    {
        var response = await _mediator.Send(new GetAllTasksRequest { ProjectId = projectId });
        return Ok(response.Tasks);
    }


    [HttpPost("projects/{projectId}/tasks")]
    public async Task<IActionResult> CreateTask(Guid projectId, [FromBody] CreateTaskRequest request)
    {
        if (request == null)
            return BadRequest("Dados da tarefa inválidos.");

        request.ProjectId = projectId;

        var created = await _mediator.Send(request);

        return CreatedAtAction(nameof(GetTasksByProject), new { projectId = created.ProjectId }, created);
    }



    [HttpPut("tasks/{taskId}")]
    public async Task<IActionResult> UpdateTask(Guid taskId, [FromBody] UpdateTaskRequest request)
    {
        if (request == null)
            return BadRequest("Dados da tarefa inválidos.");

        try
        {
            request.TaskId = taskId;

            var response = await _mediator.Send(request);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }


    [HttpDelete("tasks/{taskId}")]
    public async Task<IActionResult> DeleteTask(Guid taskId)
    {
        try
        {
            var response = await _mediator.Send(new DeleteTaskRequest { TaskId = taskId });

            return response.Success ? NoContent() : BadRequest();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }


    [HttpPost("tasks/{taskId}/comments")]
    public async Task<IActionResult> AddComment(Guid taskId, [FromBody] AddCommentRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Content))
            return BadRequest("Comentário inválido.");

        try
        {
            request.TaskId = taskId;

            var response = await _mediator.Send(request);
            return CreatedAtAction(nameof(GetTasksByProject), new { projectId = response.TaskId }, response);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }


}
