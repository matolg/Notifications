using Notification.Core.Models;

namespace Notification.Core.Services
{
	public interface IDeliveryMethod
	{
		void HandleDelivery(DeliveryInfo deliveryInfo);
	}
}