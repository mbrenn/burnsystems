using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.Interfaces;
using System.Xml.Linq;

namespace BurnSystems.Collections
{
    /// <summary>
    /// This implements the IList interface on an XContainer element. 
    /// All addings, deletions, changes on this container will be directly done
    /// on the element
    /// </summary>
    /// <typeparam name="T">Type of the entity</typeparam>
    public class XmlList<T> : IList<T>
    {
        /// <summary>
        /// Stores the converter
        /// </summary>
        private IXElementConverter<T> converter;

        /// <summary>
        /// Container node being associated the list
        /// </summary>
        private XContainer container;

        /// <summary>
        /// Initializes a new instance of the XmlList class.
        /// </summary>
        /// <param name="container">Xml-Container storing the elements</param>
        /// <param name="converter">Converter to be used to convert items to xml and vice versa</param>
        public XmlList(XContainer container, IXElementConverter<T> converter)
        {
            this.container = container;
            this.converter = converter;
        }

        /// <summary>
        /// Gets the index of the item
        /// </summary>
        /// <param name="item">Item to be requested</param>
        /// <returns>Index of the item</returns>
        public int IndexOf(T item)
        {
            var pos = 0;

            foreach (var entity in
                this.container.Elements()
                    .Select(x => converter.Convert(x)))
            {
                if (entity != null && entity.Equals(item))
                {
                    return pos;
                }

                pos++;
            }

            return -1;
        }

        /// <summary>
        /// Inserts a new item to the xmllist
        /// </summary>
        /// <param name="index">Index of the item to be added</param>
        /// <param name="item">Item to be added</param>
        public void Insert(int index, T item)
        {
            var element = this.container.Elements()
                .ElementAtOrDefault(index - 1);

            if (element == null)
            {
                this.container.Add(converter.Convert(item));
            }
            else
            {
                element.AddAfterSelf(converter.Convert(item));
            }
        }

        /// <summary>
        /// Removes an item at a certain position
        /// </summary>
        /// <param name="index">Index of the item</param>
        public void RemoveAt(int index)
        {
            var element = this.container.Elements().ElementAtOrDefault(index);
            if (element != null)
            {
                element.Remove();
            }
        }

        /// <summary>
        /// Gets or sets an element at a certain position
        /// </summary>
        /// <param name="index">Index of the item</param>
        /// <returns>Item at position</returns>
        public T this[int index]
        {
            get
            {
                var element = this.container.Elements().ElementAt(index);
                return converter.Convert(element);
            }
            set
            {
                var element = this.container.Elements().ElementAtOrDefault(index);
                if (element != null)
                {
                    element.ReplaceWith(converter.Convert(value));
                }
                else
                {
                    this.container.Add(converter.Convert(value));
                }
            }
        }

        /// <summary>
        /// Adds an item
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void Add(T item)
        {
            this.container.Add(converter.Convert(item));
        }

        /// <summary>
        /// Clears the complete list
        /// </summary>
        public void Clear()
        {
            this.container.RemoveNodes();
        }

        /// <summary>
        /// Checks whether the xml list contains a specific item
        /// </summary>
        /// <param name="item">Item which shall be checked</param>
        /// <returns>true, if item is included</returns>
        public bool Contains(T item)
        {
            return this.IndexOf(item) != -1;
        }

        /// <summary>
        /// Copies the complete list to an array
        /// </summary>
        /// <param name="array">Array to be added</param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
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
        public int Count
        {
            get { return this.container.Elements().Count(); }
        }

        /// <summary>
        /// Gets a value indicating whether the list is read-only
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes a specific item from xml list
        /// </summary>
        /// <param name="item">Item to be removed</param>
        /// <returns>True, if item has been removed</returns>
        public bool Remove(T item)
        {
            var position = this.IndexOf(item);
            if (position != -1)
            {
                this.RemoveAt(position);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets an enumerator for all elements. 
        /// Elements being null are skipped
        /// </summary>
        /// <returns>Enumerator for the list</returns>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in this.container.Elements()
                .Select(x => converter.Convert(x))
                .Where(x => x != null))
            {
                yield return item;
            }
        }

        /// <summary>
        /// Gets an enumerator for all elements. 
        /// Elements being null are skipped
        /// </summary>
        /// <returns>Enumerator for the list</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
