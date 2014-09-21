using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using Notification.Core.Models;

namespace Notification.Core.Services
{
	public class NotificationManager : INotificationManager
	{
		private readonly IMessageGeneratorProvider _messageGeneratorProvider;

		private readonly IDeliveryMethodProvider _deliveryMethodProvider;

		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public IMessageGeneratorProvider MessageGeneratorProvider
		{
			get
			{
				return _messageGeneratorProvider;
			}
		}

		public IDeliveryMethodProvider DeliveryMethodProvider
		{
			get
			{
				return _deliveryMethodProvider;
			}
		}

		public NotificationManager()
			: this(new MessageGeneratorProvider(), new DeliveryMethodProvider())
		{
		}

		public NotificationManager(IMessageGeneratorProvider messageGeneratorProvider)
			: this(messageGeneratorProvider, new DeliveryMethodProvider())
		{
		}

		public NotificationManager(IDeliveryMethodProvider deliveryMethodProvider)
			: this(new MessageGeneratorProvider(), deliveryMethodProvider)
		{
		}

		public NotificationManager(IMessageGeneratorProvider messageGeneratorProvider, IDeliveryMethodProvider deliveryMethodProvider)
		{
			if (messageGeneratorProvider == null)
				throw new ArgumentNullException("messageGeneratorProvider");

			if (deliveryMethodProvider == null)
				throw new ArgumentNullException("deliveryMethodProvider");

			_messageGeneratorProvider = messageGeneratorProvider;
			_deliveryMethodProvider = deliveryMethodProvider;
		}

		public void Send(SendOptions options, string deliveryMethod)
		{
			var errorMessages = new List<string>();
			if (!ValidateOptions(options, errorMessages)) 
				throw new Exception("Options validation error: " + string.Join(", ", errorMessages));

			var deliveryInfos = GetDeliveryInfos(options, _messageGeneratorProvider.Get(deliveryMethod));
			
			deliveryInfos.ForEach(_deliveryMethodProvider.Get(deliveryMethod).HandleDelivery);
		}

		private List<DeliveryInfo> GetDeliveryInfos(SendOptions options, IMessageGenerator messageGenerator)
		{
			var deliveryInfos = new List<DeliveryInfo>();

			foreach (var recipient in options.Recipients)
			{
				var message = messageGenerator.Generate(options.MessageCode, options.Model, recipient.Locale);

				deliveryInfos.Add(
					new DeliveryInfo
						{
							From = options.Sender.Address,
							To = recipient.Address,
							ReplyTo = string.Join(",", recipient.ReplyTo),
							Subject = message.Subject,
							Body = message.Body,
							IsBodyHtml = true
						});
			}

			return deliveryInfos;
		}

		private bool ValidateOptions(SendOptions options, List<string> errorMessages)
		{
			if (options == null)
			{
				errorMessages.Add("Send options not defined");
				return false;
			}

			if (string.IsNullOrEmpty(options.MessageCode))
				errorMessages.Add("MessageCode not defined");

			if (options.Model == null)
				errorMessages.Add("Model not defined");

			if (options.Sender == null)
				errorMessages.Add("Sender not defined");

			if (options.Recipients == null || !options.Recipients.Any())
				errorMessages.Add("Recipients not defined or empty");

			return !errorMessages.Any();
		}
	}
}
