//-----------------------------------------------------------------------
// <copyright file="AutoDictionary.cs" company="Martin Brenn">
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
    /// This interface has to be implemented by all objects, having a certain
    /// key-object and wants to be used the advantages of the autodictionary
    /// for example.
    /// </summary>
    public interface IHasKey
    {
        /// <summary>
        /// Gets the name of the key of the instance
        /// </summary>
        string Key
        {
            get;
        }
    }

    /// <summary>
    /// This class stores object, whose classes implements the interface IHasKey.
    /// IHasKey offers an access to the name of the instance
    /// </summary>
    /// <typeparam name="T">Type of objects to be stored</typeparam>
    public class AutoDictionary<T> : IDictionary<string, T> where T : IHasKey
    {
        /// <summary>
        /// Object storing the instances
        /// </summary>
        private Dictionary<string, T> dictionary = new Dictionary<string, T>();

        /// <summary>
        /// Adds a new object. If there is already an object with the same name, it
        /// will be overwritten
        /// </summary>
        /// <param name="o">Object to be added</param>
        public void Add(T o)
        {
            this.dictionary[o.Key] = o;
        }

        /// <summary>
        /// Returns an object with the given key
        /// </summary>
        /// <param name="key">Requested Key</param>
        /// <returns>Object with the key. If no key is found, an exception 
        /// will be thrown.</returns>
        public T this[string key]
        {
            get { return this.dictionary[key]; }
            set { this.Add(key, value); }
        }

        /// <summary>
        /// Removes the object from database
        /// </summary>
        /// <param name="key">Key of the object. </param>
        /// <returns>true, if the object was found and removed</returns>
        public bool Remove(string key)
        {
            return this.dictionary.Remove(key);
        }

        #region IDictionary<string,T> Member

        public void Add(string key, T value)
        {
            if (value.Key != key)
            {
                throw new ArgumentException("key != value.key");
            }

            this.Add(value);
        }

        public bool ContainsKey(string key)
        {
            return this.dictionary.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return this.dictionary.Keys; }
        }
    
        public bool TryGetValue(string key, out T value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }

        public ICollection<T> Values
        {
            get { return this.dictionary.Values; }
        }
        
        #endregion

        #region ICollection<KeyValuePair<string,T>> Member

        public void Add(KeyValuePair<string, T> item)
        {
            this.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            this.dictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, T> item)
        {
            return this.dictionary.ContainsKey(item.Key)
                && (item.Value.Key == item.Key);
        }

        public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return this.dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<string, T> item)
        {
            if (item.Key == item.Value.Key)
            {
                return this.Remove(item.Key);
            }

            return false;
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,T>> Member

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerable Member

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        #endregion
    }
}
