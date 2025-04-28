using MediatR;
using System;

namespace TaskManagement.Application.UseCases.Tasks.DeleteTask;

public class DeleteTaskRequest : IRequest<DeleteTaskResponse>
{
    public Guid TaskId { get; set; }
}
