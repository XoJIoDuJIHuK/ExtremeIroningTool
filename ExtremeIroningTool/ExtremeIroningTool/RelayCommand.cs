using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExtremeIroningTool
{
    public class RelayCommand : ICommand
    {
        private Action pAction;

        public RelayCommand(Action action) { pAction = action; }

        public event EventHandler? CanExecuteChanged = (sender, e) => { };
        public bool canExecute = true;
        public bool CanExecute(object? parameter)
        {
            return canExecute;
        }

        public void Execute(object? parameter)
        {
            pAction();
        }
    }
}
