using System;
using System.IO;
using System.Reflection;
using Notification.Core.Models;

namespace Notification.Core.Services
{
	public class LicalizedTemplateLocator : ITemplateLocator
	{
		private readonly TemplateLocatorSettings _templateLocatorSettings;

		public LicalizedTemplateLocator(TemplateLocatorSettings templateLocatorSettings)
		{
			if (templateLocatorSettings == null)
				throw new ArgumentNullException("templateLocatorSettings");

			_templateLocatorSettings = templateLocatorSettings;
		}

		public string GetTemplate(string code, string locale)
		{
			var customTemplateExtensions = _templateLocatorSettings.CustomTemplateExtensions.Split(',');

			using (var resourceStream = GetResourceDataFromFile(code, locale, customTemplateExtensions) ?? GetResourceData(code, customTemplateExtensions))
			{
				if (resourceStream == null)
					throw new ArgumentException("Missing resource for specified message code", "code");

				return new StreamReader(resourceStream).ReadToEnd();
			}
		}

		private static Stream GetResourceData(string resourceName, params string[] possibleExtensions)
		{
			foreach (var possibleExtension in possibleExtensions)
			{
				var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName + possibleExtension);
				if (resourceStream != null) return resourceStream;
			}

			return null;
		}

		private Stream GetResourceDataFromFile(string templateName, string locale, params string[] possibleExtensions)
		{
			var customTemplatePath = _templateLocatorSettings.CustomTemplatesPath;
			
			if (!string.IsNullOrEmpty(customTemplatePath))
			{
				if (!Path.IsPathRooted(customTemplatePath)) customTemplatePath = Path.Combine(Environment.CurrentDirectory, customTemplatePath);
				if (!Directory.Exists(customTemplatePath)) customTemplatePath = null;
			}

			if (customTemplatePath != null)
			{
				foreach (var possibleExtension in possibleExtensions)
				{
					var fileName = string.Format("{0}.{1}.{2}", templateName, locale, possibleExtension);

					var fullPath = Path.Combine(customTemplatePath, fileName);

					if (File.Exists(fullPath)) return File.OpenRead(fullPath);
				}

				foreach (var possibleExtension in possibleExtensions)
				{
					var fileName = string.Format("{0}.{1}", templateName, possibleExtension);

					var fullPath = Path.Combine(customTemplatePath, fileName);

					if (File.Exists(fullPath)) return File.OpenRead(fullPath);
				}
			}

			return null;
		}
	}
}