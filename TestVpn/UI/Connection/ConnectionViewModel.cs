using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using TestVpn.Common;
using TestVpn.Network;
using TestVpn.Providers;
using TestVpn.Services;

namespace TestVpn.UI.Connection
{
	public class ConnectionViewModel : INotifyPropertyChanged
	{
		public ConnectionViewModel(IVpnService vpnService, IUIThreadInvoker uiThreadInvoker)
		{
			_VpnService = vpnService;
			_UiThreadInvoker = uiThreadInvoker;

			ConnectDisconnectCommand = new Command(ConnectDisconnect, CanConnectDisconnect);
		}

		#region Infrastructure

		private readonly IVpnService _VpnService;

		private readonly IUIThreadInvoker _UiThreadInvoker;

		#endregion

		#region ConnectDisconnect data

		private string _Login;

		public string Login
		{
			get => _Login;
			set
			{
				_Login = value;
				RaisePropertyChanged();
			}
		}

		private SecureString _Password = new SecureString();

		public SecureString Password
		{
			get => _Password;
			set
			{
				_Password = value;
				RaisePropertyChanged();
			}
		}

		private Provider _Provider;

		public Provider Provider
		{
			get => _Provider;
			set
			{
				_Provider = value;
				RaisePropertyChanged();
			}
		}

		#endregion

		#region Connecting

		public bool IsConnected => _VpnService.IsConnected;

		private bool _IsInProgress;

		public bool IsInProgress
		{
			get => _IsInProgress;
			set
			{
				_IsInProgress = value;
				RaisePropertyChanged();
			}
		}

		public ICommand ConnectDisconnectCommand { get; }


		private bool CanConnectDisconnect()
		{
			return !IsInProgress;
		}

		private void ConnectDisconnect()
		{
			IsInProgress = true;
			Task.Run(
				() =>
				{
					(_VpnService.IsConnected ? _VpnService.Disconnect() : _VpnService.Connect(Provider?.Url, Login, Password))
						.ContinueWith(
							task =>
							{
								_UiThreadInvoker.Invoke(
									() =>
									{
										RaisePropertyChanged(nameof(IsConnected));
										IsInProgress = false;
										((Command) ConnectDisconnectCommand).RaiseCanExecuteChanged();
									});
							});
				});
		}

		#endregion

		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}