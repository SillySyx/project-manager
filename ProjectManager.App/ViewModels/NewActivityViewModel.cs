using ProjectManager.Components;
using ProjectManager.Components.MVVM;
using ProjectManager.DTO;
using ProjectManager.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ProjectManager.ViewModels
{
    public class NewActivityViewModel : ViewModelBase
    {
        #region Properties

        protected Guid GoBackGuid;

        private string _name { get; set; }
        public string Name
        {
            get { return _name; }
            set
            {
                if (SetValue((Name) => _name, value))
                {
                    SaveCommand.NotifyCanExecuteChanged();
                }
            }
        }

        private string _description { get; set; }
        public string Description
        {
            get { return _description; }
            set { SetValue((Description) => _description, value); }
        }

        private int _timeBudget { get; set; }
        public int TimeBudget
        {
            get { return _timeBudget; }
            set { SetValue((TimeBudget) => _timeBudget, value); }
        }

        private DateTime _deadline { get; set; }
        public DateTime Deadline
        {
            get { return _deadline == DateTime.MinValue ? (_deadline = DateTime.Now) : _deadline; }
            set { SetValue((Deadline) => _deadline, value); }
        }

        private ObservableCollection<IProject> _projects { get; set; }
        public ObservableCollection<IProject> Projects
        {
            get { return _projects ?? (_projects = new ObservableCollection<IProject>()); }
            set { SetValue((Projects) => _projects, value); }
        }

        private IProject _selectedProject { get; set; }
        public IProject SelectedProject
        {
            get { return _selectedProject; }
            set { SetValue((SelectedProject) => _selectedProject, value); }
        }

        #endregion // Properties

        #region Commands

        private DelegateCommand _saveCommand { get; set; }
        public DelegateCommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new DelegateCommand(Save, CanSave)); }
        }

        private DelegateCommand _goBackCommand { get; set; }
        public DelegateCommand GoBackCommand
        {
            get { return _goBackCommand ?? (_goBackCommand = new DelegateCommand(GoBack)); }
        }

        #endregion // Commands

        protected IProjectService ProjectService;
        protected IProjectActivityService ProjectActivityService;

        public NewActivityViewModel(IProjectService projectService, IProjectActivityService projectActivityService, ILanguageService languageService)
            : base(languageService)
        {
            ProjectService = projectService;
            ProjectActivityService = projectActivityService;

            LoadLanguage("Common", "NewActivity");
            LoadProjects();
        }

        public void SetSelectedProject(Guid id)
        {
            GoBackGuid = id;
            foreach (var project in Projects)
            {
                if (project.Id == id)
                {
                    SelectedProject = project;
                    break;
                }
            }
        }

        protected void LoadProjects()
        {
            Projects.Clear();
            foreach (var project in ProjectService.GetProjects())
            {
                var p = new Project
                {
                    Id = project.Id,
                    Name = ProjectService.GetFullName(project),
                    Parent = project.Parent,
                };
                Projects.Add(p);
            }
        }

        protected void Save()
        {
            var activity = new ProjectActivity
            {
                Name = Name,
                Description = Description,
                Deadline = Deadline,
                ProjectId = SelectedProject.Id,
                TimeBudget = TimeBudget,
            };
            ProjectActivityService.AddProjectActivity(activity);

            ViewManager.OpenProject(SelectedProject.Id);
        }

        protected bool CanSave()
        {
            return !String.IsNullOrWhiteSpace(Name);
        }

        protected void GoBack()
        {
            ViewManager.OpenProject(GoBackGuid);
        }
    }
}
