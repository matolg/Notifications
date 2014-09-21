using System;
using System.Net.Mail;

namespace Notification.Core.Services
{
	public interface ISmtpClient : IDisposable
	{
		void Send(MailMessage message);
	}
}