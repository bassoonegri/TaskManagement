using MediatR;
using TaskManagement.Application.Entities;
using TaskManagement.Application.Repositories;

namespace TaskManagement.Application.UseCases.Tasks.AddComment;

public class AddCommentHandler : IRequestHandler<AddCommentRequest, AddCommentResponse>
{
    private readonly ITaskRepository _taskRepository;

    public AddCommentHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<AddCommentResponse> Handle(AddCommentRequest request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId);

        if (task == null)
            throw new KeyNotFoundException("Tarefa não encontrada.");

        if (string.IsNullOrWhiteSpace(request.Content))
            throw new ArgumentException("O conteúdo do comentário é obrigatório.");

        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            TaskId = request.TaskId,
            Content = request.Content,
            CreatedByUserId = request.CreatedByUserId,
            CreatedAt = DateTime.UtcNow
        };

        var history = new TaskHistory
        {
            Id = Guid.NewGuid(),
            TaskId = request.TaskId,
            ChangedAt = DateTime.UtcNow,
            ChangedByUserId = request.CreatedByUserId,
            ChangeDescription = "Comentário adicionado."
        };

        await _taskRepository.AddCommentAsync(comment);
        await _taskRepository.AddTaskHistoryAsync(history);
        await _taskRepository.SaveChangesAsync();

        return new AddCommentResponse
        {
            Id = comment.Id,
            TaskId = comment.TaskId,
            Content = comment.Content
        };
    }

}
