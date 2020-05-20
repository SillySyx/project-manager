using Microsoft.Practices.Unity;
using ProjectManager.DTO;
using ProjectManager.Repositories;

namespace ProjectManager.Services
{
    public class UnityService
    {
        protected static UnityService _instance;
        public static UnityService Instance 
        {
            get { return _instance ?? (_instance = new UnityService()); }
        }

        protected UnityContainer container;

        public UnityService()
        {
            container = new UnityContainer();
            RegisterTypes();
        }

        protected void RegisterTypes()
        {
            // DTOs
            container.RegisterType<IActivityTime, ActivityTime>();
            container.RegisterType<IProject, Project>();
            container.RegisterType<IProjectActivity, ProjectActivity>();

            // Repositories
            container.RegisterInstance<IProjectRepository>(new ProjectRepository());
            container.RegisterInstance<IProjectActivityRepository>(new ProjectActivityRepository());
            container.RegisterInstance<IActivityTimeRepository>(new ActivityTimeRepository());

            // Services
            container.RegisterType<ILanguageService, LanguageService>();
            container.RegisterType<IProjectService, ProjectService>();
            container.RegisterType<IProjectActivityService, ProjectActivityService>();
            container.RegisterType<IActivityTimeService, ActivityTimeService>();
        }

        public static T Resolve<T>()
        {
            return Instance.container.Resolve<T>();
        }
    }
}
