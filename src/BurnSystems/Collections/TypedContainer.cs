using System;
using System.Collections.Generic;
using System.Linq;

namespace BurnSystems.Collections
{
    /// <summary>
    /// Implements a container, that stores the instances according to their type. 
    /// An instance can be retrieved by its type
    /// </summary>
    public class TypedContainer
    {
        /// <summary>
        /// Stores the object that has been stored within the instances
        /// </summary>
        private readonly List<object> _instances = new List<object>();

        /// <summary>
        /// Adds an object to the container
        /// </summary>
        /// <param name="instance">Instance to be added</param>
        public void Add(object instance)
        {
            _instances.Add(instance);
        }

        /// <summary>
        /// Gets the first object matching to this type. 
        /// If multiple objects of the type are available, an InvalidOperationException will be thrown.
        /// </summary>
        /// <typeparam name="T">Type to be requested</typeparam>
        /// <returns>Object or null, if not found</returns>
        public T Get<T>()
        {
            T found = default!;
            var alreadyFound = false;

            foreach (var instance in _instances)
            {
                if (instance is T instanceAsT)
                {
                    if (alreadyFound)
                    {
                        throw new InvalidOperationException(LocalizationBS.TypedContainer_MultipleInstance);
                    }

                    found = instanceAsT;
                    alreadyFound = true;
                }
            }

            return found;
        }

        /// <summary>
        /// Gets all objects matching to a certain type
        /// </summary>
        /// <typeparam name="T">Type to be requested</typeparam>
        /// <returns>Enumeration of found types</returns>
        public IEnumerable<T> GetAll<T>()
        {
            return _instances.
                Where(x => x is T)
                .Cast<T>();
        }
    }
}
