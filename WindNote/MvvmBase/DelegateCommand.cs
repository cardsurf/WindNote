using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WindNote.Events.Commands
{
    public class DelegateCommand : ICommand
    {
        private readonly Predicate<object> ExecuteCondition;
        private readonly Action<object> ExecuteAction;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<object> execute) : this(execute, null)
        { }

        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.ExecuteAction = execute;
            this.ExecuteCondition = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.ExecuteCondition == null ? true : this.ExecuteCondition(parameter);
        }

        public void Execute(object parameter)
        {
            this.ExecuteAction(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
 
}
