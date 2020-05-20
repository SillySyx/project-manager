using ProjectManager.Components;
using ProjectManager.Components.MVVM;
using ProjectManager.DTO;
using ProjectManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProjectManager.ViewModels
{
    public class ProjectViewModel : ViewModelBase
    {
        #region Properties

        private IProject _project { get; set; }
        public IProject Project
        {
            get { return _project; }
            set { SetValue((Project) => _project, value); }
        }

        private string _projectFullName { get; set; }
        public string ProjectFullName
        {
            get { return _projectFullName; }
            set { SetValue((ProjectFullName) => _projectFullName, value); }
        }

        private ObservableCollection<ActivityListItem> _activities = new ObservableCollection<ActivityListItem>();
        public ObservableCollection<ActivityListItem> Activities
        {
            get { return _activities; }
            set { SetValue((Activities) => _activities, value); }
        }

        #endregion // Properties

        #region Commands

        private DelegateCommand _newActivityCommand;
        public DelegateCommand NewActivityCommand
        {
            get { return _newActivityCommand ?? (_newActivityCommand = new DelegateCommand(NewActivity)); }
        }

        private DelegateCommand _editProjectCommand;
        public DelegateCommand EditProjectCommand
        {
            get
            {
                return _editProjectCommand ?? (_editProjectCommand = new DelegateCommand(EditProject));
            }
        }

        private DelegateCommand _deleteProjectCommand;
        public DelegateCommand DeleteProjectCommand
        {
            get
            {
                return _deleteProjectCommand ?? (_deleteProjectCommand = new DelegateCommand(DeleteProject));
            }
        }

        #endregion // Commands

        public IProjectService ProjectService;
        public IProjectActivityService ProjectActivityService;
        public IActivityTimeService ActivityTimeService;

        public ProjectViewModel(IProjectService projectService, IProjectActivityService projectActivityService, IActivityTimeService activityTimeService, ILanguageService languageService)
            : base(languageService)
        {
            ProjectService = projectService;
            ProjectActivityService = projectActivityService;
            ActivityTimeService = activityTimeService;

            LoadLanguage("Common", "Project");
        }

        public void LoadProject(Guid id)
        {
            Project = ProjectService.GetProject(id);
            ProjectFullName = ProjectService.GetFullName(Project);

            LoadActivities();
        }

        protected void LoadActivities()
        {
            Activities.Clear();
            foreach (var activity in ProjectActivityService.GetProjectActivities().Where(a => a.ProjectId == Project.Id).OrderBy(a => a.Name))
            {
                var timeUsed = 0;
                foreach (var time in ActivityTimeService.GetAllActivityTimes().Where(t => t.ActivityId == activity.Id))
                {
                    timeUsed += time.Hours;
                }

                var item = new ActivityListItem(ProjectActivityService, LanguageService)
                {
                    Id = activity.Id,
                    Name = activity.Name,
                    Deadline = activity.Deadline,
                    TimeBudget = activity.TimeBudget,
                    TimeUsed = timeUsed,
                };

                Activities.Add(item);
            }
        }

        protected void NewActivity()
        {
            ViewManager.NewActivity(Project.Id);
        }

        protected void EditProject()
        {
            ViewManager.EditProject(Project.Id);
        }

        protected void DeleteProject()
        {
            if (MessageBox.Show(Language["DeleteProjectText"], Language["DeleteProject"], MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ProjectService.DeleteProject(Project);

                ViewManager.UpdateProjectList();
                MainViewModel.SetPageContent(null);
            }
        }

        public class ActivityListItem : ViewModelBase
        {
            public Guid Id { get; set; }

            private string _name { get; set; }
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            private int _timeBudget { get; set; }
            public int TimeBudget
            {
                get { return _timeBudget; }
                set { _timeBudget = value; }
            }

            private int _timeUsed { get; set; }
            public int TimeUsed
            {
                get { return _timeUsed; }
                set { _timeUsed = value; }
            }

            private DateTime _deadline { get; set; }
            public DateTime Deadline
            {
                get { return _deadline; }
                set { _deadline = value; }
            }

            private DelegateCommand _openCommand;
            public DelegateCommand OpenCommand
            {
                get
                {
                    return _openCommand ?? (_openCommand = new DelegateCommand(Open));
                }
            }

            private DelegateCommand _deleteCommand;
            public DelegateCommand DeleteCommand
            {
                get
                {
                    return _deleteCommand ?? (_deleteCommand = new DelegateCommand(Delete));
                }
            }

            protected IProjectActivityService ProjectActivityService;

            public ActivityListItem(IProjectActivityService projectActivityService, ILanguageService languageService)
                : base(languageService)
            {
                ProjectActivityService = projectActivityService;
                
                LoadLanguage("Common", "Activity");
            }

            protected void Open()
            {
                ViewManager.OpenActivity(Id);
            }

            protected void Delete()
            {
                if (MessageBox.Show(Language["DeleteActivityText"], Language["DeleteActivity"], MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var activity = ProjectActivityService.GetProjectActivity(Id);
                    ProjectActivityService.DeleteProjectActivity(activity);

                    ViewManager.OpenProject(activity.ProjectId);
                }
            }
        }
    }
}
