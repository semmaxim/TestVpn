using System;
using System.Windows;

namespace TestVpn.Services
{
	public class UiThreadInvoker : IUIThreadInvoker
	{
		/// <inheritdoc />
		public void Invoke(Action action)
		{
			Application.Current.Dispatcher.Invoke(action);
		}
	}
}