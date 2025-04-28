using System;

namespace TaskManagement.Application.UseCases.Tasks.AddComment;

public class AddCommentResponse
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public string Content { get; set; }
}
