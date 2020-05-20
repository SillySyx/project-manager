using ProjectManager.Components;
using ProjectManager.Components.MVVM;
using ProjectManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.ViewModels
{
    public class ReportViewModel : ViewModelBase
    {
        private DateTime _selectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (SetValue((SelectedDate) => _selectedDate, value))
                {
                    LoadTimes(value);
                }
            }
        }

        private ObservableCollection<TimeReportListItem> _times { get; set; }
        public ObservableCollection<TimeReportListItem> Times
        {
            get { return _times ?? (_times = new ObservableCollection<TimeReportListItem>()); }
            set { SetValue((Times) => _times, value); }
        }

        protected IActivityTimeService ActivityTimeService;
        protected IProjectActivityService ProjectActivityService;
        protected IProjectService ProjectService;

        public ReportViewModel(IActivityTimeService activityTimeService, IProjectActivityService projectActivityService, IProjectService projectService, ILanguageService languageService)
            : base(languageService)
        {
            ActivityTimeService = activityTimeService;
            ProjectActivityService = projectActivityService;
            ProjectService = projectService;

            LoadTimes(DateTime.Now);
        }

        public void LoadTimes(DateTime date)
        {
            Times.Clear();

            var times = ActivityTimeService.GetAllActivityTimes().Where(t =>
                t.Timestamp.Year == date.Year &&
                t.Timestamp.Month == date.Month &&
                t.Timestamp.Day == date.Day);

            var listItems = new Dictionary<string, TimeReportListItem>();

            foreach (var time in times)
            {
                var activity = ProjectActivityService.GetProjectActivity(time.ActivityId);

                var project = ProjectService.GetProject(activity.ProjectId);
                var fullName = ProjectService.GetFullName(project);

                if (!listItems.ContainsKey(fullName))
                {
                    listItems.Add(fullName, new TimeReportListItem { ProjectFullName = fullName, ActivityTimes = new ObservableCollection<ActivityListItem>() });
                }

                listItems[fullName].ActivityTimes.Add(new ActivityListItem
                {
                    Id = activity.Id,
                    Name = activity.Name,
                    Hours = time.Hours,
                    Comment = time.Comment,
                    Reported = time.Reported,
                });
            }

            foreach (var item in listItems)
            {
                Times.Add(item.Value);
            }
        }

        public class TimeReportListItem
        {
            public string ProjectFullName { get; set; }

            public ObservableCollection<ActivityListItem> ActivityTimes { get; set; }
        }

        public class ActivityListItem
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public int Hours { get; set; }
            public string Comment { get; set; }
            public bool Reported { get; set; }

            private DelegateCommand _openCommand;
            public DelegateCommand OpenCommand
            {
                get
                {
                    return _openCommand ?? (_openCommand = new DelegateCommand(Open));
                }
            }

            private DelegateCommand _copyCommentCommand;
            public DelegateCommand CopyCommentCommand
            {
                get
                {
                    return _copyCommentCommand ?? (_copyCommentCommand = new DelegateCommand(CopyComment));
                }
            }

            protected void Open()
            {
                ViewManager.OpenActivity(Id);
            }

            protected void CopyComment()
            {
                System.Windows.Clipboard.SetText(Comment);
            }
        }
    }
}
