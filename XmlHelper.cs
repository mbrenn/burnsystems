//-----------------------------------------------------------------------
// <copyright file="XmlHelper.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Xml;
    using BurnSystems.Test;

    /// <summary>
    /// Helperclass for improving access to xml documents
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// Returns the content of an xml-Attribute. If the requested attribute
        /// is not found, the <c>defaultvalue</c> will be returned.
        /// </summary>
        /// <param name="xmlNode">Requested xmlnode</param>
        /// <param name="attributeName">Requested Attributename</param>
        /// <param name="defaultvalue">Defaultvalue, which will be returned
        /// if the attribute was not found. </param>
        /// <returns>Defaultvalue or found Xml-Attribute</returns>
        public static string QueryXmlAttributeText(
            XmlNode xmlNode, 
            string attributeName, 
            string defaultvalue)
        {
            Ensure.IsNotNull(xmlNode);
            Ensure.IsNotNull(attributeName);

            XmlAttribute xmlAttribute = xmlNode.Attributes[attributeName];

            if (xmlAttribute == null)
            {
                return defaultvalue;
            }

            return xmlAttribute.InnerText;
        }

        /// <summary>
        /// Returns the content of an xml-Attribute. If the requested attribute
        /// is not found, <c>string.Empty</c> will be returned.
        /// </summary>
        /// <param name="xmlNode">Requested xmlnode</param>
        /// <param name="attributeName">Requested Attributename</param>
        /// <returns>Defaultvalue or found Xml-Attribute</returns>
        public static string QueryXmlAttributeText(XmlNode xmlNode, string attributeName)
        {
            return QueryXmlAttributeText(xmlNode, attributeName, string.Empty);
        }

        /// <summary>
        /// Returns the content of an xml-Attribute. If the requested attribute
        /// is not found, an exception will be thrown. 
        /// </summary>
        /// <param name="xmlNode">Requested xmlnode</param>
        /// <param name="attributeName">Requested Attributname</param>
        /// <returns>Found attribute as <c>XmlAttribute</c>-Structure</returns>
        public static XmlAttribute QueryXmlAttribute(
            XmlNode xmlNode, 
            string attributeName)
        {
            Ensure.IsNotNull(xmlNode);
            Ensure.IsNotNull(attributeName);
            Ensure.IsNotNull(xmlNode.Attributes);

            XmlAttribute xmlAttribute = xmlNode.Attributes[attributeName];

            if (xmlAttribute == null)
            {
                throw new InvalidOperationException(String.Format(
                    CultureInfo.CurrentUICulture,
                    LocalizationBS.XmlHelper_AttributeNotFound, 
                    attributeName));
            }

            return xmlAttribute;
        }

        /// <summary>
        /// Fragt einen Xml-Knoten per XPath ab. 
        /// </summary>
        /// <param name="xmlNode">Xml node to be queried</param>
        /// <param name="xpathQuery">Used XPath-Query</param>
        /// <returns>Found Xmlnode</returns>
        /// <exception cref="InvalidOperationException">Thrown,
        /// if no xmlnode is returned by query</exception>
        public static XmlNode QuerySingleXmlNode(XmlNode xmlNode, string xpathQuery)
        {
            Ensure.IsNotNull(xmlNode);
            Ensure.IsNotNull(xpathQuery);

            XmlNode xmlFoundNode = xmlNode.SelectSingleNode(xpathQuery);
            if (xmlFoundNode == null)
            {
                throw new InvalidOperationException(String.Format(
                    CultureInfo.CurrentUICulture,
                    LocalizationBS.XmlHelper_NodeNotFound,
                    xpathQuery,
                    GetPath(xmlNode)));
            }

            return xmlFoundNode;
        }

        /// <summary>
        /// Queries a single xmlnode and returns the <c>InnerText</c> of the node or 
        /// <c>defaultValue</c>, if node is not found
        /// </summary>
        /// <param name="xmlNode">Note to be queries</param>
        /// <param name="query">Used Query to xmlNode</param>
        /// <param name="defaultValue">Default value, of node not found</param>
        /// <returns>Found string or <c>defaultValue</c></returns>
        public static string QuerySingleXmlNodeText(XmlNode xmlNode, string query, string defaultValue)
        {
            var node = xmlNode.SelectSingleNode(query);
            if (node == null)
            {
                return defaultValue;
            }

            return node.InnerText;
        }

        /// <summary>
        /// Gets the parent elements of the xmlnode as a stack. 
        /// The element on top of the stack is the root element
        /// </summary>
        /// <param name="xmlNode">Node to be queried</param>
        /// <returns>Stack of xmlnodes beginning with the root element.</returns>
        public static Stack<XmlNode> GetParentElements(XmlNode xmlNode)
        {
            var result = new Stack<XmlNode>();
            
            var current = xmlNode;
            while (current != null)
            {
                result.Push(current);
                current = current.ParentNode;
            }

            return result;
        }

        /// <summary>
        /// Gets the path to the xmlnode from the root node. 
        /// The result looks like /node/other/xmlnode
        /// </summary>
        /// <param name="xmlNode">Xmlnode to be evaluated</param>
        /// <returns>Path to node</returns>
        public static string GetPath(XmlNode xmlNode)
        {
            var stack = GetParentElements(xmlNode);
            var result = new StringBuilder();

            while (stack.Count > 0)
            {
                result.AppendFormat("/{0}", stack.Pop().Name);
            }

            return result.ToString();
        }
    }
}
