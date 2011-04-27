//-----------------------------------------------------------------------
// <copyright file="NotificationListView.cs" company="Martin Brenn">
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
    using System.Collections.Specialized;
    using System.ComponentModel;

    /// <summary>
    /// This listview encapsulates a container and throws all necessary 
    /// events fot the INotifyPropertyChanged and INotifyCollectionChanged interfaces.
    /// </summary>
    public class NotificationListView<T> : IList<T>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        /// <summary>
        /// Stores the list that has been abstracted
        /// </summary>
        private IList<T> container;

        /// <summary>
        /// Initializes a new instance of the NotificationListView class
        /// </summary>
        public NotificationListView()
        {
            this.container = new List<T>();
        }
        
        /// <summary>
        /// Initializes a new NotificationListView instance
        /// </summary>
        /// <param name="list">List to be set</param>
        public NotificationListView(IList<T> list)
        {
            this.container = list;
        }

        /// <summary>
        /// Determines the index of a specific item in the container.
        /// </summary>
        /// <param name="item">Item to be checked</param>
        /// <returns>Position of the item or -1.</returns>
        public int IndexOf(T item)
        {
            return this.container.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item at the specific position
        /// </summary>
        /// <param name="index">Index of the item</param>
        /// <param name="item">Item that shall be added</param>
        public void Insert(int index, T item)
        {
            this.container.Insert(index, item);
            this.OnPropertyChanged("Count");
            this.OnPropertyChanged("Item[]");
            this.OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add, 
                    item, 
                    index));
        }

        /// <summary>
        /// Removes an item at a certain position
        /// </summary>
        /// <param name="index">Index of the item to be removed</param>
        public void RemoveAt(int index)
        {
            var item = this[index];
            this.RemoveAt(index);
            this.OnPropertyChanged("Count");
            this.OnPropertyChanged("Item[]");
            this.OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove,
                    item,
                    index));
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">Index of the element</param>
        /// <returns>Item of the element</returns>
        public T this[int index]
        {
            get
            {
                return this.container[index];
            }
            set
            {
                T oldItem = this[index];
                this.container[index] = value;
                this.OnPropertyChanged("Item[]");
                this.OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Replace,
                        oldItem,
                        value,
                        index));
            }
        }

        /// <summary>
        /// Adds an item to the collection
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void Add(T item)
        {
            this.container.Add(item);
            this.OnPropertyChanged("Count");
            this.OnPropertyChanged("Item[]");
            this.OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add,
                    item,
                    this.Count));
        }

        /// <summary>
        /// Removes all items from collection
        /// </summary>
        public void Clear()
        {
            this.container.Clear();
            this.OnPropertyChanged("Count");
            this.OnPropertyChanged("Item[]");

        }

        /// <summary>
        /// Determines whether the container contains a specific value. 
        /// </summary>
        /// <param name="item">Item to be checked.</param>
        /// <returns>True, if container contains the value</returns>
        public bool Contains(T item)
        {
            return this.container.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the container to an array, starting at a particular array index
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from ICollection. 
        /// The Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.container.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of elements contained in the container.
        /// </summary>
        public int Count
        {
            get { return this.container.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the container is read only
        /// </summary>
        public bool IsReadOnly
        {
            get { return this.container.IsReadOnly; }
        }

        /// <summary>
        /// Removes a specific item
        /// </summary>
        /// <param name="item">Item to be removed</param>
        /// <returns>true, if item has been removed</returns>
        public bool Remove(T item)
        {
            var index = this.IndexOf(item);
            if (index == -1)
            {
                return false;
            }

            this.RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>Enumerator of the container</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.container.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>Enumerator of the container</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.container.GetEnumerator();
        }

        /// <summary>
        /// Throws the <c>PropertyChanged</c> event.
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        private void OnPropertyChanged(string propertyName)
        {
            var e = this.PropertyChanged;
            if (e != null)
            {
                e(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Called, when a property has been changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Throws the <c>CollectionChanged</c> event
        /// </summary>
        /// <param name="args">Arguments of the event</param>
        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            var e = this.CollectionChanged;
            if (e != null)
            {
                e(this, args);
            }
        }

        /// <summary>
        /// Called, when a collection has been changed
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
