using MediatR;

namespace TaskManagement.Application.UseCases.Projects.CreateProject;

public class CreateProjectRequest : IRequest<CreateProjectResponse>
{
    public string Name { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}