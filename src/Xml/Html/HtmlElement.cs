//-----------------------------------------------------------------------
// <copyright file="HtmlDocument.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Xml.Html
{
    using System.Xml.Linq;

    /// <summary>
    /// Helper class that is used to create a simple html element
    /// </summary>
    public class HtmlElement : XElement
    {
        /// <summary>
        /// Initializes a new instance of the HtmlElement class.
        /// </summary>
        /// <param name="htmlTag">Tag to be added</param>
        /// <param name="contents">Contents of the html node</param>
        public HtmlElement(
            string htmlTag,
            params object[] contents)
            : base(HtmlDocument.XHtml5Namespace + htmlTag, contents)
        {
        }
    }
}
