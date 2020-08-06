using System;

namespace TestVpn.Services
{
	public interface IUIThreadInvoker
	{
		void Invoke(Action action);
	}
}