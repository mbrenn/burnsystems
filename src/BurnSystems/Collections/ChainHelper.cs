#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;

namespace BurnSystems.Collections
{
    /// <summary>
    /// Provides a helper for chaining of objects. A bit like a list, but very rough
    /// </summary>
    /// <typeparam name="T">Type of chained element</typeparam>
    public class ChainHelper<T> : IEnumerable<T> where T : class, IChainNode<T>
    {
        /// <summary>
        /// Defines the head of the chain
        /// </summary>
        public T? Head { get; private set; }

        /// <summary>
        /// Defines the tail of the chain
        /// </summary>
        public T? Tail { get; private set; }

        /// <summary>
        /// Adds the given element to the chain
        /// </summary>
        /// <param name="element">Element to be added</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Add(T element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            if (Head == null || Tail == null)
            {
                Head = element;
                Tail = element;
            }
            else
            {
                Tail.Next = element;
                Tail = element;
            }
        }

        /// <summary>
        /// Performs an implicit conversion to the head element 
        /// </summary>
        /// <param name="c">Element to be converted</param>
        /// <returns>The head element</returns>
        public static implicit operator T?(ChainHelper<T> c) => c.Head;

        /// <summary>
        /// Gets the enumerator going through the chained list
        /// </summary>
        /// <returns>Enumerated elemets</returns>
        public IEnumerator<T> GetEnumerator()
        {
            var current = Head;
            while (current != null)
            {
                yield return current;
                current = current.Next;
            }
        }

        /// <summary>
        /// The enumerator itself
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}