namespace Notification.Core.Services
{
	public interface ITemplateLocator
	{
		string GetTemplate(string code, string locale);
	}
}