using MediatR;
using TaskManagement.Application.Repositories;

namespace TaskManagement.Application.UseCases.Reports.GetPerformanceReport;

public class GetPerformanceReportHandler : IRequestHandler<GetPerformanceReportRequest, GetPerformanceReportResponse>
{
    private readonly ITaskRepository _taskRepository;

    public GetPerformanceReportHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<GetPerformanceReportResponse> Handle(GetPerformanceReportRequest request, CancellationToken cancellationToken)
    {
        if (!string.Equals(request.Role, "manager", StringComparison.OrdinalIgnoreCase))
            throw new UnauthorizedAccessException("Acesso restrito a gerentes.");

        var average = await _taskRepository.GetAverageCompletedTasksPerUserAsync();

        return new GetPerformanceReportResponse
        {
            AverageCompletedTasks = average
        };
    }
}
