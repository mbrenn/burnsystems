namespace BurnSystems.Xml.Html
{
    using System.Xml.Linq;

    /// <summary>
    /// Offers some helpermethods that can be used to manipulate and to create html documents
    /// </summary>
    public static class HtmlDocument
    {
        /// <summary>
        /// Defines the namespace for xhtml 
        /// </summary>
        public static XNamespace XHtml5Namespace = XNamespace.Get("http://www.w3.org/1999/xhtml");

        /// <summary>
        /// Creates an Html Document of Version 5
        /// </summary>
        /// <returns></returns>
        public static XDocument Create(string title, params XElement[] contents)
        {
            var result = new XDocument(
                new XDocumentType("html", null, null, null),
                new HtmlElement(
                    "html",
                    new HtmlElement(
                        "head",
                        new HtmlElement("title", title)),
                    new HtmlElement(
                        "body",
                        contents)));

            return result;
        }
    }
}
