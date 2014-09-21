using Notification.Core.Models;

namespace Notification.Core.Services
{
	public interface INotificationManager
	{
		IMessageGeneratorProvider MessageGeneratorProvider { get; }

		IDeliveryMethodProvider DeliveryMethodProvider { get; }

		void Send(SendOptions options, string deliveryMethod);
	}
}