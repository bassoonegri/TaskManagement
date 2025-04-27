using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Entities;
using TaskManagement.Infrastructure.Services;

namespace TaskManagement.Api.Controllers;

[ApiController]
[Route("")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("projects/{projectId}/tasks")]
    public async Task<IActionResult> GetTasksByProject(Guid projectId)
    {
        var tasks = await _taskService.GetTasksByProjectAsync(projectId);
        return Ok(tasks);
    }

    [HttpPost("projects/{projectId}/tasks")]
    public async Task<IActionResult> CreateTask(Guid projectId, [FromBody] ProjectTask task)
    {
        if (task == null || string.IsNullOrWhiteSpace(task.Title))
            return BadRequest("Invalid task data.");

        try
        {
            var createdTask = await _taskService.CreateTaskAsync(projectId, task);
            return CreatedAtAction(nameof(GetTasksByProject), new { projectId = createdTask.ProjectId }, createdTask);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("tasks/{taskId}")]
    public async Task<IActionResult> UpdateTask(Guid taskId, [FromBody] ProjectTask updatedTask)
    {
        if (updatedTask == null)
            return BadRequest("Invalid task data.");

        try
        {
            var task = await _taskService.UpdateTaskAsync(taskId, updatedTask);
            return Ok(task);
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
        var success = await _taskService.DeleteTaskAsync(taskId);

        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpPost("tasks/{taskId}/comments")]
    public async Task<IActionResult> AddComment(Guid taskId, [FromBody] Comment comment)
    {
        if (comment == null || string.IsNullOrWhiteSpace(comment.Content))
            return BadRequest("Invalid comment.");

        try
        {
            var addedComment = await _taskService.AddCommentAsync(taskId, comment);
            return CreatedAtAction(nameof(GetTasksByProject), new { projectId = addedComment.TaskId }, addedComment);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

}
