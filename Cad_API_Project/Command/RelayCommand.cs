using System;
using System.Windows.Input;
using System.Windows;


namespace Cad_API_Project.Command
{
    public class RelayCommand : ICommand
    {
        #region Variables

        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;
        private readonly Action _act;

        #endregion

        #region Support

        public class CloseCommand : ICommand
        {
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }

            public void Execute(object parameter)
            {
                if (parameter is Window myWin) myWin.Close();
            }
        }

        #endregion

        #region Constructor

        public RelayCommand(Action act)
        {
            _act = act;
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException("Execute");
            _canExecute = canExecute;
        }

        #endregion Constructor

        #region Implementation

        // Evaluate the command if it is valid to execute
        public bool CanExecute(object parameter = null)
        {
            if (parameter == null || _canExecute == null) return true;
            else return _canExecute(parameter);
        }

        // Main execute method
        public void Execute(object parameter = null)
        {
            if (_act != null) _act();
            else _execute(parameter);
        }

        // In WPF CommandManager is a pre-defined class that take charge of observing the user interface
        // and calls the CanExecute method when it deems it necessary
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion Implementation


    }
}
