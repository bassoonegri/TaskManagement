using TaskManagement.Application.Entities;

namespace TaskManagement.Infrastructure.Services;

public interface ITaskService
{
    Task<IEnumerable<ProjectTask>> GetTasksByProjectAsync(Guid projectId);
    Task<ProjectTask> CreateTaskAsync(Guid projectId, ProjectTask task);
    Task<ProjectTask> UpdateTaskAsync(Guid taskId, ProjectTask updatedTask);
    Task<bool> DeleteTaskAsync(Guid taskId);
    Task<Comment> AddCommentAsync(Guid taskId, Comment comment);

}

