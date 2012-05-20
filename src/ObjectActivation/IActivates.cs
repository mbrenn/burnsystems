using System;
using System.Collections.Generic;

namespace BurnSystems.ObjectActivation
{
	/// <summary>
	/// Defines that the implementing class is capable to activate objects
	/// </summary>
	public interface IActivates
	{
		/// <summary>
		/// Gets an object by enablers
		/// </summary>
		/// <param name="enablers">Enumeration of relevant enablers</param>
		/// <returns>The enabled object or null, if no object can be enabled</returns>
		object Get(IEnumerable<IEnabler> enablers);
	}
}
