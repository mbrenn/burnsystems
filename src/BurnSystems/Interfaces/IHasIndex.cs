namespace BurnSystems.Interfaces
{
    /// <summary>
    /// This interface is used for all objects having a
    /// readable index which can not be written.
    /// </summary>
    /// <typeparam name="TKey">Type of key storing a value</typeparam>
    /// <typeparam name="TValue">Type of value behind a key</typeparam>
    public interface IHasIndex<TKey, TValue>
    {
        /// <summary>
        /// Über diesen Indexer findet der Zugriff auf das Objekt statt
        /// </summary>
        /// <param name="key">Request key</param>
        /// <returns>Result of value behind key</returns>
        TValue this[TKey key]
        {
            get;
        }
    }
}
