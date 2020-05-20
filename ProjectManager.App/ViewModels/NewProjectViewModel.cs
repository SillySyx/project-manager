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
    public class NewProjectViewModel : ViewModelBase
    {
        #region Properties

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

        #endregion // Commands

        public IProjectService ProjectService;

        public NewProjectViewModel(IProjectService projectService, ILanguageService languageService)
            : base(languageService)
        {
            ProjectService = projectService;

            LoadLanguage("Common", "NewProject");
            LoadParents();
        }

        protected void LoadParents()
        {
            var list = new List<IProject>();
            list.Add(new Project
            {
                Id = Guid.Empty,
                Name = "",
                Parent = null
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

        protected void Save()
        {
            var viewModel = MainViewModel.GetSideContent().DataContext as ProjectsListViewModel;

            var project = new Project
            {
                Name = Name,
                Description = Description,
                Parent = SelectedParent != null ? new Nullable<Guid>(SelectedParent.Id) : null
            };

            ProjectService.AddProject(project);

            viewModel.AddNewProjectToList(project);

            ViewManager.OpenProject(project.Id);
        }

        protected bool CanSave()
        {
            return !String.IsNullOrWhiteSpace(Name);
        }
    }
}
