using System;
using System.Windows.Input;

namespace WPF_PanelMatrix.ViewModel {
    public class DelegateCommand : ICommand {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;
        public event EventHandler? CanExecuteChanged;

        public DelegateCommand(Action<object?> execute, Predicate<object?>? canExecute = null) {
            if (execute == null) {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public void Execute(object? parameter) {
            if (!CanExecute(parameter)) {
                throw new InvalidOperationException("Command execution is disabled");
            }

            _execute(parameter);
        }

        public void RaiseCanExecuteChanged() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
