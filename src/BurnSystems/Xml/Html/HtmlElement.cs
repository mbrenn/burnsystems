using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BurnSystems.Xml.Html
{
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

        /// <summary>
        /// Creates a new instance of the HtmlElement and sets the html tag and includes the content
        /// </summary>
        /// <param name="htmlTag">Html Tag to be created</param>
        /// <param name="contents">Contents of the new html tag</param>
        /// <returns>Created html tag</returns>
        public static HtmlElement Create(
            string htmlTag,
            params object[] contents)
        {
            return new HtmlElement(htmlTag, contents);
        }

        /// <summary>
        /// Gets the text and converts each newline character (\r\n) to a <br />-tag. 
        /// The result can directly be added to a node
        /// </summary>
        /// <param name="text">Text to be converted</param>
        /// <returns>Array of elements containing text and newline breaks. </returns>
        public static object[] ConvertNewLineToBreaks(string text)
        {
            var result = new List<object>();
            var notFirst = false;
            foreach (var element in text.Split('\r').Select(x => x.Trim()))
            {
                if (notFirst)
                {
                    result.Add(new XElement("br"));
                }

                result.Add(element);
                notFirst = true;
            }

            return result.ToArray();
        }
    }
}
