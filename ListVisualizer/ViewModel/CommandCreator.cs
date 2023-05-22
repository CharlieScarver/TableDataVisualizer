using System;
using System.Windows.Input;

namespace ListVisualizer.ViewModel
{
    public class CommandCreator : ICommand
    {
        private readonly Action<object> _action;

        public event EventHandler? CanExecuteChanged;

        public CommandCreator(Action<object> action)
        {
            _action = action;
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

    }
}
