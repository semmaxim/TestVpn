using System;
using System.Windows.Input;

namespace TestVpn.Common
{
	public class Command : ICommand
	{
		private readonly Action _Execute;

		private readonly Func<bool> _CanExecute;

		public Command(Action execute, Func<bool> canExecute = null)
		{
			_Execute = execute;
			_CanExecute = canExecute;
		}

		/// <inheritdoc />
		public bool CanExecute(object parameter)
		{
			return _CanExecute?.Invoke() ?? true;
		}

		/// <inheritdoc />
		public void Execute(object parameter)
		{
			if (!CanExecute(parameter))
				return;

			_Execute();
		}

		/// <inheritdoc />
		public event EventHandler CanExecuteChanged;

		public void RaiseCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}

	public class Command<TParameter> : ICommand
	{
		private readonly Action<TParameter> _Execute;

		private readonly Func<TParameter, bool> _CanExecute;

		public Command(Action<TParameter> execute, Func<TParameter, bool> canExecute = null)
		{
			_Execute = execute;
			_CanExecute = canExecute;
		}

		/// <inheritdoc />
		public bool CanExecute(object parameter)
		{
			return _CanExecute?.Invoke((TParameter) parameter) ?? true;
		}

		/// <inheritdoc />
		public void Execute(object parameter)
		{
			if (!CanExecute(parameter))
				return;

			_Execute((TParameter) parameter);
		}

		/// <inheritdoc />
		public event EventHandler CanExecuteChanged;

		public void RaiseCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}