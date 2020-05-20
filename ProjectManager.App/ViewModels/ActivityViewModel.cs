using ProjectManager.Components;
using ProjectManager.Components.MVVM;
using ProjectManager.DTO;
using ProjectManager.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ProjectManager.ViewModels
{
    public class ActivityViewModel : ViewModelBase
    {
        #region Properties

        private string _projectFullName { get; set; }
        public string ProjectFullName
        {
            get { return _projectFullName; }
            set { SetValue((ProjectFullName) => _projectFullName, value); }
        }

        private IProjectActivity _activity { get; set; }
        public IProjectActivity Activity
        {
            get { return _activity; }
            set { SetValue((Activity) => _activity, value); }
        }

        private ObservableCollection<ActivityTimeListItem> _times = new ObservableCollection<ActivityTimeListItem>();
        public ObservableCollection<ActivityTimeListItem> Times
        {
            get { return _times; }
            set { SetValue((Times) => _times, value); }
        }

        private int _timeUsed { get; set; }
        public int TimeUsed
        {
            get { return _timeUsed; }
            set { SetValue((TimeUsed) => _timeUsed, value); }
        }

        private string _timeUsedFactor { get; set; }
        public string TimeUsedFactor
        {
            get { return _timeUsedFactor; }
            set { SetValue((TimeUsedFactor) => _timeUsedFactor, value); }
        }

        #endregion // Properties

        #region Commands

        private DelegateCommand _reportTimeCommand { get; set; }
        public DelegateCommand ReportTimeCommand
        {
            get { return _reportTimeCommand ?? (_reportTimeCommand = new DelegateCommand(ReportTime)); }
        }
        

        private DelegateCommand _editActivityCommand { get; set; }
        public DelegateCommand EditActivityCommand
        {
            get { return _editActivityCommand ?? (_editActivityCommand = new DelegateCommand(EditActivity)); }
        }

        private DelegateCommand _deleteActivityCommand { get; set; }
        public DelegateCommand DeleteActivityCommand
        {
            get { return _deleteActivityCommand ?? (_deleteActivityCommand = new DelegateCommand(DeleteActivity)); }
        }

        #endregion // Commands

        protected IProjectService ProjectService;
        protected IProjectActivityService ProjectActivityService;
        protected IActivityTimeService ActivityTimeService;

        public ActivityViewModel(IProjectService projectService, IProjectActivityService projectActivityService, IActivityTimeService activityTimeService, ILanguageService languageService)
            : base(languageService)
        {
            ProjectService = projectService;
            ProjectActivityService = projectActivityService;
            ActivityTimeService = activityTimeService;

            LoadLanguage("Common", "Activity");
        }

        public void LoadActivity(Guid id)
        {
            Activity = ProjectActivityService.GetProjectActivity(id);

            ProjectFullName = ProjectService.GetFullName(ProjectService.GetProject(Activity.ProjectId));

            LoadActivityTimes(id);
        }

        public void LoadActivityTimes(Guid id)
        {
            Times.Clear();
            var used = 0;
            foreach (var time in ActivityTimeService.GetAllActivityTimes(id).OrderByDescending(time => time.Timestamp))
            {
                used += time.Hours;
                Times.Add(new ActivityTimeListItem(time, ActivityTimeService, LanguageService));
            }
            TimeUsed = used;
            
            if (Activity.TimeBudget == 0)
            {
                TimeUsedFactor = "";
                return;
            }

            var factor = (double)used / (double)Activity.TimeBudget * 100;
            TimeUsedFactor = (int)factor + "%";
        }

        protected void EditActivity()
        {
            ViewManager.EditActivity(Activity.Id);
        }

        protected void DeleteActivity()
        {
            if (MessageBox.Show(Language["DeleteActivityText"], Language["DeleteActivity"], MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ProjectActivityService.DeleteProjectActivity(Activity);

                ViewManager.OpenProject(Activity.ProjectId);
            }
        }

        protected void ReportTime()
        {
            ViewManager.OpenTimeReport(Activity.Id);
        }

        public class ActivityTimeListItem : ViewModelBase
        {
            public Guid Id { get; set; }
            public Guid ActivityId { get; set; }

            public DateTime Timestamp { get; set; }
            public int Hours { get; set; }
            public string Comment { get; set; }

            private bool _reported { get; set; }
            public bool Reported
            {
                get { return _reported; }
                set { SetValue((Reported) => _reported, value); }
            }

            private DelegateCommand _deleteCommand { get; set; }
            public DelegateCommand DeleteCommand
            {
                get { return _deleteCommand ?? (_deleteCommand = new DelegateCommand(Delete)); }
            }

            private DelegateCommand _editCommand { get; set; }
            public DelegateCommand EditCommand
            {
                get { return _editCommand ?? (_editCommand = new DelegateCommand(Edit)); }
            }

            private DelegateCommand _toggleReportedCommand { get; set; }
            public DelegateCommand ToggleReportedCommand
            {
                get { return _toggleReportedCommand ?? (_toggleReportedCommand = new DelegateCommand(ToggleReported)); }
            }

            protected IActivityTimeService Service;

            public ActivityTimeListItem(IActivityTime time, IActivityTimeService service, ILanguageService languageService)
                : base(languageService)
            {
                Service = service;

                ActivityId = time.ActivityId;
                Comment = time.Comment;
                Hours = time.Hours;
                Id = time.Id;
                Timestamp = time.Timestamp;
                Reported = time.Reported;

                LoadLanguage("Common", "Time");
            }

            protected void Delete()
            {
                if (MessageBox.Show(Language["DeleteTime"], Language["DeleteTimeText"], MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Service.DeleteActivityTime(Id);
                    ViewManager.OpenActivity(ActivityId);
                }
            }

            protected void Edit()
            {
                ViewManager.EditTime(Id);
            }

            protected void ToggleReported()
            {
                var value = !Reported;

                var activity = Service.GetActivityTime(Id);
                if (activity != null)
                {
                    activity.Reported = value;
                    Service.UpdateActivityTime(activity);
                }

                Reported = value;
            }
        }
    }
}
