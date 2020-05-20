using ProjectManager.DTO;
using ProjectManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManager.Services
{
    public class ProjectService : IProjectService
    {
        protected IProjectRepository ProjectRepository;
        protected IProjectActivityService ProjectActivityService;

        public ProjectService(IProjectRepository projectRepository, IProjectActivityService projectActivityService)
        {
            ProjectRepository = projectRepository;
            ProjectActivityService = projectActivityService;
        }

        public IProject GetProject(Guid id)
        {
            return ProjectRepository.Get(id);
        }

        public IEnumerable<IProject> GetProjects()
        {
            return ProjectRepository.GetAll().ToList();
        }

        public void AddProject(IProject project)
        {
            ProjectRepository.Add(project);
        }

        public void DeleteProject(IProject project)
        {
            foreach (var activity in ProjectActivityService.GetProjectActivities().Where(a => a.ProjectId == project.Id))
            {
                ProjectActivityService.DeleteProjectActivity(activity);
            }

            ProjectRepository.Delete(project);
        }

        public string GetFullName(IProject project, IEnumerable<IProject> projects = null)
        {
            if (project == null) return String.Empty;
            if (projects == null) projects = GetProjects();

            if (project.Parent.HasValue && project.Parent != Guid.Empty)
            {
                return String.Format("{0} > {1}", GetFullName(projects.FirstOrDefault(p => p.Id == project.Parent.Value), projects), project.Name);
            }
            return project.Name;
        }

        public bool IsChildOf(Guid id, Guid parentId, IEnumerable<IProject> projects = null)
        {
            if (projects == null) projects = GetProjects();

            var project = projects.FirstOrDefault(p => p.Id == id);
            if (project == null) return false;

            if (project.Parent.HasValue)
            {
                if (project.Parent.Value == parentId) return true;

                return IsChildOf(project.Parent.Value, parentId, projects);
            }
            return false;
        }

        public bool UpdateProject(IProject project)
        {
            return ProjectRepository.Update(project);
        }
    }
}
