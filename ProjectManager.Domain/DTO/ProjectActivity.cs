using System;

namespace ProjectManager.DTO
{
    public class ProjectActivity : IProjectActivity
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int TimeBudget { get; set; }

        public DateTime Deadline { get; set; }
    }
}
