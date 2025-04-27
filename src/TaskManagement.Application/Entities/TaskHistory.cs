namespace TaskManagement.Application.Entities;

public class TaskHistory
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public ProjectTask Task { get; set; }

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    public Guid ChangedByUserId { get; set; }
    public string ChangeDescription { get; set; }
}

