using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using ProjectManager.Components.MVVM;
using ProjectManager.Views;
using ProjectManager.Services;
using ProjectManager.Components;

namespace ProjectManager.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties

        private UserControl _sideContent { get; set; }
        public UserControl SideContent
        {
            get { return _sideContent; }
            set { SetValue((SideContent) => _sideContent, value); }
        }

        private UserControl _pageContent { get; set; }
        public UserControl PageContent
        {
            get { return _pageContent; }
            set { SetValue((PageContent) => _pageContent, value); }
        }

        #endregion // Properties

        protected static MainViewModel instance;

        public MainViewModel(ILanguageService languageService)
            : base(languageService)
        {
            instance = this;

            SideContent = ViewManager.GetViewAndViewModel<ProjectsListView>();
        }

        public static void SetPageContent(UserControl content)
        {
            instance.PageContent = content;
        }

        public static UserControl GetPageContent()
        {
            return instance.PageContent;
        }

        public static void SetSideContent(UserControl content)
        {
            instance.SideContent = content;
        }

        public static UserControl GetSideContent()
        {
            return instance.SideContent;
        }
    }
}
