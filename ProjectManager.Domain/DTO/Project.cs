using System;

namespace ProjectManager.DTO
{
    public class Project : IProject
    {
        public Guid Id { get; set; }
        public Guid? Parent { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
