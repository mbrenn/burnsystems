using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.Scope
{
    /// <summary>
    /// Creates a subinstance of a context. 
    /// Changes, like adding new context sources are only performend on the subinstance. 
    /// During disposal, only the objects created by subinstance are deletec
    /// </summary>
    public class NestedContext : IContext
    {
        /// <summary>
        /// Stores the subcontext
        /// </summary>
        private IContext subContext = new Context();

        /// <summary>
        /// Stores the parentcontext
        /// </summary>
        private IContext parentContext;

        /// <summary>
        /// Initializes a new instance of the NestedContext class.
        /// </summary>
        /// <param name="parentContext">Context to be set</param>
        public NestedContext(IContext parentContext)
        {
            this.parentContext = parentContext;
        }

        /// <summary>
        /// Gets the local context source
        /// </summary>
        public IContextSource LocalContextSource
        {
            get { return subContext.LocalContextSource; }
        }

        /// <summary>
        /// Adds an context source
        /// </summary>
        /// <param name="source">Source to be added</param>
        public void Add(IContextSource source)
        {
            this.subContext.Add(source);
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
            var result = this.subContext.Get<T>();
            if (result == null)
            {
                this.parentContext.Get<T>();
            }

            return result;
        }

        /// <summary> 
        /// Gets an object by token. This object is created if necessary.
        /// </summary>
        /// <typeparam name="T">Type of the parameter</typeparam>
        /// <param name="token">Token to be added</param>
        /// <returns>Object to be created or retrieved</returns>
        public T Get<T>(string token)
        {
            var result = this.subContext.Get<T>(token);
            if (result == null)
            {
                this.parentContext.Get<T>(token);
            }

            return result;
        }

        /// <summary>
        /// Disposes the object
        /// </summary>
        public void Dispose()
        {
            this.subContext.Dispose();
        }

        /// <summary>
        /// Gets all object matching to a specific type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Enumeration of all items matching to the type</returns>
        public IEnumerable<T> GetAll<T>()
        {
            return this.subContext.GetAll<T>()
                .Union(this.parentContext.GetAll<T>());
        }
    }
}
