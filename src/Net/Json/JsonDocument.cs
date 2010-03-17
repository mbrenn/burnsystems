//-----------------------------------------------------------------------
// <copyright file="JsonDocument.cs" company="Martin Brenn">
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
    /// <summary>
    /// Returns a json document, which is a simple object enclosed
    /// by round brackets
    /// </summary>
    public class JsonDocument : JsonObject
    {
        /// <summary>
        /// Converts the object to a string
        /// </summary>
        /// <returns>String to be converted</returns>
        public override string ToString()
        {
            return string.Format("({0})", base.ToString());
        }
    }
}
