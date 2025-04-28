using MediatR;
using TaskManagement.Application.Entities;
using TaskManagement.Application.Repositories;

namespace TaskManagement.Application.UseCases.Projects.CreateProject;

public class CreateProjectHandler : IRequestHandler<CreateProjectRequest, CreateProjectResponse>
{
    private readonly IProjectRepository _projectRepository;

    public CreateProjectHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<CreateProjectResponse> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Nome do projeto é obrigatório.");

        if (request.UserId == Guid.Empty)
            throw new ArgumentException("Usuário do projeto é obrigatório.");

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow
        };

        await _projectRepository.CreateAsync(project);
        await _projectRepository.SaveChangesAsync();

        return new CreateProjectResponse
        {
            Id = project.Id,
            Name = project.Name,
            UserId = project.UserId,
            CreatedAt = project.CreatedAt
        };
    }
}
