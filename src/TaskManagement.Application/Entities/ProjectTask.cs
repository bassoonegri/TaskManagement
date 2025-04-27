namespace TaskManagement.Application.Entities;

public enum TaskStatus
{
    Pending,
    InProgress,
    Done
}

public enum TaskPriority
{
    Low,
    Medium,
    High
}

public class ProjectTask
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.Pending;
    public TaskPriority Priority { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; }

    public ICollection<TaskHistory> History { get; set; }
    public ICollection<Comment> Comments { get; set; }
}
