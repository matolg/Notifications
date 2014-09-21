namespace Notification.Core.Models
{
	public class MailSettings
	{
		public bool UseEmailTestMode { get; set; }

		public string TestEmailAddress { get; set; }

		public string EmailBodyEncoding { get; set; }

		public string EmailSubjectEncoding { get; set; }

		public string EmailHeaderEncoding { get; set; }

		public SmtpOptions SmtpOptions { get; set; }
	}
}
