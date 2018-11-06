namespace BurnSystems.Collections
{
    /// <summary>
    /// This class is just a container for holding another object. 
    /// It is required for anonymous caching objects.
    /// </summary>
    /// <typeparam name="T">Type of item to be stored</typeparam>
    public class Container<T>
    {
        /// <summary>
        /// Initializes a new instance of the Container class.
        /// </summary>
        public Container()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Container class.
        /// </summary>
        /// <param name="item">Item to be stored.</param>
        public Container(T item)
        {
            Item = item;
        }

        /// <summary>
        /// Gets or sets the item to be stored
        /// </summary>
        public T Item
        {
            get;
            set;
        }
    }
}
