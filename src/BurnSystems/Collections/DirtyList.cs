using System.Collections;
using System.Collections.Generic;

namespace BurnSystems.Collections
{
    /// <summary>
    /// Abstracts a standard list by adding a dirty flag which gets dirty
    /// whenever a field is set, added or removed. 
    /// The flag will not set to dirty, when the field internally is modified.
    /// The dirty flag can be cleared.  
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DirtyList<T> : IList<T>
    {
        /// <summary>
        /// Contains the list implementation
        /// </summary>
        private IList<T> _listImplementation = new List<T>();

        /// <summary>
        /// Gets or sets the flag whether the list is dirty
        /// </summary>
        public bool IsDirty { get; private set; }

        /// <summary>
        /// Clears the dirty flag
        /// </summary>
        public void ClearDirty()
        {
            IsDirty = false;
        }

        /// <summary>
        /// Returns the current dirty flag and resets it
        /// </summary>
        /// <returns></returns>
        public bool PopDirty()
        {
            var result = IsDirty;
            IsDirty = false;
            return result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _listImplementation.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _listImplementation).GetEnumerator();
        }

        public void Add(T item)
        {
            IsDirty = true;
            _listImplementation.Add(item);
        }

        public void Clear()
        {
            IsDirty = true;
            _listImplementation.Clear();
        }

        public bool Contains(T item)
        {
            return _listImplementation.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _listImplementation.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            IsDirty = true;
            return _listImplementation.Remove(item);
        }

        public int Count => _listImplementation.Count;

        public bool IsReadOnly => _listImplementation.IsReadOnly;

        public int IndexOf(T item)
        {
            return _listImplementation.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            IsDirty = true;
            _listImplementation.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            IsDirty = true;
            _listImplementation.RemoveAt(index);
        }

        public T this[int index]
        {
            get => _listImplementation[index];
            set
            {
                IsDirty = true;
                _listImplementation[index] = value;
            }
        }
    }
}