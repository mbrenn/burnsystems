using System;
using System.Collections.Generic;
using BurnSystems.ObjectActivation.Enabler;

namespace BurnSystems.ObjectActivation
{
	/// <summary>
	/// Helper class containing several extension methods
	/// for the activation container.
	/// </summary>
	public static class ActivationContainerExtensions
	{
		/// <summary>
		/// Gets the object by enablers but in a 
		/// </summary>
		/// <param name="activates"></param>
		/// <param name="enablers"></param>
		/// <returns></returns>
		public static T Get<T>(this IActivates activates, IEnumerable<IEnabler> enablers)
		{
			var result = activates.Get(enablers);
			if(result == null)
			{
				return default(T);
			}
			
			return (T) result;
		}
		
		/// <summary>
		/// Gets the object by type
		/// </summary>
		/// <param name="activates">Activation container of the object</param>
		/// <returns>Found type of null</returns>
		public static T Get<T>(this IActivates activates)
		{
			return activates.Get<T>(
				new IEnabler[] { new ByTypeEnabler(typeof(T)) });
		}		
	}
}
