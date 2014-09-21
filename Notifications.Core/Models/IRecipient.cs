using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Notification.Core.Models
{
	public interface IRecipient
	{
		string Address { get; }

		string Locale { get; }

		ICollection<IRecipient> ReplyTo { get; }
	}

	public class Recipient : IRecipient
	{
		public string Address { get; set; }

		public string Locale { get; set; }

		public ICollection<IRecipient> ReplyTo { get; private set; }

		public Recipient(string address, string locale)
		{
			Address = address;
			Locale = locale;
			ReplyTo = new Collection<IRecipient>();
		}
	}
}