using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// This class is capable to hold an IActivates interface and creates the necessary
    /// interfaces. 
    /// </summary>
    public class InstanceBuilder
    {
        /// <summary>
        /// Stores the container or block being used for instantiation
        /// </summary>
        private IActivates container;

        /// <summary>
        /// Stores the cache for instantiation
        /// </summary>
        private Dictionary<Type, InstantiationCacheEntry> cache = new Dictionary<Type, InstantiationCacheEntry>();

        /// <summary>
        /// Initializes a new instance of the instance builder
        /// </summary>
        /// <param name="container"></param>
        public InstanceBuilder(IActivates container)
        {
            this.container = container;

            this.container.BindingChanged += (x, y) => cache.Clear();
        }

        /// <summary>
        /// Creates an instance of the object
        /// </summary>
        /// <typeparam name="T">Type of the object to be requested</typeparam>
        /// <returns>Created instance</returns>
        public T Create<T>()
        {
            InstantiationCacheEntry entry;
            if (this.cache.TryGetValue(typeof(T), out entry))
            {
                return (T)entry.FactoryMethod();
            }
            else
            {
                var result = Expression.Parameter(typeof(T), "result");

                var expression = Expression.Block(
                    new[] { result },
                    Expression.Assign(result, Expression.New(typeof(T))),
                    result);

                var cacheEntry = new InstantiationCacheEntry();

                cacheEntry.FactoryMethod = Expression.Lambda<Func<object>>(expression).Compile();

                this.cache[typeof(T)] = cacheEntry;

                return (T) cacheEntry.FactoryMethod();
            }
        }
    }
}
