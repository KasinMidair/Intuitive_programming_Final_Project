using SlideFun.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SlideFun.Core.Helper
{
    public class RelayCommand<T> : ICommand
    {
        protected Action<T> _execute;
        protected Func<T, bool> _canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        public RelayCommand(Action<T> Excecute,Func<T,bool>CanExecute = null)
        {
            if (Excecute == null)
                throw new ArgumentException("execute");
            _execute += Excecute;
            _canExecute = CanExecute;   
        }

        public  bool CanExecute(object? parameter)
        {
            try
            {
                return _canExecute == null || _canExecute((T)parameter);
            }
            catch
            {
                return true;
            }
        }


        public virtual void Execute(object? parameter)
        {
            MusicSystemService.Instance.PlayBTN_ClickSound();
            _execute((T)parameter);
        }
    }
}
