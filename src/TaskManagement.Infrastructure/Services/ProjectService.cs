using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Entities;
using TaskManagement.Infrastructure.Context;

namespace TaskManagement.Infrastructure.Services;

public class ProjectService : IProjectService
{
    private readonly TaskManagementDbContext _dbContext;

    public ProjectService(TaskManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Project>> GetProjectsByUserAsync(Guid userId)
    {
        return await _dbContext.Projects
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task<Project> CreateProjectAsync(Project project)
    {
        project.Id = Guid.NewGuid();
        project.CreatedAt = DateTime.UtcNow;

        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();
        return project;
    }

    public async Task<bool> DeleteProjectAsync(Guid projectId)
    {
        var project = await _dbContext.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null)
            throw new KeyNotFoundException("Projeto não encontrado.");

        if (project.Tasks.Any(t => t.Status == TaskManagement.Application.Entities.TaskStatus.Pending))
            throw new InvalidOperationException("Não é possível excluir um projeto com tarefas pendentes.");

        _dbContext.Projects.Remove(project);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
