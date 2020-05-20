using ProjectManager.DTO;
using ProjectManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManager.Services
{
    public class ProjectActivityService : IProjectActivityService
    {
        protected IProjectActivityRepository Repository;
        protected IActivityTimeService ActivityTimeService;

        public ProjectActivityService(IProjectActivityRepository repository, IActivityTimeService activityTimeService)
        {
            Repository = repository;
            ActivityTimeService = activityTimeService;
        }

        public IProjectActivity GetProjectActivity(Guid id)
        {
            return Repository.Get(id);
        }

        public IEnumerable<IProjectActivity> GetProjectActivities()
        {
            return Repository.GetAll().ToList();
        }

        public void AddProjectActivity(IProjectActivity activity)
        {
            Repository.Add(activity);
        }

        public void DeleteProjectActivity(IProjectActivity activity)
        {
            ActivityTimeService.DeleteActivityTimes(activity.Id);

            Repository.Delete(activity);
        }

        public bool UpdateProjectActivity(IProjectActivity activity)
        {
            return Repository.Update(activity);
        }
    }
}
