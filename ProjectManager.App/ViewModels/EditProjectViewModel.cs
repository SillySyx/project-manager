using ProjectManager.Components;
using ProjectManager.Components.MVVM;
using ProjectManager.DTO;
using ProjectManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManager.ViewModels
{
    public class EditProjectViewModel : ViewModelBase
    {
        #region Properties

        protected Guid ProjectId { get; set; }

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

        private IProject _selectedParent { get; set; }
        public IProject SelectedParent
        {
            get { return _selectedParent; }
            set { SetValue((SelectedParent) => _selectedParent, value); }
        }

        private IEnumerable<IProject> _parents { get; set; }
        public IEnumerable<IProject> Parents
        {
            get { return _parents; }
            set { SetValue((Parents) => _parents, value); }
        }

        #endregion // Properties

        #region Commands

        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new DelegateCommand(Save, CanSave));
            }
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

        public IProjectService ProjectService;

        public EditProjectViewModel(IProjectService projectService, ILanguageService languageService)
            : base(languageService)
        {
            ProjectService = projectService;

            LoadLanguage("Common", "EditProject");
            LoadParents();
        }

        protected void LoadParents()
        {
            var list = new List<IProject>();
            list.Add(new Project
            {
                Id = Guid.Empty,
                Name = "",
                Parent = null,
                Description = "",
            });

            var projects = ProjectService.GetProjects();
            foreach (var project in projects)
            {
                list.Add(new Project
                {
                    Id = project.Id,
                    Name = ProjectService.GetFullName(project, projects),
                    Parent = project.Parent
                });
            }
            Parents = list.OrderBy(p => p.Name);
        }

        public void LoadProject(Guid id)
        {
            ProjectId = id;
            var project = ProjectService.GetProject(ProjectId);

            Name = project.Name;
            Description = project.Description;

            Parents = Parents.Where(p => p.Id != ProjectId && !ProjectService.IsChildOf(p.Id, project.Id));

            SelectedParent = Parents.FirstOrDefault(p => p.Id == project.Parent);
        }

        protected void Save()
        {
            var project = new Project
            {
                Id = ProjectId,
                Name = Name,
                Description = Description,
            };
            if (SelectedParent != null) project.Parent = SelectedParent.Id;

            ProjectService.UpdateProject(project);

            ViewManager.UpdateProjectList();
            ViewManager.OpenProject(ProjectId);
        }

        protected bool CanSave()
        {
            return !String.IsNullOrWhiteSpace(Name);
        }

        protected void GoBack()
        {
            ViewManager.OpenProject(ProjectId);
        }
    }
}
