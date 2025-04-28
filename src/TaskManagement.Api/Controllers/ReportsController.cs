using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.UseCases.Reports.GetPerformanceReport;

namespace TaskManagement.Api.Controllers;

[ApiController]
[Route("reports")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("performance")]
    public async Task<IActionResult> GetPerformanceReport([FromQuery] string role)
    {
        if (string.IsNullOrWhiteSpace(role))
            return BadRequest("Função de usuário é obrigatória.");

        if (!string.Equals(role, "manager", StringComparison.OrdinalIgnoreCase))
            return Forbid("Acesso restrito a gerentes.");

        var result = await _mediator.Send(new GetPerformanceReportRequest { Role = role });
        return Ok(new { AverageCompletedTasks = result.AverageCompletedTasks });
    }

}