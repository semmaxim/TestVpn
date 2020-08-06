using System;
using System.Windows;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using NLog.Extensions.Logging;
using TestVpn.Network;
using TestVpn.Providers;
using TestVpn.Services;
using TestVpn.UI.Shell;

namespace TestVpn
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var configuration = new ConfigurationBuilder().Build();
			var serviceProvider = BuildDi(configuration);

			ShutdownMode = ShutdownMode.OnLastWindowClose;

			var shell = new ShellView();
			var viewModelFactory = serviceProvider.GetService<IViewModelFactoryService>();
			shell.DataContext = viewModelFactory.CreateViewModel<ShellViewModel>();
			shell.Show();
		}

		private IServiceProvider BuildDi(IConfiguration config)
		{
			return new ServiceCollection()
				.AddLogging(loggingBuilder =>
				{
					loggingBuilder.ClearProviders();
					loggingBuilder.SetMinimumLevel(LogLevel.Trace);
					loggingBuilder.AddNLog(config);
				})
				.AddSingleton<IUIThreadInvoker, UiThreadInvoker>()
				.AddSingleton<IViewModelFactoryService, ViewModelFactoryService>()
				.AddSingleton<IVpnService, VpnService>()
				.AddSingleton<IProvidersService, ProvidersService>()
				.BuildServiceProvider(); ;
		}
	}
}
