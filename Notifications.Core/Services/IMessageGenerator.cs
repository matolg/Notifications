using Notification.Core.Models;

namespace Notification.Core.Services
{
	public interface IMessageGenerator
	{
		Message Generate(string messageCode, object model, string locale);
	}
}