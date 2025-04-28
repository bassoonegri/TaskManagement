using MediatR;

namespace TaskManagement.Application.UseCases.Reports.GetPerformanceReport;

public class GetPerformanceReportRequest : IRequest<GetPerformanceReportResponse>
{
    public string Role { get; set; }
}
