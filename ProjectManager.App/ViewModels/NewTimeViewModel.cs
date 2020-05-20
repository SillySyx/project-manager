using ProjectManager.Components;
using ProjectManager.Components.MVVM;
using ProjectManager.DTO;
using ProjectManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.ViewModels
{
    public class NewTimeViewModel : ViewModelBase
    {
        #region Properties

        private Guid _activityId { get; set; }
        public Guid ActivityId
        {
            get { return _activityId; }
            set
            {
                if (SetValue((ActivityId) => _activityId, value))
                {
                    SaveCommand.NotifyCanExecuteChanged();
                }
            }
        }

        private string _fullName { get; set; }
        public string FullName
        {
            get { return _fullName; }
            set { SetValue((FullName) => _fullName, value); }
        }

        private DateTime _selectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { SetValue((SelectedDate) => _selectedDate, value); }
        }

        private int _hours = 1;
        public int Hours
        {
            get { return _hours; }
            set
            {
                if (SetValue((Hours) => _hours, value))
                {
                    SaveCommand.NotifyCanExecuteChanged();
                }
            }
        }

        private string _comment { get; set; }
        public string Comment
        {
            get { return _comment; }
            set
            {
                if (SetValue((Comment) => _comment, value))
                {
                    SaveCommand.NotifyCanExecuteChanged();
                }
            }
        }

        #endregion // Properties

        #region Commands

        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new DelegateCommand(Save, CanSave)); }
        }

        private DelegateCommand _goBackCommand;
        public DelegateCommand GoBackCommand
        {
            get
            {
                return _goBackCommand ?? (_goBackCommand = new DelegateCommand(GoBack));
            }
        }

        #endregion // Commands

        public IActivityTimeService Service;
        public IProjectActivityService ProjectActivityService;
        public IProjectService ProjectService;

        public NewTimeViewModel(IActivityTimeService service, IProjectActivityService projectActivityService, IProjectService projectService, ILanguageService languageService)
            : base(languageService)
        {
            Service = service;
            ProjectActivityService = projectActivityService;
            ProjectService = projectService;

            LoadLanguage("Common");
        }

        public void Load(Guid id)
        {
            ActivityId = id;

            var activity = ProjectActivityService.GetProjectActivity(id);

            var project = ProjectService.GetProject(activity.ProjectId);

            FullName = ProjectService.GetFullName(project) + " > " + activity.Name;
        }

        protected void Save()
        {
            var time = new ActivityTime
            {
                ActivityId = ActivityId,
                Comment = Comment,
                Hours = Hours,
                Timestamp = SelectedDate,
            };
            Service.AddActivityTime(time);

            ViewManager.OpenActivity(ActivityId);
        }

        protected bool CanSave()
        {
            return
                ActivityId != Guid.Empty &&
                Hours > 0 &&
                !String.IsNullOrWhiteSpace(Comment);
        }

        protected void GoBack()
        {
            ViewManager.OpenActivity(ActivityId);
        }
    }
}
