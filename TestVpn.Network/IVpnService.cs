using System;
using System.Security;
using System.Threading.Tasks;

namespace TestVpn.Network
{
	public interface IVpnService
	{
		bool IsConnected { get; }

		bool IsOperationInProgress { get; }

		Task Connect(string server, string user, SecureString password);

		Task Disconnect();
	}
}