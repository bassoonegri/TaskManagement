namespace TaskManagement.Infrastructure.Services;

public interface IReportService
{
    Task<double> GetAverageCompletedTasksPerUserAsync();
}
