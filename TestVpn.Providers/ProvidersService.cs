using System;
using System.Linq;
using TestVpn.Providers.Resources;

namespace TestVpn.Providers
{
	public class ProvidersService : IProvidersService
	{
		public ProvidersService()
		{
			_Providers = new Lazy<Provider[]>(LoadProviders, true);
		}

		private readonly Lazy<Provider[]> _Providers;

		private Provider[] LoadProviders()
		{
			return new[]
			       {
				       new Provider ("Australia", "au.testprovider.net", FlagImages.Australia),
				       new Provider ("France", "fr.testprovider.net", FlagImages.France),
				       new Provider ("Germany", "ge.testprovider.net", FlagImages.Germany),
				       new Provider ("Mexico", "mx.testprovider.net", FlagImages.Mexico),
				       new Provider ("Russia", "ru.testprovider.net", FlagImages.Russia),
				       new Provider ("United States", "us.testprovider.net", FlagImages.US)
			       };
		}

		/// <inheritdoc />
		public Provider[] GetProviders()
		{
			return _Providers.Value.ToArray();
		}
	}
}