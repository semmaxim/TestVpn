namespace TestVpn.Services
{
	public interface IViewModelFactoryService
	{
		TViewModel CreateViewModel<TViewModel>();
	}
}