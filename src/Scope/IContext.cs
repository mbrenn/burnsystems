using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.Scope
{
    /// <summary>
    /// The interface of the context defines the methods that 
    /// have to be implemented by a context scope
    /// </summary>
    public interface IContext : IDisposable
    {
        /// <summary>
        /// Gets the local context source which can be used to add additional factory methods
        /// </summary>
        IContextSource LocalContextSource
        {
            get;
        }

        /// <summary>
        /// Adds an additional source to the context. 
        /// </summary>
        /// <param name='source'>
        /// Source to be added to the context
        /// </param>
        void Add(IContextSource source);

        /// <summary>
        /// Gets an object, it is created if necessary.
        /// This method only works, if there is just one 
        /// instance within the context source
        /// </summary>
        /// <typeparam name="T">Type of the parameter</typeparam>
        /// <returns>Object to be created or retrieved</returns>
        T Get<T>();

        /// <summary> 
        /// Gets an object by token. This object is created if necessary.
        /// </summary>
        /// <typeparam name="T">Type of the parameter</typeparam>
        /// <param name="token">Token to be added</param>
        /// <returns>Object to be created or retrieved</returns>
        T Get<T>(string token);

        /// <summary>
        /// Gets all object matching to a specific type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Enumeration of all items matching to the type</returns>
        IEnumerable<T> GetAll<T>();
    }
}
