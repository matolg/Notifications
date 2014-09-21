using System.Net.Mail;
using System.Text;

namespace Notification.Core.Helpers
{
	public static class EmailHelper
	{
		public static string FixMailAddressDisplayName(string address)
		{
			return address == null ? null : address.Replace(' ', '\xA0');
		}

		public static MailAddress FixMailAddressDisplayName(MailAddress address, string emailHeaderEncoding)
		{
			if (address == null || string.IsNullOrEmpty(address.DisplayName))
				return address;

			return new MailAddress(address.Address, FixMailAddressDisplayName(address.DisplayName),
				Encoding.GetEncoding(emailHeaderEncoding));
		}
	}
}