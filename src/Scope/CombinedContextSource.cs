using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.Test;

namespace BurnSystems.Scope
{
    /// <summary>
    /// Combines two context sources. 
    /// If an object is not found in the first context source, 
    /// the second context source will be queried
    /// </summary>
    public class CombinedContextSource : IContextSource
    {
        /// <summary>
        /// Stores the first context source
        /// </summary>
        private IContextSource first;

        /// <summary>
        /// Stores the second context source
        /// </summary>
        private IContextSource second;

        /// <summary>
        /// Initializes a new instance of the CombinedContextSource class.
        /// </summary>
        /// <param name="first">First contextsource</param>
        /// <param name="second">Second contextsource</param>
        public CombinedContextSource(IContextSource first, IContextSource second)
        {
            Ensure.IsNotNull(first);
            Ensure.IsNotNull(second);
            this.first = first;
            this.second = second;
        }

        /// <summary>
        /// Creates a context
        /// </summary>
        /// <returns>Created context</returns>
        public IContext CreateContext()
        {
            return new Context(this);
        }

        /// <summary>
        /// Adds a factory method which can retrieved within a context. 
        /// This method is not supported because nothing shall be added
        /// in a combined context source
        /// </summary>
        /// <typeparam name="T">Type of the item to be added</typeparam>
        /// <param name="factory">Factory method</param>
        public void Add<T>(Func<T> factory)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Adds a factory method which can retrieved within a context.
        /// This method is not supported because nothing shall be added
        /// in a combined context source
        /// </summary>
        /// <typeparam name="T">Type of the item to be added</typeparam>
        /// <param name="token">Token of the item</param>
        /// <param name="factory">Factory method</param>
        public void Add<T>(string token, Func<T> factory)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Creates the factory method for a type
        /// </summary>
        /// <typeparam name="T">Type to be queried</typeparam>
        /// <returns>Function creating the item</returns>
        public T Create<T>()
        {
            var result = this.first.Create<T>();

            if (result == null)
            {
                result = this.second.Create<T>();
            }

            return result;
        }

        /// <summary>
        /// Creates the factory method for a type
        /// </summary>
        /// <typeparam name="T">Type to be queried</typeparam>
        /// <param name="token">Token to be queries</param>
        /// <returns>Created item</returns>
        public T Create<T>(string token)
        {
            var result = this.first.Create<T>(token);

            if (result == null)
            {
                result = this.second.Create<T>(token);
            }

            return result;
        }

        /// <summary>
        /// Creates all items matching to the type
        /// </summary>
        /// <typeparam name="T">Type to be queried</typeparam>
        /// <returns>Enumerations of all matching types</returns>
        public IEnumerable<T> CreateAll<T>()
        {
            return this.first.CreateAll<T>()
                .Union(this.second.CreateAll<T>());
        }
    }
}
