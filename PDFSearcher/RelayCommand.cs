using System;
using System.Windows.Input;

namespace PDFSearcher
{
    public class RelayCommand : ICommand
    {
        private Action MethodToExecute;
        public RelayCommand(Action methodToExecute)
        {
            MethodToExecute = methodToExecute;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            MethodToExecute.Invoke();
        }
    }
}
