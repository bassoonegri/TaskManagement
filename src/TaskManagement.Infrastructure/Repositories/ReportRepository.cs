using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Entities.Interface;
using TaskManagement.Infrastructure.Context;

namespace TaskManagement.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly TaskManagementDbContext _dbContext;

        public ReportRepository(TaskManagementDbContext dbContext)
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
                return 0;

            return completedTasks.Average();
        }

    }
}
