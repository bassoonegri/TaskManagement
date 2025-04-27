using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Entities;
using TaskManagement.Infrastructure.Context;

namespace TaskManagement.Infrastructure.Services;

public class TaskService : ITaskService
{
    private readonly TaskManagementDbContext _dbContext;

    public TaskService(TaskManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ProjectTask>> GetTasksByProjectAsync(Guid projectId)
    {
        return await _dbContext.Tasks
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<ProjectTask> CreateTaskAsync(Guid projectId, ProjectTask task)
    {
        var taskCount = await _dbContext.Tasks.CountAsync(t => t.ProjectId == projectId);

        if (taskCount >= 20)
            throw new InvalidOperationException("Limite de tarefas excedido para este projeto.");

        task.ProjectId = projectId;
        _dbContext.Tasks.Add(task);
        await _dbContext.SaveChangesAsync();
        return task;
    }

    public async Task<ProjectTask> UpdateTaskAsync(Guid taskId, ProjectTask updatedTask)
    {
        var existingTask = await _dbContext.Tasks.FindAsync(taskId);

        if (existingTask == null)
            throw new KeyNotFoundException("Tarefa não encontrada.");

        if (existingTask.Priority != updatedTask.Priority)
            throw new InvalidOperationException("Não é permitido alterar a prioridade da tarefa após a criação.");

        existingTask.Title = updatedTask.Title;
        existingTask.Description = updatedTask.Description;
        existingTask.DueDate = updatedTask.DueDate;
        existingTask.Status = updatedTask.Status;

        _dbContext.Tasks.Update(existingTask);

        _dbContext.TaskHistories.Add(new TaskHistory
        {
            Id = Guid.NewGuid(),
            TaskId = taskId,
            ChangedAt = DateTime.UtcNow,
            ChangedByUserId = updatedTask.Project.UserId,
            ChangeDescription = "Tarefa atualizada."
        });

        await _dbContext.SaveChangesAsync();
        return existingTask;
    }

    public async Task<bool> DeleteTaskAsync(Guid taskId)
    {
        var task = await _dbContext.Tasks.FindAsync(taskId);

        if (task == null)
            return false;

        _dbContext.Tasks.Remove(task);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<Comment> AddCommentAsync(Guid taskId, Comment comment)
    {
        var task = await _dbContext.Tasks.FindAsync(taskId);

        if (task == null)
            throw new KeyNotFoundException("Tarefa não encontrada.");

        comment.TaskId = taskId;
        _dbContext.Comments.Add(comment);

        _dbContext.TaskHistories.Add(new TaskHistory
        {
            Id = Guid.NewGuid(),
            TaskId = taskId,
            ChangedAt = DateTime.UtcNow,
            ChangedByUserId = comment.CreatedByUserId,
            ChangeDescription = "Comentário adicionado."
        });

        await _dbContext.SaveChangesAsync();
        return comment;
    }
}
