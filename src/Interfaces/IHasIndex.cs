//-----------------------------------------------------------------------
// <copyright file="IHasIndex.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Interfaces
{
    /// <summary>
    /// Die Schnittstelle für alle Objekte, die einfach
    /// über einen Indexer erreichbar sind.
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
