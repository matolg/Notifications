using System;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using log4net;
using Notification.Core.Helpers;
using Notification.Core.Models;

namespace Notification.Core.Services
{
	public class EmailDeliveryMethod : IDeliveryMethod
	{
		private ISmtpClient SmtpClient { get; set; }

		private MailSettings MailSettings { get; set; }

		private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public EmailDeliveryMethod(MailSettings mailSettings)
			: this(new DefaultSmtpClient(mailSettings.SmtpOptions), mailSettings)
		{
		}

		public EmailDeliveryMethod(ISmtpClient smtpClient, MailSettings mailSettings)
		{
			if (smtpClient == null)
				throw new ArgumentNullException("smtpClient");
			
			SmtpClient = smtpClient;
			MailSettings = mailSettings;
		}

		public void HandleDelivery(DeliveryInfo deliveryInfo)
		{
			try
			{
				using (var message = new MailMessage())
				{
					CreateMailMessage(deliveryInfo, message);

					SmtpClient.Send(message);

					if (_log.IsInfoEnabled)
						_log.InfoFormat("Mail sent to {0}", deliveryInfo.To);
				}
			}
			catch (SmtpFailedRecipientException exc)
			{
				_log.Error("Email sending was failed. Wrong email address " + exc.FailedRecipient +
									". Recipient is missed. " + exc.Message);

				throw;
			}
			catch (Exception ex)
			{
				_log.Error("Error while sending mail", ex);

				throw;
			}
		}

		private void CreateMailMessage(DeliveryInfo deliveryInfo, MailMessage message)
		{
			var recipients = deliveryInfo.To;
			if (string.IsNullOrEmpty(recipients)) throw new ArgumentNullException(recipients);

			var addressCollection = new MailAddressCollection();

			if (!MailSettings.UseEmailTestMode) addressCollection.Add(recipients);
			else addressCollection.Add(MailSettings.TestEmailAddress);

			foreach (var address in addressCollection)
			{
				message.To.Add(EmailHelper.FixMailAddressDisplayName(address, MailSettings.EmailHeaderEncoding));
			}

			message.From = EmailHelper.FixMailAddressDisplayName(new MailAddress(deliveryInfo.From), MailSettings.EmailHeaderEncoding);

			message.BodyEncoding = Encoding.GetEncoding(MailSettings.EmailBodyEncoding);
			message.SubjectEncoding = Encoding.GetEncoding(MailSettings.EmailSubjectEncoding);
			message.HeadersEncoding = Encoding.GetEncoding(MailSettings.EmailHeaderEncoding);

			message.Body = deliveryInfo.Body;
			message.IsBodyHtml = deliveryInfo.IsBodyHtml;
			message.Subject = deliveryInfo.Subject;

			if (!string.IsNullOrEmpty(deliveryInfo.ReplyTo))
			{
				var replyToCollection = new MailAddressCollection { deliveryInfo.ReplyTo };

				foreach (var address in replyToCollection)
				{
					message.ReplyToList.Add(EmailHelper.FixMailAddressDisplayName(address, MailSettings.EmailHeaderEncoding));
				}
			}
		}
	}
}