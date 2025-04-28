using MediatR;
using System;

namespace TaskManagement.Application.UseCases.Tasks.CreateTask;

public class CreateTaskRequest : IRequest<CreateTaskResponse>
{
    public Guid ProjectId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Priority { get; set; }
    public DateTime DueDate { get; set; }
}
