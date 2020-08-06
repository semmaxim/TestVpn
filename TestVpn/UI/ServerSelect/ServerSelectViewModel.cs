using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TestVpn.Common;
using TestVpn.Providers;

namespace TestVpn.UI.ServerSelect
{
	public class ServerSelectViewModel : INotifyPropertyChanged
	{
		public ServerSelectViewModel(IProvidersService providersService)
		{
			SelectProviderCommand = new Command<Provider>(SelectProvider);

			Providers = providersService.GetProviders();
		}

		public ICommand SelectProviderCommand { get; }

		private void SelectProvider(Provider provider)
		{
			RaiseProviderSelected(provider);
		}

		public event EventHandler<Provider> ProviderSelected;

		protected virtual void RaiseProviderSelected(Provider e)
		{
			ProviderSelected?.Invoke(this, e);
		}

		public Provider[] Providers { get; }

		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}