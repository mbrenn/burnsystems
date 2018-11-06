namespace BurnSystems.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Interfaces;

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
                container.Elements()
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
            var element = container.Elements()
                .ElementAtOrDefault(index - 1);

            if (element == null)
            {
                container.Add(converter.Convert(item));
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
            var element = container.Elements().ElementAtOrDefault(index);
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
                var element = container.Elements().ElementAt(index);
                return converter.Convert(element);
            }
            set
            {
                var element = container.Elements().ElementAtOrDefault(index);
                if (element != null)
                {
                    element.ReplaceWith(converter.Convert(value));
                }
                else
                {
                    container.Add(converter.Convert(value));
                }
            }
        }

        /// <summary>
        /// Adds an item
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void Add(T item)
        {
            container.Add(converter.Convert(item));
        }

        /// <summary>
        /// Clears the complete list
        /// </summary>
        public void Clear()
        {
            container.RemoveNodes();
        }

        /// <summary>
        /// Checks whether the xml list contains a specific item
        /// </summary>
        /// <param name="item">Item which shall be checked</param>
        /// <returns>true, if item is included</returns>
        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
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
        public int Count => container.Elements().Count();

        /// <summary>
        /// Gets a value indicating whether the list is read-only
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Removes a specific item from xml list
        /// </summary>
        /// <param name="item">Item to be removed</param>
        /// <returns>True, if item has been removed</returns>
        public bool Remove(T item)
        {
            var position = IndexOf(item);
            if (position != -1)
            {
                RemoveAt(position);
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
            foreach (var item in container.Elements()
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
            return GetEnumerator();
        }

        /// <summary>
        /// Creates an xml-List whose values matches to a specific attribute of all subnodes
        /// </summary>
        /// <param name="container">Container element containing values of all subnodes</param>
        /// <param name="nodeName">Name of the node containing the information</param>
        /// <param name="attributeName">Name of the attribute that is requested</param>
        /// <returns>List of information</returns>
        public static IList<T> GetListForAttributes(XContainer container, string nodeName, string attributeName)
        {
            var converter = new AttributeEntityConverter<T>(nodeName, attributeName);

            return new XmlList<T>(
                container,
                converter);
        }

        /// <summary>
        /// Creates an xml-List whose values matches to a specific element of all subnodes
        /// </summary>
        /// <param name="container">Container element containing values of all subnodes</param>
        /// <param name="nodeName">Name of the node containing the information</param>
        /// <returns>List of information</returns>
        public static IList<T> GetListForElements(XContainer container, string nodeName)
        {
            var converter = new ElementEntityConverter<T>(nodeName);

            return new XmlList<T>(
                container,
                converter);
        }

        /// <summary>
        /// Converts an attribute of all subelements to a specific type
        /// </summary>
        internal class AttributeEntityConverter<Q> : IXElementConverter<Q>
        {
            /// <summary>
            /// Name of the node
            /// </summary>
            private string nodeName;
            
            /// <summary>
            /// Name of the attribute
            /// </summary>
            private string attributeName;

            /// <summary>
            /// Initializes a new instance of the AttributeEntityConverter class
            /// </summary>
            /// <param name="nodeName">Name of the node</param>
            /// <param name="attributeName">Name of the attribute</param>
            public AttributeEntityConverter(string nodeName, string attributeName)
            {
                this.nodeName = nodeName;
                this.attributeName = attributeName;
            }

            /// <summary>
            /// Converts the element to the type
            /// </summary>
            /// <param name="element"></param>
            /// <returns></returns>
            public Q Convert(XElement element)
            {
                if (element.Name != nodeName)
                {
                    return default(Q);
                }

                var attribute = element.Attribute(attributeName);
                if (attribute == null)
                {
                    return default(Q);
                }

                return (Q)System.Convert.ChangeType(attribute.Value, typeof(Q));
            }

            /// <summary>
            /// Converts an entity to the element
            /// </summary>
            /// <param name="entity">Entity to be converted</param>
            /// <returns>Entity as an XElement</returns>
            public XElement Convert(Q entity)
            {
                return new XElement(
                    nodeName,
                    new XAttribute(attributeName, entity.ToString()));
            }
        }

        /// <summary>
        /// Converts an element of all subelements to a specific type
        /// </summary>
        internal class ElementEntityConverter<Q> : IXElementConverter<Q>
        {
            /// <summary>
            /// Name of the node
            /// </summary>
            private string nodeName;

            /// <summary>
            /// Initializes a new instance of the AttributeEntityConverter class
            /// </summary>
            /// <param name="nodeName">Name of the node</param>
            public ElementEntityConverter(string nodeName)
            {
                this.nodeName = nodeName;
            }

            /// <summary>
            /// Converts the element to the type
            /// </summary>
            /// <param name="element"></param>
            /// <returns></returns>
            public Q Convert(XElement element)
            {
                if (element.Name != nodeName)
                {
                    return default(Q);
                }

                return (Q)System.Convert.ChangeType(element.Value, typeof(Q));
            }

            /// <summary>
            /// Converts an entity to the element
            /// </summary>
            /// <param name="entity">Entity to be converted</param>
            /// <returns>Entity as an XElement</returns>
            public XElement Convert(Q entity)
            {
                return new XElement(
                    nodeName,
                    entity.ToString());
            }
        }
    }
}
