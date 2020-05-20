using ProjectManager.DTO;
using System;
using System.Linq;

namespace ProjectManager.Services
{
    public interface IActivityTimeService
    {
        IActivityTime AddActivityTime(IActivityTime time);

        IActivityTime GetActivityTime(Guid id);

        IQueryable<IActivityTime> GetAllActivityTimes();

        IQueryable<IActivityTime> GetAllActivityTimes(Guid activityId);

        void DeleteActivityTime(Guid id);

        void DeleteActivityTimes(Guid activityId);

        bool UpdateActivityTime(IActivityTime time);
    }
}
