﻿//-----------------------------------------------------------------------
// <copyright file="ObjectContainer.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using BurnSystems.Interfaces;

    /// <summary>
    /// The objectcontainer stores the objects
    /// and offers a method to get an access to the objects
    /// </summary>
    [Serializable]
    public class ObjectContainer : IParserObject
    {
        /// <summary>
        /// Stores the objects
        /// </summary>
        private Dictionary<string, object> objects;

        /// <summary>
        /// Initializes a new instance of the ObjectContainer class.
        /// </summary>
        public ObjectContainer()
        {
            this.objects = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the ObjectContainer class.
        /// </summary>
        /// <param name="copy">Objectcontainer to be copied</param>
        public ObjectContainer(ObjectContainer copy)
        {
            this.objects = new Dictionary<string, object>(copy.objects);
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
                lock (this.objects)
                {
                    return this.objects[key];
                }
            }

            set
            {
                lock (this.objects)
                {
                    this.objects[key] = value;
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
            lock (this.objects)
            {
                object result;

                if (this.objects.TryGetValue(key, out result))
                {
                    return (T)result;
                }

                return default(T);
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
            lock (this.objects)
            {
                this.objects[key] = value;
            }
        }

        /// <summary>
        /// Tries to get an untyped value 
        /// </summary>
        /// <param name="key">Key of the requested object.</param>
        /// <param name="result">Result of the object</param>
        /// <returns>true, if the object was found</returns>
        public bool TryGetValue(string key, out object result)
        {
            lock (this.objects)
            {
                return this.objects.TryGetValue(key, out result);
            }
        }

        /// <summary>
        /// This function returns a specific property, which is accessed by name
        /// </summary>
        /// <param name="name">Name of requested property</param>
        /// <returns>Property behind this object</returns>
        public object GetProperty(string name)
        {
            return this[name];
        }

        /// <summary>
        /// This function has to execute a function and to return an object
        /// </summary>
        /// <param name="functionName">Name of function</param>
        /// <param name="parameters">Parameters for the function</param>
        /// <returns>Return of function</returns>
        public object ExecuteFunction(string functionName, IList<object> parameters)
        {
            return null;
        }
    }
}
