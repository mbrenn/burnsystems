﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace BurnSystems.Collections
{
    /// <summary>
    ///     This listview encapsulates a container and throws all necessary
    ///     events fot the INotifyPropertyChanged and INotifyCollectionChanged interfaces.
    /// </summary>
    public class NotificationListView<T> : IList<T>, IList, INotifyPropertyChanged
    {
        /// <summary>
        ///     Stores the list that has been abstracted
        /// </summary>
        private readonly IList<T> _container;

        /// <summary>
        ///     Stores the synchronisation root
        /// </summary>
        private readonly object _syncRoot = new object();

        /// <summary>
        ///     Called, when a collection has been changed
        /// </summary>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        /// <summary>
        ///     Gets the number of elements contained in the container.
        /// </summary>
        public int Count => _container.Count;

        /// <summary>
        ///     Gets a value indicating whether the container is read only
        /// </summary>
        public bool IsReadOnly => _container.IsReadOnly;

        /// <summary>
        ///     Called, when a property has been changed
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        ///     Initializes a new instance of the NotificationListView class
        /// </summary>
        public NotificationListView()
        {
            _container = new List<T>();
        }

        /// <summary>
        ///     Initializes a new NotificationListView instance
        /// </summary>
        /// <param name="list">List to be set</param>
        public NotificationListView(IList<T> list)
        {
            _container = list;
        }

        /// <summary>
        ///     Determines the index of a specific item in the container.
        /// </summary>
        /// <param name="item">Item to be checked</param>
        /// <returns>Position of the item or -1.</returns>
        public int IndexOf(T item)
        {
            return _container.IndexOf(item);
        }

        /// <summary>
        ///     Inserts an item at the specific position
        /// </summary>
        /// <param name="index">Index of the item</param>
        /// <param name="item">Item that shall be added</param>
        public void Insert(int index, T item)
        {
            _container.Insert(index, item);
            OnPropertyChanged("Count");
            OnPropertyChanged("Item[]");
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add,
                    item,
                    index));
        }

        /// <summary>
        ///     Removes an item at a certain position
        /// </summary>
        /// <param name="index">Index of the item to be removed</param>
        public void RemoveAt(int index)
        {
            var item = this[index];
            _container.RemoveAt(index);
            OnPropertyChanged("Count");
            OnPropertyChanged("Item[]");
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove,
                    item,
                    index));
        }

        /// <summary>
        ///     Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">Index of the element</param>
        /// <returns>Item of the element</returns>
        public T this[int index]
        {
            get => _container[index];
            set
            {
                var oldItem = this[index];
                _container[index] = value;
                OnPropertyChanged("Item[]");
                OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Replace,
                        value,
                        oldItem,
                        index));
            }
        }

        /// <summary>
        ///     Adds an item to the collection
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void Add(T item)
        {
            _container.Add(item);
            OnPropertyChanged("Count");
            OnPropertyChanged("Item[]");
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add,
                    item,
                    Count - 1));
        }

        /// <summary>
        ///     Removes all items from collection
        /// </summary>
        public void Clear()
        {
            _container.Clear();
            OnPropertyChanged("Count");
            OnPropertyChanged("Item[]");
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        ///     Determines whether the container contains a specific value.
        /// </summary>
        /// <param name="item">Item to be checked.</param>
        /// <returns>True, if container contains the value</returns>
        public bool Contains(T item)
        {
            return _container.Contains(item);
        }

        /// <summary>
        ///     Copies the elements of the container to an array, starting at a particular array index
        /// </summary>
        /// <param name="array">
        ///     The one-dimensional Array that is the destination of the elements copied from ICollection.
        ///     The Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _container.CopyTo(array, arrayIndex);
        }

        /// <summary>
        ///     Removes a specific item
        /// </summary>
        /// <param name="item">Item to be removed</param>
        /// <returns>true, if item has been removed</returns>
        public bool Remove(T item)
        {
            var index = IndexOf(item);
            if (index == -1) return false;

            RemoveAt(index);
            return true;
        }

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>Enumerator of the container</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _container.GetEnumerator();
        }

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>Enumerator of the container</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _container.GetEnumerator();
        }

        /// <summary>
        ///     Throws the <c>PropertyChanged</c> event.
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        private void OnPropertyChanged(string propertyName)
        {
            var e = PropertyChanged;
            e?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     Throws the <c>CollectionChanged</c> event
        /// </summary>
        /// <param name="args">Arguments of the event</param>
        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            var e = CollectionChanged;
            e?.Invoke(this, args);
        }

        #region Implementation of the IList interface

        /// <summary>
        ///     Adds an item to list
        /// </summary>
        /// <param name="value">Value to be added</param>
        /// <returns>Position of recently added item</returns>
        public int Add(object value)
        {
            if (!(value is T)) throw new InvalidCastException("value");

            Add((T) value);
            return Count - 1;
        }

        /// <summary>
        ///     Checks whether the list contains the value
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <returns>True, if item is available</returns>
        public bool Contains(object value)
        {
            if (!(value is T)) return false;

            return Contains((T) value);
        }

        /// <summary>
        ///     Gets the position of the value
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <returns>Position of value</returns>
        public int IndexOf(object value)
        {
            if (!(value is T)) return -1;

            return IndexOf((T) value);
        }

        /// <summary>
        ///     Inserts an item to database
        /// </summary>
        /// <param name="index">Index of the item</param>
        /// <param name="value">Value to be added</param>
        public void Insert(int index, object value)
        {
            if (!(value is T)) throw new InvalidCastException("value");

            Insert(index, (T) value);
        }

        /// <summary>
        ///     Returns false
        /// </summary>
        public bool IsFixedSize => false;

        /// <summary>
        ///     Removes an item
        /// </summary>
        /// <param name="value">Value to be removed</param>
        public void Remove(object value)
        {
            if (!(value is T)) return;

            Remove((T) value);
        }

        /// <summary>
        ///     Gets an object
        /// </summary>
        /// <param name="index">Index of the object</param>
        /// <returns>Found object</returns>
        object? IList.this[int index]
        {
            get => this[index];
            set
            {
                if (!(value is T)) throw new InvalidCastException("value");

                this[index] = (T) value;
            }
        }

        /// <summary>
        ///     Copies the items to the array
        /// </summary>
        /// <param name="array">Array to be filled</param>
        /// <param name="index">Index where to start</param>
        public void CopyTo(Array array, int index)
        {
            ((IList) _container).CopyTo(array, index);
        }

        /// <summary>
        ///     Gets the information whether the list is synchronized
        /// </summary>
        public bool IsSynchronized => false;

        /// <summary>
        ///     Gets the synchronisation object
        /// </summary>
        public object SyncRoot => _syncRoot;

        #endregion
    }
}