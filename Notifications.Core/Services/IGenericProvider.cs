using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Notification.Core.Services
{
	// ToDo: move into more generic library
	public interface IGenericProvider<TService>
	{
		void Register<T>(string name) where T : TService;

		void Register(string name, TService instance);

		TService Get(string name, bool throwIfNotFound = true);
	}

	public class GenericProvider<TService> : IGenericProvider<TService>
	{
		private readonly ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();

		public void Register<T>(string deliveryType) where T : TService
		{
			var instance = Activator.CreateInstance<T>();

			this._cache.AddOrUpdate(deliveryType, instance, (s, o) => instance);
		}

		public void Register(string name, TService instance)
		{
			this._cache.AddOrUpdate(name, instance, (s, o) => instance);
		}

		public TService Get(string name, bool throwIfNotFound = true)
		{
			object o;

			if (!this._cache.TryGetValue(name, out o) && throwIfNotFound)
			{
				throw new KeyNotFoundException(string.Format("Type '{0}' is not registred", name));
			}

			return (TService)o;
		}
	}
}