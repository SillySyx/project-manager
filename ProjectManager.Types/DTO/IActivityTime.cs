using System;

namespace ProjectManager.DTO
{
    public interface IActivityTime
    {
        Guid Id { get; set; }
        Guid ActivityId { get; set; }

        DateTime Timestamp { get; set; }
        int Hours { get; set; }
        string Comment { get; set; }
        bool Reported { get; set; }
    }
}
