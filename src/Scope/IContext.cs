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
        /// Adds an item to context
        /// </summary>
        /// <typeparam name="T">Type of the item to be added</typeparam>
        /// <param name="item">Item to be added</param>
        void Add<T>(T item);

        /// <summary>
        /// Adds an item to context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <param name="item"></param>
        void Add<T>(string token, T item);

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
    }
}
