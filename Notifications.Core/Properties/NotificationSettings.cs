using System.ComponentModel;

namespace Notification.Core.Properties
{
	public class NotificationSettings
	{
		// Директория с шаблонами сообщений
		public string CustomTemplatesPath { get; set; }

		// Допустимые расширения шаблонов сообщений
		public string CustomTemplateExtensions { get; set; }

		// Использовать тестовую email-отправку
		public bool UseEmailTestMode { get; set; }

		// Адрес для тестовой отправки
		public string TestEmailAddress { get; set; }
		
		// Кодировка email письма
		public string EmailBodyEncoding { get; set; }

		// Кодировка темы email письма
		public string EmailSubjectEncoding { get; set; }

		// Кодировка заголовков email письма
		public string EmailHeaderEncoding { get; set; }

		// Почтовый сервер
		public string EmailHost { get; set; }

		[DefaultValue(25)]
		// Порт почтового сервера
		public int ServerPort { get; set; }

		// Имя пользователя почтового сервера
		public string SmtpUserName { get; set; }

		// Пароль пользователя почтового сервера
		public string SmtpPassword { get; set; }
	}
}