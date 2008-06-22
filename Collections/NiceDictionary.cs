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

        public void Add(TKey key, TValue value)
        {
            _Dictionary.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return _Dictionary.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return _Dictionary.Keys; }
        }

        public bool Remove(TKey key)
        {
            return _Dictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _Dictionary.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get { return _Dictionary.Values; }
        }

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

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _Dictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _Dictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)_Dictionary).Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)_Dictionary).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _Dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((IDictionary<TKey, TValue>)_Dictionary).IsReadOnly; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _Dictionary.Remove(item.Key);
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Member

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _Dictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerable Member

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _Dictionary.GetEnumerator();
        }

        #endregion
    }
}
