using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TestVpn.Common;
using TestVpn.Providers;
using TestVpn.Services;
using TestVpn.UI.Connection;
using TestVpn.UI.ServerSelect;

namespace TestVpn.UI.Shell
{
	public class ShellViewModel : INotifyPropertyChanged
	{
		public ShellViewModel(IViewModelFactoryService viewModelFactoryService)
		{
			_ViewModelFactoryService = viewModelFactoryService;

			ServerSelectCommand = new Command(ServerSelect);

			_ConnectionViewModel = _ViewModelFactoryService.CreateViewModel<ConnectionViewModel>();
			CurrentViewModel = _ConnectionViewModel;
		}

		#region Infrastructure

		private readonly IViewModelFactoryService _ViewModelFactoryService;

		#endregion

		private readonly ConnectionViewModel _ConnectionViewModel;

		private object _CurrentViewModel;

		public object CurrentViewModel
		{
			get => _CurrentViewModel;
			set
			{
				_CurrentViewModel = value;
				RaisePropertyChanged();
			}
		}

		public ICommand ServerSelectCommand { get; }

		private void ServerSelect()
		{
			var serverSelectViewModel = _ViewModelFactoryService.CreateViewModel<ServerSelectViewModel>();
			serverSelectViewModel.ProviderSelected += ServerSelectViewModel_ProviderSelected;
			CurrentViewModel = serverSelectViewModel;
		}

		private void ServerSelectViewModel_ProviderSelected(object sender, Provider provider)
		{
			_ConnectionViewModel.Provider = provider;
			((ServerSelectViewModel) sender).ProviderSelected -= ServerSelectViewModel_ProviderSelected;
			CurrentViewModel = _ConnectionViewModel;
		}

		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}