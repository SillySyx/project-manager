using ProjectManager.DTO;
using System;
using System.Collections.Generic;

namespace ProjectManager.Services
{
    public interface IProjectActivityService
    {
        IProjectActivity GetProjectActivity(Guid id);

        IEnumerable<IProjectActivity> GetProjectActivities();

        void AddProjectActivity(IProjectActivity activity);

        void DeleteProjectActivity(IProjectActivity activity);

        bool UpdateProjectActivity(IProjectActivity activity);
    }
}
