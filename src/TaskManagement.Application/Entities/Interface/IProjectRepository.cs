using TaskManagement.Application.Entities;

namespace TaskManagement.Application.Repositories;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetProjectsByUserAsync(Guid userId);
    Task<Project> GetByIdAsync(Guid projectId);
    Task CreateAsync(Project project);
    Task DeleteAsync(Project project);
    Task SaveChangesAsync();
}
