//-----------------------------------------------------------------------
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

    /// <summary>
    /// The objectcontainer stores the objects
    /// and offers a method to get an access to the objects
    /// </summary>
    [Serializable]
    public class ObjectContainer
    {
        /// <summary>
        /// Stores the objects
        /// </summary>
        private Dictionary<string, object> objects =
            new Dictionary<string, object>();

        /// <summary>
        /// Gets an object by key
        /// </summary>
        /// <typeparam name="T">Type of requested object</typeparam>
        /// <param name="key">Name of requested object</param>
        /// <returns>Value of requested object or null, if object
        /// is not found</returns>
        public T GetObject<T>(string key) where T : class
        {
            object result;

            if (this.objects.TryGetValue(key, out result))
            {
                return (T)result;
            }

            return null;
        }

        /// <summary>
        /// Sets an object into database
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Key of object to be set</param>
        /// <param name="value">Value of object</param>
        public void SetObject<T>(string key, T value)
        {
            this.objects[key] = value;
        }
    }
}
