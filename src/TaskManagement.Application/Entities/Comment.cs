namespace TaskManagement.Application.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public ProjectTask Task { get; set; }

    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid CreatedByUserId { get; set; }
}
