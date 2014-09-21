namespace Notification.Core.Models
{
	public class DeliveryInfo
	{
		public string Subject { get; set; }

		public string Body { get; set; }
		
		public bool IsBodyHtml { get; set; }

		public string From { get; set; }

		public string To { get; set; }

		public string ReplyTo { get; set; }
	}
}