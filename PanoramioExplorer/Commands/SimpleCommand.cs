using System;
using System.Windows.Input;

namespace PanoramioExplorer.Commands
{
    public class SimpleCommand<T> : ICommand where T : class
    {
        private readonly Action<T> executeAction;

        public SimpleCommand(Action<T> executeAction)
        {
            this.executeAction = executeAction;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            executeAction(parameter as T);
        }

        public event EventHandler CanExecuteChanged;
    }
}
