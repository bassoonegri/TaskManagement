namespace TaskManagement.Application.UseCases.Projects.GetAllProjects;

public class GetAllProjectsResponse
{
    public IEnumerable<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
}

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
