using ProjectManager.Components;
using ProjectManager.Properties;
using ProjectManager.Views;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ProjectManager
{
    public partial class App : Application
    {
        protected Window CreateWindow()
        {
            var settings = Settings.Default;
            var window = new Window
            {
                Title = "ProjectManager",
                MinWidth = 700,
                MinHeight = 500,
                Width = settings.WindowSize.Width,
                Height = settings.WindowSize.Height,
                Left = settings.WindowPosition.X,
                Top = settings.WindowPosition.Y
            };
            window.Closing += (s, arg) =>
            {
                var win = s as Window;

                settings.WindowSize = new Size(win.Width, win.Height);
                settings.WindowPosition = new Point(win.Left, win.Top);

                settings.Save();
            };
            return window;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = CreateWindow();
            MainWindow.Content = ViewManager.GetViewAndViewModel<MainView>();
            MainWindow.Show();
            ViewManager.OpenReport();
        }
    }
}
