using System;
using System.Windows.Input;

namespace MongoDbGuiTask1
{
    public class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private readonly Action<object> methodToExecute;
        private readonly Func<bool>? canExecuteEvaulator;

        public RelayCommand(Action<object> methodToExecute, Func<bool>? canExecuteEvaulator)
        {
            this.methodToExecute = methodToExecute;
            this.canExecuteEvaulator = canExecuteEvaulator;
        }

        public RelayCommand(Action<object> methodToExecute) : this(methodToExecute, null)
        {
        }


        public bool CanExecute(object? parameter)
        {
            if (canExecuteEvaulator is null)
            {
                return true;
            }
            else
            {
                return canExecuteEvaulator.Invoke();
            }
        }

        public void Execute(object? parameter)
        {
            methodToExecute.Invoke(parameter ?? new());
        }
    }
}
