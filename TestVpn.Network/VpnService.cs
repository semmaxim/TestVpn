using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace TestVpn.Network
{
	public class VpnService : IVpnService
	{
		public VpnService(ILoggerProvider loggerProvider)
		{
			_Logger = loggerProvider.CreateLogger(GetType().FullName);

			_Logger.LogTrace("VpnService created.");
		}

		private readonly ILogger _Logger;

		public bool IsConnected { get; private set; }

		public bool IsOperationInProgress { get; private set; }

		private readonly SemaphoreSlim _OperationsSemaphore = new SemaphoreSlim(1, 1);

		public async Task Connect(string server, string user, SecureString password)
		{
			await _OperationsSemaphore.WaitAsync();
			try
			{
				if (!IsConnected)
				{
					IsOperationInProgress = true;
					await ConnectInternal(server, user, password);
				}
			}
			finally
			{
				IsOperationInProgress = false;
				_OperationsSemaphore.Release();
			}
		}

		private async Task ConnectInternal(string server, string user, SecureString password)
		{
			IntPtr unmanagedString = IntPtr.Zero;
			try
			{
				unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(password);
				_Logger.LogDebug($"Connecting. Server = {server}, user = {user}, password = {Marshal.PtrToStringUni(unmanagedString)}");
			}
			finally
			{
				Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
			}

			if (IsConnected)
				await DisconnectInternal();

			await Task.Delay(TimeSpan.FromSeconds(5d));

			_Logger.LogDebug($"Connected.");

			IsConnected = true;
		}

		public async Task Disconnect()
		{
			await _OperationsSemaphore.WaitAsync();
			try
			{
				if (IsConnected)
				{
					IsOperationInProgress = true;
					await DisconnectInternal();
				}
			}
			finally
			{
				IsOperationInProgress = false;
				_OperationsSemaphore.Release();
			}
		}

		private async Task DisconnectInternal()
		{
			_Logger.LogTrace("Disconnecting.");

			await Task.Delay(TimeSpan.FromSeconds(5d));

			_Logger.LogTrace("Disconnected.");

			IsConnected = false;
		}
	}
}
