using ProjectManager.Components;
using ProjectManager.Components.MVVM;
using ProjectManager.DTO;
using ProjectManager.Services;
using ProjectManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProjectManager.ViewModels
{
    public class ProjectsListViewModel : ViewModelBase
    {
        #region Commands

        private ICommand _newProjectCommand { get; set; }
        public ICommand NewProjectCommand
        {
            get { return _newProjectCommand ?? (_newProjectCommand = new DelegateCommand(NewProject)); }
        }

        private ICommand _openProjectCommand { get; set; }
        public ICommand OpenProjectCommand
        {
            get { return _openProjectCommand ?? (_openProjectCommand = new DelegateCommand(OpenProject)); }
        }

        private ICommand _deleteProjectCommand { get; set; }
        public ICommand DeleteProjectCommand
        {
            get { return _deleteProjectCommand ?? (_deleteProjectCommand = new DelegateCommand(DeleteProject)); }
        }

        private ICommand _timeReportCommand { get; set; }
        public ICommand TimeReportCommand
        {
            get { return _timeReportCommand ?? (_timeReportCommand = new DelegateCommand(OpenReport)); }
        }

        #endregion // Commands

        public IProjectService ProjectService;
        public IProjectActivityService ProjectActivityService;
        public IActivityTimeService ActivityTimeService;

        public ObservableCollection<TreeViewItem> Projects { get; set; }

        public ProjectsListViewModel(IProjectService projectService, IProjectActivityService projectActivityService, IActivityTimeService activityTimeService, ILanguageService languageService)
            : base(languageService)
        {
            ProjectService = projectService;
            ProjectActivityService = projectActivityService;
            ActivityTimeService = activityTimeService;

            LoadLanguage("Common", "Projects");

            Projects = new ObservableCollection<TreeViewItem>();
            LoadProjects();
        }

        protected TreeViewItem FindNode(IEnumerable<TreeViewItem> nodes, Guid? id)
        {
            if (id == null) return null;

            foreach (var node in nodes)
            {
                var nodeProject = node.Tag as Project;
                if (nodeProject.Id == id) return node;

                if (node.Items.Count > 0)
                {
                    var parent = FindNode(node.Items.Cast<TreeViewItem>(), id);
                    if (parent != null)
                    {
                        return parent;
                    }
                }
            }
            return null;
        }

        public void LoadProjects()
        {
            Projects.Clear();
            foreach (var project in ProjectService.GetProjects().OrderBy(p => p.Parent).ThenBy(p => p.Name))
            {
                AddNewProjectToList(project);
            }
        }

        public void AddNewProjectToList(IProject project)
        {
            var menu = new ContextMenu();
            menu.Items.Add(new MenuItem
            {
                Header = Language["Open"],
                Command = OpenProjectCommand,
                CommandParameter = project
            });
            menu.Items.Add(new MenuItem
            {
                Header = Language["Delete"],
                Command = DeleteProjectCommand,
                CommandParameter = project
            });

            var node = new TreeViewItem
            {
                Header = project.Name,
                Style = App.Current.TryFindResource("StyledTreeViewItem") as Style,
                Tag = project,
                ContextMenu = menu
            };
            node.MouseDoubleClick += (s, e) =>
            {
                if (!(s as TreeViewItem).IsSelected) return;

                OpenProjectCommand.Execute(project);
                e.Handled = true;
            };

            var parent = FindNode(Projects, project.Parent);
            if (parent != null)
            {
                parent.Items.Add(node);
            }
            else
            {
                Projects.Add(node);
            }
        }

        protected void NewProject()
        {
            ViewManager.NewProject();
        }

        protected void OpenProject(object parameter)
        {
            var project = parameter as Project;
            ViewManager.OpenProject(project.Id);
        }

        protected void DeleteProject(object parameter)
        {
            if (MessageBox.Show(Language["DeleteProjectText"], Language["DeleteProject"], MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var project = parameter as Project;
                var node = FindNode(Projects, project.Id);
                var parent = FindNode(Projects, project.Parent);
                if (parent != null)
                {
                    parent.Items.Remove(node);
                }
                else
                {
                    Projects.Remove(node);
                }

                ProjectService.DeleteProject(project);
                LoadProjects();
            }
        }

        protected void OpenReport()
        {
            ViewManager.OpenReport();
        }
    }
}
