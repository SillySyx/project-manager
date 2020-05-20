using ProjectManager.DTO;
using System;
using System.Collections.Generic;

namespace ProjectManager.Services
{
    public interface IProjectService
    {
        IProject GetProject(Guid id);

        IEnumerable<IProject> GetProjects();

        void AddProject(IProject project);

        void DeleteProject(IProject project);

        string GetFullName(IProject project, IEnumerable<IProject> projects = null);

        bool UpdateProject(IProject project);

        bool IsChildOf(Guid id, Guid parentId, IEnumerable<IProject> projects = null);
    }
}
