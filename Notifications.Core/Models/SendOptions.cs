using System.Collections.Generic;

namespace Notification.Core.Models
{
	public class SendOptions
	{
		public string MessageCode { get; set; }

		public object Model { get; set; }

		public ISender Sender { get; set; }

		public IEnumerable<IRecipient> Recipients { get; set; }
	}
}