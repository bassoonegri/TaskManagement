using TaskManagement.Application.Entities;
using TaskManagement.Application.Repositories;
using TaskManagement.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;


namespace TaskManagement.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TaskManagementDbContext _dbContext;

    public TaskRepository(TaskManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ProjectTask>> GetTasksByProjectIdAsync(Guid projectId)
    {
        return await _dbContext.Tasks
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<ProjectTask> GetByIdAsync(Guid taskId)
    {
        return await _dbContext.Tasks.FindAsync(taskId);
    }

    public async Task CreateAsync(ProjectTask task)
    {
        await _dbContext.Tasks.AddAsync(task);
    }

    public async Task UpdateAsync(ProjectTask task)
    {
        _dbContext.Tasks.Update(task);
    }

    public async Task DeleteAsync(ProjectTask task)
    {
        _dbContext.Tasks.Remove(task);
    }

    public async Task<int> GetTaskCountByProjectAsync(Guid projectId)
    {
        return await _dbContext.Tasks.CountAsync(t => t.ProjectId == projectId);
    }

    public async Task AddCommentAsync(Comment comment)
    {
        await _dbContext.Comments.AddAsync(comment);
    }

    public async Task AddTaskHistoryAsync(TaskHistory history)
    {
        await _dbContext.TaskHistories.AddAsync(history);
    }

    public async Task<double> GetAverageCompletedTasksPerUserAsync()
    {
        var completedTasks = await _dbContext.Tasks
            .Where(t => t.Status == TaskManagement.Application.Entities.TaskStatus.Done)
            .GroupBy(t => t.Project.UserId)
            .Select(g => g.Count())
            .ToListAsync();

        if (!completedTasks.Any())
            return 0;

        return completedTasks.Average();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
