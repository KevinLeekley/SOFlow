// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System.Collections.Generic;
using UnityEngine;

namespace SOFlow.Utilities
{
	/// <summary>
	/// This class serves as a simple interface for retrieving a cached WaitForSeconds instance
	/// for coroutines.
	/// </summary>
	public static class WaitCache
	{
		/// <summary>
		/// The wait cache set.
		/// </summary>
		private static Dictionary<float, WaitForSeconds> _waitCache = new Dictionary<float, WaitForSeconds>();

		/// <summary>
		/// Get a cached WaitForSeconds instance with the specified seconds.
		/// </summary>
		/// <param name="seconds"></param>
		/// <returns></returns>
		public static WaitForSeconds Get(float seconds)
		{
			WaitForSeconds result;

			if(!_waitCache.TryGetValue(seconds, out result))
			{
				result = new WaitForSeconds(seconds);
				_waitCache.Add(seconds, result);
			}

			return result;
		}
	}
}