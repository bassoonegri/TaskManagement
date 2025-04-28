namespace TaskManagement.Application.UseCases.Tasks.GetAllTasks;

public class GetAllTasksResponse
{
    public IEnumerable<ProjectTaskDto> Tasks { get; set; } = new List<ProjectTaskDto>();
}

public class ProjectTaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Priority { get; set; }
    public int Status { get; set; }
    public DateTime DueDate { get; set; }
}
