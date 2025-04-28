using System;

namespace TaskManagement.Application.UseCases.Tasks.UpdateTask;

public class UpdateTaskResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
}
