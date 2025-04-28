namespace TaskManagement.Application.Entities.Interface;

public interface IReportRepository
{
    Task<double> GetAverageCompletedTasksPerUserAsync();

}
