using System;
using System.Collections.Generic;

namespace TaskManagement.Application.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<ProjectTask> Tasks { get; set; }
    }
}
