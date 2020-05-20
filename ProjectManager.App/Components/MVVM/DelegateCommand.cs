using System;
using System.Windows.Input;

namespace ProjectManager.Components.MVVM
{
    public class DelegateCommand : ICommand
    {
        protected Action<object> executeCommand;
        protected Func<object, bool> canExecuteCommand;

        // new DelegateCommand(() => {}, () => {});
        public DelegateCommand(Action executeCommand, Func<bool> canExecuteCommand = null)
        {
            this.executeCommand = new Action<object>((p) => executeCommand());
            if (canExecuteCommand != null)
            {
                this.canExecuteCommand = new Func<object, bool>((p) => canExecuteCommand());
            }
        }

        // new DelegateCommand((p) => {}, (p) => {});
        public DelegateCommand(Action<object> executeCommand, Func<object, bool> canExecuteCommand = null)
        {
            this.executeCommand = executeCommand;
            if (canExecuteCommand != null)
            {
                this.canExecuteCommand = canExecuteCommand;
            }
        }

        public void NotifyCanExecuteChanged() 
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        #region ICommand

        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter = null)
        {
            return canExecuteCommand == null ? true : canExecuteCommand(parameter);
        }

        public void Execute(object parameter = null)
        {
            executeCommand(parameter);
        }

        #endregion // ICommand
    }
}
