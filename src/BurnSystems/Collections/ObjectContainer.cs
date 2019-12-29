namespace BurnSystems.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Extensions;

    /// <summary>
    /// The objectcontainer stores the objects
    /// and offers a method to get an access to the objects
    /// </summary>
    [Serializable]
    public class ObjectContainer : IEnumerable<KeyValuePair<string, object>>
    {
        /// <summary>
        /// Stores the objects
        /// </summary>
        private readonly Dictionary<string, object> _objects;

        /// <summary>
        /// Initializes a new instance of the ObjectContainer class.
        /// </summary>
        public ObjectContainer()
        {
            _objects = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the ObjectContainer class.
        /// </summary>
        /// <param name="copy">Objectcontainer to be copied</param>
        public ObjectContainer(ObjectContainer copy)
        {
            _objects = new Dictionary<string, object>(copy._objects);
        }

        /// <summary>
        /// Gets or sets an untyped variable of the object container. 
        /// </summary>
        /// <param name="key">Key of required object</param>
        /// <returns>Object with the key</returns>
        public object this[string key]
        {
            get
            {
                lock (_objects)
                {
                    return _objects[key];
                }
            }

            set
            {
                lock (_objects)
                {
                    SetObject(key, value);
                }
            }
        }

        /// <summary>
        /// Gets an object by key
        /// </summary>
        /// <typeparam name="T">Type of requested object</typeparam>
        /// <param name="key">Name of requested object</param>
        /// <returns>Value of requested object or null, if object
        /// is not found</returns>
        public T GetObject<T>(string key) 
        {
            try
            {
                lock (_objects)
                {
                    if (_objects.TryGetValue(key, out var result))
                    {
                        return (T)result;
                    }

                    return default!;
                }
            }
            catch (InvalidCastException)
            {
                // Invalid casting
                return default!;
            }
        }

        /// <summary>
        /// Sets an object into database
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Key of object to be set</param>
        /// <param name="value">Value of object</param>
        public void SetObject<T>(string key, T value)
        {
            lock (_objects)
            {
                if (value == null)
                {
                    _objects.Remove(key);
                }
                else
                {
                    _objects[key] = value;
                }
            }
        }

        /// <summary>
        /// Tries to get an untyped value 
        /// </summary>
        /// <typeparam name="T">Type of the object that shall be converted between object and dictionary</typeparam>
        /// <param name="key">Key of the requested object.</param>
        /// <param name="result">Result of the object</param>
        /// <returns>true, if the object was found</returns>
        public bool TryGetValue<T>(string key, out T result)
        {
            lock (_objects)
            {
                var exists = _objects.TryGetValue(key, out var temp);
                if (exists && temp is T)
                {
                    result = (T)temp;
                }
                else
                {
                    result = default!;
                }

                return exists;
            }
        }

        /// <summary>
        /// This function returns a specific property, which is accessed by name
        /// </summary>
        /// <param name="name">Name of requested property</param>
        /// <returns>Property behind this object</returns>
        public object? GetProperty(string name)
        {
            return this[name];
        }

        /// <summary>
        /// This function has to execute a function and to return an object
        /// </summary>
        /// <param name="functionName">Name of function</param>
        /// <param name="parameters">Parameters for the function</param>
        /// <returns>Return of function</returns>
        public object? ExecuteFunction(string functionName, IList<object> parameters)
        {
            lock (_objects)
            {
                switch (functionName)
                {
                    case "GetSummary":
                        return StringManipulation.Join(
                            _objects.Select(
                                x => string.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0}: {1}",
                                    x.Key,
                                    x.Value.ConvertToString())),
                            "\n");
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the items as an enumeration
        /// </summary>
        /// <returns>Items of the enumeration</returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            lock (_objects)
            {
                List<KeyValuePair<string, object>> copy;

                copy = _objects.ToList();

                return copy.GetEnumerator();
            }
        }

        /// <summary>
        /// Gets the items of the enumeration
        /// </summary>
        /// <returns>Items of the enumeration</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            List<KeyValuePair<string, object>> copy;

            lock (_objects)
            {
                // Copies the items in synchronisation
                copy = _objects.ToList();
            }

            return copy.GetEnumerator();
        }
    }
}
