using Microsoft.AspNetCore.Mvc;
using TaskManagement.Infrastructure.Services;

namespace TaskManagement.Api.Controllers;

[ApiController]
[Route("reports")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("performance")]
    public async Task<IActionResult> GetPerformanceReport([FromQuery] string role)
    {
        if (!string.Equals(role, "manager", StringComparison.OrdinalIgnoreCase))
            return Forbid("Access restricted to managers.");

        var average = await _reportService.GetAverageCompletedTasksPerUserAsync();
        return Ok(new { AverageCompletedTasks = average });
    }
}
