﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace BurnSystems.Collections
{
    /// <summary>
    /// Implements a priority queue, in which the element with 
    /// the smallest value is at first position
    /// </summary>
    /// <typeparam name="T">Type of elements</typeparam>
    public class PriorityQueue<T> : IEnumerable<T>
    {
        /// <summary>
        /// Storage for elements
        /// </summary>
        private readonly List<T> _elements = new List<T>();

        /// <summary>
        /// Comparer for elements
        /// </summary>
        private readonly Comparison<T> _comparer;

        /// <summary>
        /// Initializes a new instance of the PriorityQueue class.
        /// The empty constructor is only valid, if <c>T</c> is inherited
        /// of IComparable
        /// </summary>
        public PriorityQueue()
        {
            _comparer =
                (x, y) =>
                {
                    if (x == null || y == null)
                    {
                        throw new InvalidOperationException("One of the two values are null");
                    }
                    
                    return ((IComparable) x).CompareTo((IComparable) y);
                };

        }

        /// <summary>
        /// Initializes a new instance of the PriorityQueue class.
        /// </summary>
        /// <param name="comparer">Comparer to be used</param>
        public PriorityQueue(Comparison<T> comparer)
        {
            _comparer = comparer;
        }

        /// <summary>
        /// Gets the number of elements
        /// </summary>
        public int Count => _elements.Count;

        /// <summary>
        /// Adds a new item to priorityqueue
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void Add(T item)
        {
            var position = GetPosition(item, out _);

            _elements.Insert(position, item);
        }

        /// <summary>
        /// Removes the item from priority queue
        /// </summary>
        /// <param name="item">Item to be removed</param>
        public void Remove(T item)
        {
            var position = GetPosition(item, out var exists);

            if (exists)
            {
                _elements.RemoveAt(position);
            }
        }

        /// <summary>
        /// Checks, if the item exists
        /// </summary>
        /// <param name="item">Item to be checked.</param>
        /// <returns>True, if item exists in list</returns>
        public bool Exists(T item)
        {
            GetPosition(item, out var exists);
            return exists;
        }

        /// <summary>
        /// Gets the first entry
        /// </summary>
        /// <returns>First entry to be added</returns>
        public T Peek()
        {
            if (Count == 0)
            {
                return default!;
            }

            return _elements[0];
        }

        /// <summary>
        /// Gets the first object and removes it from queue
        /// </summary>
        /// <returns>Object to be popped.</returns>
        public T Pop()
        {
            if (Count == 0)
            {
                return default!;
            }

            // Not fast, but beautiful
            var result = _elements[0];
            _elements.RemoveAt(0);
            return result;
        }

        /// <summary>
        /// Clears the eventlist
        /// </summary>
        public void Clear()
        {
            _elements.Clear();
        }

        /// <summary>
        /// Gets all elements an array. 
        /// </summary>
        /// <returns>Array of elements</returns>
        public T[] GetElements()
        {
            return _elements.ToArray();
        }

        /// <summary>
        /// Gets an enumerator for the class.
        /// </summary>
        /// <returns>Enumerator enumerating all elements</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        /// <summary>
        /// Resorts the priorityqueue
        /// </summary>
        public void Resort()
        {
            // Resort in inverse order because the smallest objects are at the end
            _elements.Sort((x, y) => _comparer(y, x));
        }

        /// <summary>
        /// Gets an enumerator for the class.
        /// </summary>
        /// <returns>Enumerator enumerating all elements</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        /// <summary>
        /// Gets the position of the entry.
        /// </summary>
        /// <param name="item">Entry, whose position should be queried.</param>
        /// <param name="exists">true, if the element was found</param>
        /// <returns>Position of entry, which be the one of the entry. </returns>
        private int GetPosition(T item, out bool exists)
        {
            if (Count == 0)
            {
                exists = false;
                return 0;
            }

            var left = 0;
            var right = Count; // Exclusive border

            var current = right / 2;
            while (left < right && current < Count)
            {
                var compareValue = _comparer(item, _elements[current]);

                if (compareValue >= 0)
                {
                    right = current;
                }
                else
                {
                    left = current + 1;
                }

                current = (left + right) / 2;
            }

            // Found ?! 
            if (current >= Count)
            {
                exists = false;
                return current;
            }

            exists = _elements[current]?.Equals(item) == true;
            return current;
        }
    }
}
