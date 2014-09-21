namespace Notification.Core.Services
{
	public interface IDeliveryMethodProvider : IGenericProvider<IDeliveryMethod>
	{
	}

	public class DeliveryMethodProvider : GenericProvider<IDeliveryMethod>, IDeliveryMethodProvider
	{
	}
}