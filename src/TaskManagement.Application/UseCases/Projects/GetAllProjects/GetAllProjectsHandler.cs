using MediatR;
using TaskManagement.Application.Repositories;

namespace TaskManagement.Application.UseCases.Projects.GetAllProjects;

public class GetAllProjectsHandler : IRequestHandler<GetAllProjectsRequest, GetAllProjectsResponse>
{
    private readonly IProjectRepository _projectRepository;

    public GetAllProjectsHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<GetAllProjectsResponse> Handle(GetAllProjectsRequest request, CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
            throw new ArgumentException("UserId é obrigatório.");

        var projects = await _projectRepository.GetProjectsByUserAsync(request.UserId);

        return new GetAllProjectsResponse
        {
            Projects = projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                UserId = p.UserId,
                CreatedAt = p.CreatedAt
            }).ToList()
        };
    }
}
