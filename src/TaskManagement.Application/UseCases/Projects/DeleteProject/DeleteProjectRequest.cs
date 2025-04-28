using MediatR;

namespace TaskManagement.Application.UseCases.Projects.DeleteProject;

public class DeleteProjectRequest : IRequest<DeleteProjectResponse>
{
    public Guid ProjectId { get; set; }
}
