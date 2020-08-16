#nullable enable

namespace BurnSystems.Collections
{
    /// <summary>
    /// Defines the interface for elements which are a node in the linked list- 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IChainNode<T> where T : class
    {
        /// <summary>
        /// Gets or sets the next element in the chain
        /// </summary>
        T? Next { get; set; }
    }
}