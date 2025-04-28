using MediatR;

namespace TaskManagement.Application.UseCases.Tasks.GetAllTasks;

public class GetAllTasksRequest : IRequest<GetAllTasksResponse>
{
    public Guid ProjectId { get; set; }
}
