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

using System;
using System.Collections.Generic;
using System.Text;

namespace BurnSystems.Interfaces
{
    /// <summary>
    /// Die Schnittstelle für alle Objekte, die einfach
    /// über einen Indexer erreichbar sind.
    /// </summary>
    public interface IHasIndex<TKey, TValue>
    {
        /// <summary>
        /// Über diesen Indexer findet der Zugriff auf das Objekt statt
        /// </summary>
        /// <param name="oKey">Schlüssel</param>
        /// <returns>Wert</returns>
        TValue this[TKey oKey]
        {
            get;
        }
    }
}
