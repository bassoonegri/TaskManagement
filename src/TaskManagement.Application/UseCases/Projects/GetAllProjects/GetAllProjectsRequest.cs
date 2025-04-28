using MediatR;

namespace TaskManagement.Application.UseCases.Projects.GetAllProjects;

public class GetAllProjectsRequest : IRequest<GetAllProjectsResponse>
{
    public Guid UserId { get; set; }
}
