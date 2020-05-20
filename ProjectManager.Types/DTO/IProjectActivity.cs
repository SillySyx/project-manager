using System;

namespace ProjectManager.DTO
{
    public interface IProjectActivity
    {
        Guid Id { get; set; }
        Guid ProjectId { get; set; }

        string Name { get; set; }
        string Description { get; set; }

        int TimeBudget { get; set; }

        DateTime Deadline { get; set; }
    }
}
