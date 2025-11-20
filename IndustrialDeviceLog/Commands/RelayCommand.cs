using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IndustrialDeviceLog.Commands
{
    /// <summary>
    /// 命令封装轻量类
    /// </summary>
    internal class RelayCommand<T>: ICommand
    {
        private readonly Action<T> _execute; // 命令执行的逻辑             
        private readonly Func<bool>? _canExecute; // 命令是否可执行

        public RelayCommand(Action<T> execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            //canExecute 不传时默认为 null（按钮一直可用）
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute();
        }

        // 命令执行（按钮点击时触发）
        public void Execute(object? parameter)
        {
            if (parameter is T typedParam)
            {
                _execute(typedParam);
            }
            else if (parameter == null) 
            {
                _execute(default(T)); 
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
