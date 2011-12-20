using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.Scope
{
    /// <summary>
    /// Defines the interface of the factory of a context. 
    /// A context is a collection of object instances which can be retrieved if 
    /// necessary. 
    /// </summary>
    public interface IContextSource
    {
        /// <summary>
        /// Adds a factory method which can retrieved within a context
        /// </summary>
        /// <typeparam name="T">Type of the item to be added</typeparam>
        /// <param name="factory">Factory method</param>
        void Add<T>(Func<T> factory);

        /// <summary>
        /// Adds a factory method which can retrieved within a context
        /// </summary>
        /// <typeparam name="T">Type of the item to be added</typeparam>
        /// <param name="token">Token of the item</param>
        /// <param name="factory">Factory method</param>
        void Add<T>(string token, Func<T> factory);

        /// <summary>
        /// Creates the factory method for a type
        /// </summary>
        /// <typeparam name="T">Type to be queried</typeparam>
        /// <returns>Function creating the item</returns>
        T Create<T>();

        /// <summary>
        /// Creates the factory method for a type
        /// </summary>
        /// <typeparam name="T">Type to be queried</typeparam>
        /// <param name="token">Token to be queries</param>
        /// <returns>Created item</returns>
        T Create<T>(string token);

        /// <summary>
        /// Creates all items matching to the type
        /// </summary>
        /// <typeparam name="T">Type to be queried</typeparam>
        /// <returns>Enumerations of all matching types</returns>
        IEnumerable<T> CreateAll<T>();
    }
}
