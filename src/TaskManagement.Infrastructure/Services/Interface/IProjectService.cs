using TaskManagement.Application.Entities;

namespace TaskManagement.Infrastructure.Services;

public interface IProjectService
{
    Task<IEnumerable<Project>> GetProjectsByUserAsync(Guid userId);
    Task<Project> CreateProjectAsync(Project project);
    Task<bool> DeleteProjectAsync(Guid projectId);
}
