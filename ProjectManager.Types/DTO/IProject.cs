using System;

namespace ProjectManager.DTO
{
    public interface IProject
    {
        Guid Id { get; set; }
        Guid? Parent { get; set; }

        string Name { get; set; }
        string Description { get; set; }
    }
}
