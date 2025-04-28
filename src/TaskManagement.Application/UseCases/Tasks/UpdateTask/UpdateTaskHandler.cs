using MediatR;
using TaskManagement.Application.Entities;
using TaskManagement.Application.Repositories;

namespace TaskManagement.Application.UseCases.Tasks.UpdateTask;

public class UpdateTaskHandler : IRequestHandler<UpdateTaskRequest, UpdateTaskResponse>
{
    private readonly ITaskRepository _taskRepository;

    public UpdateTaskHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<UpdateTaskResponse> Handle(UpdateTaskRequest request, CancellationToken cancellationToken)
    {
        var existingTask = await _taskRepository.GetByIdAsync(request.TaskId);

        if (existingTask == null)
            throw new KeyNotFoundException("Tarefa não encontrada.");

        if ((int)existingTask.Priority != request.Priority)
            throw new InvalidOperationException("Não é permitido alterar a prioridade da tarefa.");

        existingTask.Title = request.Title;
        existingTask.Description = request.Description;
        existingTask.DueDate = request.DueDate;
        existingTask.Status = (TaskManagement.Application.Entities.TaskStatus)request.Status;

        await _taskRepository.UpdateAsync(existingTask);
        await _taskRepository.SaveChangesAsync();

        return new UpdateTaskResponse
        {
            Id = existingTask.Id,
            Title = existingTask.Title
        };
    }
}
