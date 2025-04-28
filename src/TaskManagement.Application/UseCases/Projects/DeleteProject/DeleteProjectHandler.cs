using MediatR;
using TaskManagement.Application.Repositories;

namespace TaskManagement.Application.UseCases.Projects.DeleteProject;

public class DeleteProjectHandler : IRequestHandler<DeleteProjectRequest, DeleteProjectResponse>
{
    private readonly IProjectRepository _projectRepository;

    public DeleteProjectHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<DeleteProjectResponse> Handle(DeleteProjectRequest request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);

        if (project == null)
            throw new KeyNotFoundException("Projeto não encontrado.");

        if (project.Tasks.Any(t => t.Status == TaskManagement.Application.Entities.TaskStatus.Pending))
            throw new InvalidOperationException("Não é possível excluir um projeto com tarefas pendentes.");

        await _projectRepository.DeleteAsync(project);
        await _projectRepository.SaveChangesAsync();

        return new DeleteProjectResponse { Success = true };
    }
}
