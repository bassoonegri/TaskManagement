using MediatR;
using TaskManagement.Application.Repositories;

namespace TaskManagement.Application.UseCases.Tasks.DeleteTask;

public class DeleteTaskHandler : IRequestHandler<DeleteTaskRequest, DeleteTaskResponse>
{
    private readonly ITaskRepository _taskRepository;

    public DeleteTaskHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<DeleteTaskResponse> Handle(DeleteTaskRequest request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId);

        if (task == null)
            throw new KeyNotFoundException("Tarefa não encontrada.");

        await _taskRepository.DeleteAsync(task);
        await _taskRepository.SaveChangesAsync();

        return new DeleteTaskResponse
        {
            Success = true
        };
    }
}
