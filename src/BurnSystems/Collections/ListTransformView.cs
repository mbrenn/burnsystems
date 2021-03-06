﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace BurnSystems.Collections
{
    /// <summary>
    /// This class transforms a list into another view dynamically. 
    /// It implements the interface IList, INotifyPropertyChanged and INotifyCollectionChanged correctly.
    /// 
    /// It is necessary to call Detach if the transformation is not required any more. 
    /// This list is read-only because the original list has to be modified
    /// </summary>
    /// <typeparam name="T">Type of the elements of the list that is the source type</typeparam>
    /// <typeparam name="TQ">Type of the elements of the list, that shall be delivered</typeparam>    
	public class ListTransformView<T, TQ> : IList<TQ>, IList, INotifyPropertyChanged //, INotifyCollectionChanged
    {
        /// <summary>
        /// Stores the selector
        /// </summary>
        private readonly Func<T, TQ> _selector;

        /// <summary>
        /// Stores the list
        /// </summary>
        private readonly IList<T> _list;

        /// <summary>
        /// True, if this has a fixed size
        /// </summary>
        public bool IsFixedSize => false;

        /// <summary>
        /// True, if the instance is synchronized.
        /// </summary>
        public bool IsSynchronized => false;

        /// <summary>
        /// Gets the synchronisation root
        /// </summary>
        public object SyncRoot { get; } = new object();

        /// <summary>
        /// This event is called, when a property has been changed
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the ListTransformView
        /// </summary>
        /// <param name="list">List to be transformed</param>
        /// <param name="selector">Selector to be used for transformation</param>
        public ListTransformView(IList<T> list ,Func<T,TQ> selector)
        {
            if (!(list is INotifyCollectionChanged notifyCollectionChanged))
            {
                throw new InvalidOperationException("list is not of type INotifyCollectionChanged");
            }

            if (!(list is INotifyPropertyChanged notifyPropertyChanged))
            {
                throw new InvalidOperationException("list is not of type INotifyPropertyChanged");
            }
            
            notifyPropertyChanged.PropertyChanged += OnPropertyChanged;
            notifyCollectionChanged.CollectionChanged += OnCollectionChanged;

            _list = list;
            _selector = selector;
        }

        /// <summary>
        /// This method detaches the events from the original list
        /// </summary>
        public void Detach()
        {
            
            if (!(_list is INotifyCollectionChanged notifyCollectionChanged))
            {
                throw new InvalidOperationException("list is not of type INotifyCollectionChanged");
            }

            if (!(_list is INotifyPropertyChanged notifyPropertyChanged))
            {
                throw new InvalidOperationException("list is not of type INotifyPropertyChanged");
            }
            
            notifyPropertyChanged.PropertyChanged -= OnPropertyChanged;
            notifyCollectionChanged.CollectionChanged -= OnCollectionChanged;
        }
        
		
        /// <summary>
        /// Called, if a property has been changed
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Arguments of event</param>
        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            /*
            var ev = this.CollectionChanged;
            if (ev != null)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        ev(
                            this,
                            new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Add,
                                e.NewItems
                                    .OfType<T>()
                                    .Select(x => this.selector(x))
                                    .ToList(),
                                e.NewStartingIndex));
                        break;
                    case NotifyCollectionChangedAction.Move:
                        ev(
                            this,
                            new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Move,
                                e.NewItems
                                    .OfType<T>()
                                    .Select(x => this.selector(x))
                                    .ToList(),
                                e.NewStartingIndex,
                                e.OldStartingIndex));
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        ev(
                            this,
                            new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Remove,
                                e.OldItems
                                    .OfType<T>()
                                    .Select(x => this.selector(x))
                                    .ToList(),
                                e.OldStartingIndex));
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        ev(
                            this,
                            new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Replace,
                                e.OldItems
                                    .OfType<T>()
                                    .Select(x => this.selector(x))
                                    .ToList(),
                                e.NewItems
                                    .OfType<T>()
                                    .Select(x => this.selector(x))
                                    .ToList()));
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        ev(
                            this,
                            new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Replace));
                        break;
                    default:
                        break;
                }
            }
            */
        }

        /// <summary>
        /// Called, if a property has been changed
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Arguments of event</param>
        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var ev = PropertyChanged;
            ev?.Invoke(sender, e);
        }

        /// <summary>
        /// Gets the index of a specific item
        /// </summary>
        /// <param name="item">Item to be queried</param>
        /// <returns>Index of the item</returns>
        public int IndexOf(TQ item)
        {
            var found = -1;

            foreach (var element in _list.Select(x => _selector(x)))
            {
                found++;
                if (element == null && item == null)
                {
                    return found;
                }

                if (element == null)
                {
                    continue;
                }

                if (element.Equals(item))
                {
                    return found;
                }
            }

            return -1;
        }

        /// <summary>
        /// This method is not implemented
        /// </summary>
        /// <param name="index">Index of the element</param>
        /// <param name="item">Element to be added</param>
        public void Insert(int index, TQ item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is not implemented
        /// </summary>
        /// <param name="index">Item to be removed</param>
        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets an item. 
        /// The setting of items is not implemented
        /// </summary>
        /// <param name="index">Index of the element</param>
        /// <returns>Element to be retrieved</returns>
        public TQ this[int index]
        {
            get => _selector(_list[index]);
            set => throw new NotImplementedException();
        }

        /// <summary>
        /// This method is not implemented
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void Add(TQ item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is not implemented
        /// </summary>
        public void Clear()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if an element is contained
        /// </summary>
        /// <param name="item">Item to be checked</param>
        /// <returns>True, if item is in list</returns>
        public bool Contains(TQ item)
        {
            return IndexOf(item) != -1;
        }

        /// <summary>
        /// Copies the elements to an array
        /// </summary>
        /// <param name="array">Array to be used</param>
        /// <param name="arrayIndex">Position where the data shall be copied</param>
        public void CopyTo(TQ[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException();
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            var pos = arrayIndex;
            foreach (var element in this)
            {
                array[pos] = element;
                pos++;

                if (pos >= array.Length)
                {
                    throw new ArgumentException();
                }
            }
        }

        /// <summary>
        /// Gets the number of elements
        /// </summary>
        public int Count => _list.Count;

        /// <summary>
        /// Gets a value indicating whether the list is read only
        /// </summary>
        public bool IsReadOnly => true;

        /// <summary>
        /// Method is not implemented
        /// </summary>
        /// <param name="item">Item to be removed</param>
        /// <returns>true, if item has been removed</returns>
        public bool Remove(TQ item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>Enumerator for the instance</returns>
        public IEnumerator<TQ> GetEnumerator()
        {
            foreach (var item in _list)
            {
                yield return _selector(item);
            }
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>Enumerator for the instance</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var item in _list)
            {
                yield return _selector(item);
            }
        }

        /// <summary>
        /// This method is not implemented
        /// </summary>
        /// <param name="value">Value to be added</param>
        /// <returns>Position of new item</returns>
        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if the item is inclued
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <returns>true, if item is included</returns>
        public bool Contains(object value)
        {
            return IndexOf(value) != -1;
        }

        /// <summary>
        /// Gets the index of the item within the array
        /// </summary>
        /// <param name="value">Item whose index is required</param>
        /// <returns>Index of item or -1 if not existing</returns>
        public int IndexOf(object value)
        {
            var realValue = value is TQ;
            if (!realValue)
            {
                // Not null, but not of type value
                return -1;
            }

            return IndexOf((TQ) value);
        }

        /// <summary>
        /// Method not implemented
        /// </summary>
        /// <param name="index">Index of the new element</param>
        /// <param name="value">Value of the element</param>
        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="value">Item to be removed</param>
        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets a specific item. The setting of items is not implemented
        /// </summary>
        /// <param name="index">Index of the item</param>
        /// <returns>Requested object</returns>
        object? IList.this[int index]
        {
            get => this[index];
            set => throw new NotImplementedException();
        }

        /// <summary>
        /// Copies the elements to an array
        /// </summary>
        /// <param name="array">Array to be filled</param>
        /// <param name="index">Index within the array</param>
        public void CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException();
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            var pos = index;
            foreach (var element in this)
            {
                array.SetValue(element, pos);
                pos++;

                if (pos >= array.Length)
                {
                    throw new ArgumentException();
                }
            }
        }

    }
}
