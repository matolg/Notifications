using Funq;
using Notification.Core.Models;
using Notification.Core.Services;

namespace SimpleClient
{
	class Program
	{
		static void Main(string[] args)
		{
		}
	}

	public static class CompositionRoot
	{
		private static Container Container { get; set; }

		public static void Init()
		{
			Container = new Container();

			InitNotification(Container);
		}

		public static void InitNotification(Container container)
		{
			var mailSettings = new MailSettings
			{
				SmtpOptions = new SmtpOptions()
			};

			var templateLocatorSettings = new TemplateLocatorSettings();

			container.Register<INotificationManager>(c => new NotificationManager());

			container.Resolve<INotificationManager>().DeliveryMethodProvider
				.Register("email", new EmailDeliveryMethod(mailSettings));

			container.Resolve<INotificationManager>().MessageGeneratorProvider
				.Register("email", new RazorMessageGenerator(templateLocatorSettings));
		}
	}
}
