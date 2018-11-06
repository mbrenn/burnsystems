namespace BurnSystems.Collections
{
    using System;
    using System.Collections.Generic;
    using Interfaces;

    /// <summary>
    /// Dieses Dictionary entspricht dem normalen Dictionary, nur dass 
    /// bei der Abfrage über den Indexer ein null-Wert zurückgegeben wird, 
    /// wenn dieser nicht vorhanden ist 
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="TValue">Type of values in dictionary</typeparam>
    [Serializable()]
    public class NiceDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IHasIndex<TKey, TValue>
    {
        /// <summary>
        /// Eingebettetes Dictionary
        /// </summary>
        private Dictionary<TKey, TValue> dictionary;

        /// <summary>
        /// Initializes a new instance of the NiceDictionary class.
        /// </summary>
        public NiceDictionary()
        {
            dictionary = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Gets a collection of keys
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { return dictionary.Keys; }
        }

        /// <summary>
        /// Gets the number of entries
        /// </summary>
        public int Count
        {
            get { return dictionary.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is readonly
        /// </summary>
        public bool IsReadOnly
        {
            get { return ((IDictionary<TKey, TValue>)dictionary).IsReadOnly; }
        }

        /// <summary>
        /// Gets a collection of values
        /// </summary>
        public ICollection<TValue> Values
        {
            get { return dictionary.Values; }
        }

        /// <summary>
        /// Gets a specific entry or null, if not found
        /// </summary>
        /// <param name="key">Requested key</param>
        /// <returns>Value of key or null</returns>
        public TValue this[TKey key]
        {
            get
            {
                TValue value;

                if (dictionary.TryGetValue(key, out value))
                {
                    return value;
                }

                return default(TValue);
            }

            set
            {
                dictionary[key] = value;
            }
        }

        #region IDictionary<TKey,TValue> Member

        /// <summary>
        /// Adds a new key
        /// </summary>
        /// <param name="key">Key to be added</param>
        /// <param name="value">Value to be added</param>
        public void Add(TKey key, TValue value)
        {
            dictionary.Add(key, value);
        }

        /// <summary>
        /// Checks, if the dictionary contains 
        /// </summary>
        /// <param name="key">Requested key</param>
        /// <returns>true, if key exists</returns>
        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Removes an entry
        /// </summary>
        /// <param name="key">Key of entry to be removed</param>
        /// <returns>true, if entry is removed</returns>
        public bool Remove(TKey key)
        {
            return dictionary.Remove(key);
        }

        /// <summary>
        /// Tries to get a value
        /// </summary>
        /// <param name="key">Requested key</param>
        /// <param name="value">Output for value</param>
        /// <returns>true, if value is found</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Member

        /// <summary>
        /// Adds a new item
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            dictionary.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Clears the dictionary
        /// </summary>
        public void Clear()
        {
            dictionary.Clear();
        }

        /// <summary>
        /// Checks, if the item exists
        /// </summary>
        /// <param name="item">item to be checked</param>
        /// <returns>true, if exists</returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)dictionary).Contains(item);
        }

        /// <summary>
        /// Copies elements
        /// </summary>
        /// <param name="array">Array to be copied</param>
        /// <param name="arrayIndex">Used index</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)dictionary).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes an item
        /// </summary>
        /// <param name="item">Item to be removed</param>
        /// <returns>True, if removal was successful</returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return dictionary.Remove(item.Key);
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Member

        /// <summary>
        /// Gets an enumerator
        /// </summary>
        /// <returns>Enumerator of dictionary</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerable Member

        /// <summary>
        /// Gets an enumerator
        /// </summary>
        /// <returns>Enumerator of dictionary</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        #endregion
    }
}
