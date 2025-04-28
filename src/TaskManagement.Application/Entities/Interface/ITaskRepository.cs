using TaskManagement.Application.Entities;

namespace TaskManagement.Application.Repositories;

public interface ITaskRepository
{
    Task<IEnumerable<ProjectTask>> GetTasksByProjectIdAsync(Guid projectId);
    Task<ProjectTask> GetByIdAsync(Guid taskId);
    Task CreateAsync(ProjectTask task);
    Task UpdateAsync(ProjectTask task);
    Task DeleteAsync(ProjectTask task);
    Task<int> GetTaskCountByProjectAsync(Guid projectId);
    Task AddCommentAsync(Comment comment);
    Task AddTaskHistoryAsync(TaskHistory history);
    Task<double> GetAverageCompletedTasksPerUserAsync();
    Task SaveChangesAsync();
}
