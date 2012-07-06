using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// Defines a cache entry for the instantiation of a class.
    /// </summary>
    public class InstantiationCacheEntry
    {
        /// <summary>
        /// Gets or sets the factory method
        /// </summary>
        public Func<object> FactoryMethod
        {
            get;
            set;
        }
    }
}
