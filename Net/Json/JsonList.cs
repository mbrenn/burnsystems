//-----------------------------------------------------------------------
// <copyright file="JsonList.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Net.Json
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    
    /// <summary>
    /// Stores a list of json entries
    /// </summary>
    public class JsonList : IJsonObject
    {
        /// <summary>
        /// Stores the list of json objects
        /// </summary>
        private List<IJsonObject> list =
            new List<IJsonObject>();

        /// <summary>
        /// Gets a list of json objects
        /// </summary>
        public List<IJsonObject> List
        {
            get { return this.list; }
        }

        /// <summary>
        /// Converts the list to a json string
        /// </summary>
        /// <returns>This object as a json string</returns>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append('[');

            var komma = string.Empty;

            foreach (var item in this.List)
            {
                stringBuilder.AppendFormat(
                    "{0}{1}",
                    komma, item.ToString());
                komma = ",";
            }

            stringBuilder.Append(']');

            return stringBuilder.ToString();
        }
    }
}
