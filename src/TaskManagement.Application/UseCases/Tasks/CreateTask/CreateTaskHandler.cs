using MediatR;
using TaskManagement.Application.Entities;
using TaskManagement.Application.Repositories;

namespace TaskManagement.Application.UseCases.Tasks.CreateTask;

public class CreateTaskHandler : IRequestHandler<CreateTaskRequest, CreateTaskResponse>
{
    private readonly ITaskRepository _taskRepository;

    public CreateTaskHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<CreateTaskResponse> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ArgumentException("Título da tarefa é obrigatório.");

        var taskCount = await _taskRepository.GetTaskCountByProjectAsync(request.ProjectId);

        if (taskCount >= 20)
            throw new InvalidOperationException("Limite de 20 tarefas por projeto atingido.");

        var task = new ProjectTask
        {
            Id = Guid.NewGuid(),
            ProjectId = request.ProjectId,
            Title = request.Title,
            Description = request.Description,
            Priority = (TaskManagement.Application.Entities.TaskPriority)request.Priority,
            Status = TaskManagement.Application.Entities.TaskStatus.Pending,
            DueDate = request.DueDate,
            CreatedAt = DateTime.UtcNow
        };

        await _taskRepository.CreateAsync(task);
        await _taskRepository.SaveChangesAsync();

        return new CreateTaskResponse
        {
            Id = task.Id,
            ProjectId = task.ProjectId,
            Title = task.Title
        };
    }
}
