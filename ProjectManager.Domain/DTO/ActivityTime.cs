using System;

namespace ProjectManager.DTO
{
    public class ActivityTime : IActivityTime
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }

        public DateTime Timestamp { get; set; }
        public int Hours { get; set; }
        public string Comment { get; set; }
        public bool Reported { get; set; }
    }
}
