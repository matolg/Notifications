using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using Notification.Core.Models;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace Notification.Core.Services
{
	public interface IMessageTemplate
	{
		string Subject { get; }
	}

	public class MessageTemplate<T> : TemplateBase<T>, IMessageTemplate
	{
		public string Subject { get; protected set; }
	}

	// Wrapper around RazorEngine template generator
	public class RazorMessageGenerator : IMessageGenerator
	{
		private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected TemplateService _service;

		protected ITemplateLocator _templateLocator;

		public RazorMessageGenerator(TemplateLocatorSettings templateLocatorSettings) 
			: this(new LicalizedTemplateLocator(templateLocatorSettings)) { }

		public RazorMessageGenerator(ITemplateLocator templateLocator)
		{
			if (templateLocator == null)
				throw new ArgumentNullException("templateLocator");

			_templateLocator = templateLocator;

			var serviceConfiguration = new TemplateServiceConfiguration
											{
												BaseTemplateType = typeof(MessageTemplate<>),
												Namespaces = new HashSet<string> { typeof(MessageTemplate<>).Namespace }
											};

			_service = new TemplateService(serviceConfiguration);
		}

		public Message Generate(string messageCode, object model, string locale)
		{
			if (model == null)
				throw new NullReferenceException("model");

			try
			{
				var cacheName = string.Format("{0}.{1}", messageCode, locale);

				if (!_service.HasTemplate(cacheName))
				{
					var template = _templateLocator.GetTemplate(messageCode, locale);

					_service.Compile(template, typeof(MessageTemplate<>), cacheName);
				}

				var resolvedTemplate = _service.Resolve(cacheName, model);
				var result = _service.Run(resolvedTemplate, null);

				return new Message
				{
					Subject = ((IMessageTemplate)resolvedTemplate).Subject,
					Body = result
				};
			}
			catch (TemplateCompilationException tce)
			{
				_log.Error("Template compilation error", tce);
				throw;
			}
		}
	}
}