//-----------------------------------------------------------------------
// <copyright file="CollectionExtensions.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Extensions
{
    using System.Collections.Generic;

    /// <summary>
    /// These collection extensions are used to ease the use
    /// collector class
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Sets a key-value pair in the dictionary
        /// </summary>
        /// <typeparam name="T">Type of the key</typeparam>
        /// <typeparam name="W">Type of the value</typeparam>
        /// <param name="dict">Dictionary to be modified</param>
        /// <param name="key">Key to be set</param>
        /// <param name="value">Value, which shall be assigned to key</param>
        /// <returns>The same dictionary. </returns>
        public static Dictionary<T, W> With<T, W>(
            this Dictionary<T, W> dict,
            T key,
            W value)
        {
            dict[key] = value;
            return dict;
        }
    }
}
