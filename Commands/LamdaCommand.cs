using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkController.Client.Commands.BaseCommands;

namespace WorkController.Client.Commands
{
    internal class LamdaCommand : BaseCommand
    {
        private readonly Action<object> _Execute;
        private readonly Func<object, bool> _CanExecute;
        public LamdaCommand(Action<object> Execute,Func<object,bool> canExecute=null)
        {
            _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _CanExecute = canExecute;
        }
        public override bool CanExecute(object parameter) =>
            _CanExecute?.Invoke(parameter) ?? true;

        public override void Execute(object parameter) => _Execute(parameter);
    }
}
