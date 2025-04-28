using MediatR;
using System;

namespace TaskManagement.Application.UseCases.Tasks.AddComment;

public class AddCommentRequest : IRequest<AddCommentResponse>
{
    public Guid TaskId { get; set; }
    public Guid CreatedByUserId { get; set; }
    public string Content { get; set; }
}
