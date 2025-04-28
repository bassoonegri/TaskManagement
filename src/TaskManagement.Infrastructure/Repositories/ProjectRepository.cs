using TaskManagement.Application.Entities;
using TaskManagement.Application.Repositories;
using TaskManagement.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;


namespace TaskManagement.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly TaskManagementDbContext _dbContext;

    public ProjectRepository(TaskManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Project>> GetProjectsByUserAsync(Guid userId)
    {
        return await _dbContext.Projects
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task<Project> GetByIdAsync(Guid projectId)
    {
        return await _dbContext.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == projectId);
    }

    public async Task CreateAsync(Project project)
    {
        await _dbContext.Projects.AddAsync(project);
    }

    public async Task DeleteAsync(Project project)
    {
        _dbContext.Projects.Remove(project);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
