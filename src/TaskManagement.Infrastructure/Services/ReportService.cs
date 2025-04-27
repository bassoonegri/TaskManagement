using Microsoft.EntityFrameworkCore;
using TaskManagement.Infrastructure.Context;

namespace TaskManagement.Infrastructure.Services;

public class ReportService : IReportService
{
    private readonly TaskManagementDbContext _dbContext;

    public ReportService(TaskManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<double> GetAverageCompletedTasksPerUserAsync()
    {
        var completedTasks = await _dbContext.Tasks
            .Where(t => t.Status == TaskManagement.Application.Entities.TaskStatus.Done)
            .GroupBy(t => t.Project.UserId)
            .Select(g => g.Count())
            .ToListAsync();

        if (!completedTasks.Any())
            throw new InvalidOperationException("Nenhuma tarefa concluída encontrada para calcular o relatório.");

        return completedTasks.Average();
    }
}
