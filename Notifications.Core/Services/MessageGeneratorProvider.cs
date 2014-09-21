namespace Notification.Core.Services
{
	public interface IMessageGeneratorProvider : IGenericProvider<IMessageGenerator>
	{
	}

	public class MessageGeneratorProvider : GenericProvider<IMessageGenerator>, IMessageGeneratorProvider
	{
	}
}