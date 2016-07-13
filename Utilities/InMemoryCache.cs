using System;
using System.Collections.Generic;

namespace Structura.Shared.Utilities
{
	/// <summary>
	/// Dictionary with clear error messages for retrieving non-existant values or requesting mismatched types
	/// </summary>
	public class InMemoryCache
	{
		private readonly Dictionary<string, dynamic> _dictionary = new Dictionary<string, dynamic>();

		/// <summary>
		/// Adds new values or updates an existing value
		/// </summary>
		public void SetOrAdd(string key, dynamic value)
		{
			_dictionary[key] = value;
		}

		public bool ContainsKey(string key)
		{
			return _dictionary.ContainsKey(key);
		}

		/// <summary>
		/// Retrieve existing value. Throws exception if it doesn't exist or the type of the existing value differs
		/// </summary>
		public T Get<T>(string key)
		{
			if (!ContainsKey(key))
			{
				
				throw new ArgumentException(
					$"The value of key {key} with type {typeof(T).Name} is requested " +
					$"to {nameof(InMemoryCache)} but this key is unknown.",
					nameof(key));
			}

			var val = _dictionary[key];
			if (!(val != null || !typeof(T).IsPrimitive))
			{
				throw new ArgumentException(
					$"The value of key {key} is null in {nameof(InMemoryCache)} " +
					$"and this cannot be casted to primitive type {nameof(T)}", 
					nameof(key));
			}

			if (!(val == null || typeof(T).IsAssignableFrom(val.GetType())))
			{
				throw new ArgumentException(
					$"The value of key {key} is requested to {nameof(InMemoryCache)} but the type " +
					$"of the value {val?.GetType()} differs from the requested type {nameof(T)}.");
			}

			if (typeof(T) == typeof(object))
			{
				return (T)(object)val; // causes a null reference exception if not casted like this
			}
			else
			{
				return (T)val; // necessary for derived types
			}
		}
	}
}
