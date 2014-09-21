namespace Notification.Core.Models
{
	public interface ISender
	{
		string Address { get; set; }
	}

	public class Sender : ISender
	{
		public string Address { get; set; }
	}
}