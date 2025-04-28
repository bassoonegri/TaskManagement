using System;

namespace TaskManagement.Application.UseCases.Tasks.CreateTask;

public class CreateTaskResponse
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Title { get; set; }
}
