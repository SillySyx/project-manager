using ProjectManager.DTO;
using ProjectManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManager.Services
{
    public class ActivityTimeService : IActivityTimeService
    {
        protected IActivityTimeRepository Repository;

        public ActivityTimeService(IActivityTimeRepository repository)
        {
            Repository = repository;
        }

        public IActivityTime AddActivityTime(IActivityTime time)
        {
            return Repository.Add(time);
        }

        public IActivityTime GetActivityTime(Guid id)
        {
            return Repository.Get(id);
        }

        public IQueryable<IActivityTime> GetAllActivityTimes()
        {
            return Repository.GetAll();
        }

        public IQueryable<IActivityTime> GetAllActivityTimes(Guid activityId)
        {
            return Repository.GetAll().Where(a => a.ActivityId == activityId);
        }

        public void DeleteActivityTime(Guid id)
        {
            var time = Repository.Get(id);
            Repository.Delete(time);
        }

        public void DeleteActivityTimes(Guid activityId)
        {
            var times = Repository.GetAll().Where(a => a.ActivityId == activityId).ToList();
            foreach (var time in times)
            {
                Repository.Delete(time);
            }
        }

        public bool UpdateActivityTime(IActivityTime time)
        {
            return Repository.Update(time);
        }
    }
}
