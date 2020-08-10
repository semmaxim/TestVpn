using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestVpn.Services
{
	public class ViewModelFactoryService : IViewModelFactoryService
	{
		private readonly IServiceProvider _ServiceProvider;

		private readonly ILogger _Logger;

		public ViewModelFactoryService(IServiceProvider serviceProvider, ILoggerProvider loggerProvider)
		{
			_ServiceProvider = serviceProvider;
			_Logger = loggerProvider.CreateLogger(GetType().FullName);

			_Logger.LogTrace("ViewModelFactoryService created.");
		}

		public TViewModel CreateViewModel<TViewModel>()
		{
			_Logger.LogTrace("Creating {0} view model.", typeof(TViewModel).FullName);

			return ActivatorUtilities.CreateInstance<TViewModel>(_ServiceProvider);
		}
	}
}