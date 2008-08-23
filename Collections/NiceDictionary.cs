//-----------------------------------------------------------------------
// <copyright file="NiceDictionary.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using BurnSystems.Interfaces;

namespace BurnSystems.Collections
{
    /// <summary>
    /// Dieses Dictionary entspricht dem normalen Dictionary, nur dass 
    /// bei der Abfrage über den Indexer ein null-Wert zurückgegeben wird, 
    /// wenn dieser nicht vorhanden ist 
    /// </summary>
    [Serializable()]
    public class NiceDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IHasIndex<TKey, TValue>
    {
        /// <summary>
        /// Eingebettetes Dictionary
        /// </summary>
        Dictionary<TKey, TValue> _Dictionary;

        /// <summary>
        /// Erstellt eine neue Instanz des 'netten' Verzeichnis
        /// </summary>
        public NiceDictionary()
        {
            _Dictionary = new Dictionary<TKey, TValue>();
        }

        #region IDictionary<TKey,TValue> Member

        /// <summary>
        /// Adds a new key
        /// </summary>
        /// <param name="key">Key to be added</param>
        /// <param name="value">Value to be added</param>
        public void Add(TKey key, TValue value)
        {
            _Dictionary.Add(key, value);
        }

        /// <summary>
        /// Checks, if the dictionary contains 
        /// </summary>
        /// <param name="key">Requested key</param>
        /// <returns>true, if key exists</returns>
        public bool ContainsKey(TKey key)
        {
            return _Dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Gets a collection of keys
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { return _Dictionary.Keys; }
        }

        /// <summary>
        /// Removes an entry
        /// </summary>
        /// <param name="key">Key of entry to be removed</param>
        /// <returns>true, if entry is removed</returns>
        public bool Remove(TKey key)
        {
            return _Dictionary.Remove(key);
        }

        /// <summary>
        /// Tries to get a value
        /// </summary>
        /// <param name="key">Requested key</param>
        /// <param name="value">Output for value</param>
        /// <returns>true, if value is found</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return _Dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets a collection of values
        /// </summary>
        public ICollection<TValue> Values
        {
            get { return _Dictionary.Values; }
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
                TValue oValue;

                if (_Dictionary.TryGetValue(key, out oValue))
                {
                    return oValue;
                }
                return default(TValue);
            }
            set
            {
                _Dictionary[key] = value;
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Member

        /// <summary>
        /// Adds a new item
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _Dictionary.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Clears the dictionary
        /// </summary>
        public void Clear()
        {
            _Dictionary.Clear();
        }

        /// <summary>
        /// Checks, if the item exists
        /// </summary>
        /// <param name="item">item to be checked</param>
        /// <returns>true, if exists</returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)_Dictionary).Contains(item);
        }

        /// <summary>
        /// Copies elements
        /// </summary>
        /// <param name="array">Array to be copied</param>
        /// <param name="arrayIndex">Used index</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)_Dictionary).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of entries
        /// </summary>
        public int Count
        {
            get { return _Dictionary.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is readonly
        /// </summary>
        public bool IsReadOnly
        {
            get { return ((IDictionary<TKey, TValue>)_Dictionary).IsReadOnly; }
        }

        /// <summary>
        /// Removes an item
        /// </summary>
        /// <param name="item">Item to be removed</param>
        /// <returns>True, if removal was successful</returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _Dictionary.Remove(item.Key);
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Member

        /// <summary>
        /// Gets an enumerator
        /// </summary>
        /// <returns>Enumerator of dictionary</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _Dictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerable Member

        /// <summary>
        /// Gets an enumerator
        /// </summary>
        /// <returns>Enumerator of dictionary</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _Dictionary.GetEnumerator();
        }

        #endregion
    }
}
