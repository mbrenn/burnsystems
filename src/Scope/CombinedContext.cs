using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.Test;

namespace BurnSystems.Scope
{
    /// <summary>
    /// Combines a context with an existing context
    /// </summary>
    public class CombinedContext : IContext
    {
        /// <summary>
        /// Primary context
        /// </summary>
        private IContext primaryContext;

        /// <summary>
        /// Secondary source
        /// </summary>
        private IContext secondaryContext;

        /// <summary>
        /// Initializes a new instance of the CombinedContext class.
        /// </summary>
        /// <param name="first">First context</param>
        /// <param name="second">Secondary context. 
        /// This context will be modified, if an additional context source has been added</param>
        public CombinedContext(IContext first, IContext second)
        {
            Ensure.IsNotNull(first);
            Ensure.IsNotNull(second);
            this.primaryContext = first;
            this.secondaryContext = second;
        }

        /// <summary>
        /// Adds an item to context
        /// </summary>
        /// <typeparam name="T">Type of the item to be added</typeparam>
        /// <param name="item">Item to be added</param>
        public void Add(IContextSource source)
        {
            this.secondaryContext.Add(source);
        }

        /// <summary>
        /// Gets an object, it is created if necessary.
        /// This method only works, if there is just one 
        /// instance within the context source
        /// </summary>
        /// <typeparam name="T">Type of the parameter</typeparam>
        /// <returns>Object to be created or retrieved</returns>
        public T Get<T>()
        {
            var found = this.primaryContext.Get<T>();

            if (found == null)
            {
                found = this.secondaryContext.Get<T>();
            }

            return found;
        }

        /// <summary> 
        /// Gets an object by token. This object is created if necessary.
        /// </summary>
        /// <typeparam name="T">Type of the parameter</typeparam>
        /// <param name="token">Token to be added</param>
        /// <returns>Object to be created or retrieved</returns>
        public T Get<T>(string token)
        {
            var found = this.primaryContext.Get<T>(token);

            if (found == null)
            {
                found = this.secondaryContext.Get<T>(token);
            }

            return found;
        }

        /// <summary>
        /// Disposes all items implementing the IDisposable
        /// This method shall not be called, because this class shall be used as a helper methods. 
        /// The methods creating the original context-variables shall dispose the context
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException("DO NOT CALL ME");
        }
    }
}
