using MediatR;
using TaskManagement.Application.Repositories;

namespace TaskManagement.Application.UseCases.Tasks.GetAllTasks;

public class GetAllTasksHandler : IRequestHandler<GetAllTasksRequest, GetAllTasksResponse>
{
    private readonly ITaskRepository _taskRepository;

    public GetAllTasksHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<GetAllTasksResponse> Handle(GetAllTasksRequest request, CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.GetTasksByProjectIdAsync(request.ProjectId);

        return new GetAllTasksResponse
        {
            Tasks = tasks.Select(t => new ProjectTaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Priority = (int)t.Priority,
                Status = (int)t.Status,
                DueDate = t.DueDate
            }).ToList()
        };
    }
}
