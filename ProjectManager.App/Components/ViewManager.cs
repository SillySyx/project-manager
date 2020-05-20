using ProjectManager.Services;
using ProjectManager.ViewModels;
using ProjectManager.Views;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ProjectManager.Components
{
    public class ViewManager
    {
        protected static IDictionary<Type, Type> ViewMappings;

        static ViewManager()
        {
            ViewMappings = new Dictionary<Type, Type>();

            ViewMappings.Add(typeof(ActivityView), typeof(ActivityViewModel));
            ViewMappings.Add(typeof(EditActivityView), typeof(EditActivityViewModel));
            ViewMappings.Add(typeof(EditProjectView), typeof(EditProjectViewModel));
            ViewMappings.Add(typeof(EditTimeView), typeof(EditTimeViewModel));
            ViewMappings.Add(typeof(MainView), typeof(MainViewModel));
            ViewMappings.Add(typeof(NewActivityView), typeof(NewActivityViewModel));
            ViewMappings.Add(typeof(NewProjectView), typeof(NewProjectViewModel));
            ViewMappings.Add(typeof(NewTimeView), typeof(NewTimeViewModel));
            ViewMappings.Add(typeof(ProjectsListView), typeof(ProjectsListViewModel));
            ViewMappings.Add(typeof(ProjectView), typeof(ProjectViewModel));
            ViewMappings.Add(typeof(ReportView), typeof(ReportViewModel));
        }

        public static UserControl GetViewAndViewModel<T>()
        {
            var viewType = typeof(T);
            var view = Activator.CreateInstance(viewType) as UserControl;

            Type viewModelType;
            if (ViewMappings.TryGetValue(viewType, out viewModelType))
            {
                var viewModel = typeof(UnityService).GetMethod("Resolve").MakeGenericMethod(viewModelType).Invoke(null, null);
                view.DataContext = viewModel;
            }
            return view;
        }

        public static void OpenProject(Guid id)
        {
            var view = GetViewAndViewModel<ProjectView>();
            (view.DataContext as ProjectViewModel).LoadProject(id);
            MainViewModel.SetPageContent(view);
        }

        public static void NewActivity(Guid id)
        {
            var view = GetViewAndViewModel<NewActivityView>();
            (view.DataContext as NewActivityViewModel).SetSelectedProject(id);
            MainViewModel.SetPageContent(view);
        }

        public static void NewProject()
        {
            var view = ViewManager.GetViewAndViewModel<NewProjectView>();
            MainViewModel.SetPageContent(view);
        }

        public static void OpenActivity(Guid id)
        {
            var view = GetViewAndViewModel<ActivityView>();
            (view.DataContext as ActivityViewModel).LoadActivity(id);
            MainViewModel.SetPageContent(view);
        }

        public static void EditProject(Guid id)
        {
            var view = GetViewAndViewModel<EditProjectView>();
            (view.DataContext as EditProjectViewModel).LoadProject(id);
            MainViewModel.SetPageContent(view);
        }

        public static void EditActivity(Guid id)
        {
            var view = GetViewAndViewModel<EditActivityView>();
            (view.DataContext as EditActivityViewModel).LoadActivity(id);
            MainViewModel.SetPageContent(view);
        }

        public static void UpdateProjectList()
        {
            var view = MainViewModel.GetSideContent();
            var viewModel = view.DataContext as ProjectsListViewModel;
            viewModel.LoadProjects();
        }

        public static void OpenTimeReport(Guid id)
        {
            var view = GetViewAndViewModel<NewTimeView>();
            (view.DataContext as NewTimeViewModel).Load(id);
            MainViewModel.SetPageContent(view);
        }

        public static void OpenReport()
        {
            var view = GetViewAndViewModel<ReportView>();
            MainViewModel.SetPageContent(view);
        }

        public static void EditTime(Guid id)
        {
            var view = GetViewAndViewModel<EditTimeView>();
            (view.DataContext as EditTimeViewModel).Load(id);
            MainViewModel.SetPageContent(view);
        }
    }
}
