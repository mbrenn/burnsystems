using System;
using System.Collections.Generic;
using System.Linq;

namespace BurnSystems.ObjectActivation
{
	/// <summary>
	/// Description of EnablerChecker.
	/// </summary>
	internal class CriteriaCatalogue
	{
		/// <summary>
		/// Stores the list of included criteria
		/// </summary>
		private List<ICriteria> criterias = new List<ICriteria>();
		
		/// <summary>
		/// Adds a criteria to this object
		/// </summary>
		/// <param name="criteria">Criteria to be added</param>
		public void Add(ICriteria criteria)
		{
			this.criterias.Add(criteria);
		}
		
		/// <summary>
		/// Checks if the list of enablers matches
		/// to all included criteria
		/// </summary>
		/// <param name="enablers">List of enablers</param>
		/// <returns>true, if all objects match as expected</returns>
		public bool DoesMatch(IEnumerable<IEnabler> enablers)
		{
			// All criteria have to match to at least one enabler, 
			// Otherwise we have a problem.
			return this.criterias.All( 
				x => enablers.Any(y => x.DoesMatch(y)));
		}
	}
}
