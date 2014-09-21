using System;
using System.Net;
using System.Net.Mail;
using Notification.Core.Models;

namespace Notification.Core.Services
{
	public class DefaultSmtpClient : ISmtpClient
	{
		private readonly SmtpClient _smtpClient;

		public DefaultSmtpClient(SmtpOptions smtpOptions)
		{
			if (string.IsNullOrEmpty(smtpOptions.Host))
				throw new ArgumentNullException(_smtpClient.Host);

			_smtpClient = new SmtpClient(smtpOptions.Host, smtpOptions.Port);

			if (!string.IsNullOrEmpty(smtpOptions.UserName) && !string.IsNullOrEmpty(smtpOptions.Password))
				_smtpClient.Credentials = new NetworkCredential(smtpOptions.UserName, smtpOptions.Password);
		}

		public void Send(MailMessage message)
		{
			_smtpClient.Send(message);
		}

		public void Dispose()
		{
			_smtpClient.Dispose();
		}
	}
}