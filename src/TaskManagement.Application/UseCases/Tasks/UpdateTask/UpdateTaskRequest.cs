using MediatR;
using System;

namespace TaskManagement.Application.UseCases.Tasks.UpdateTask;

public class UpdateTaskRequest : IRequest<UpdateTaskResponse>
{
    public Guid TaskId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Status { get; set; }
    public int Priority { get; set; }
    public DateTime DueDate { get; set; }
}
