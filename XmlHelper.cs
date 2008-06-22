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

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Globalization;

namespace BurnSystems
{
    /// <summary>
    /// Eine Hilfsklasse, die den Zugriff auf die Xml-Klassen verbessert.
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// Gibt ein Xml-Attribut zurück. Wird der Xml-Knoten nicht gefunden,
        /// so wird der Standardwert zur+ckgegeben. 
        /// </summary>
        /// <param name="xmlNode">Xml-Knoten</param>
        /// <param name="strAttributeName">Attributname</param>
        /// <param name="strDefaultvalue">Standardwert, der zurückgegeben wird, 
        /// wenn der Knoten nicht gefunden wird. </param>
        /// <returns></returns>
        public static String QueryXmlAttributeText(XmlNode xmlNode, String strAttributeName, String strDefaultvalue)
        {
            if (xmlNode == null)
            {
                throw new ArgumentNullException("xmlNode");
            }
            XmlAttribute xmlAttribute = xmlNode.Attributes[strAttributeName];

            if (xmlAttribute == null)
            {
                return strDefaultvalue;
            }

            return xmlAttribute.InnerText;
        }

        /// <summary>
        /// Gibt ein Xml-Attribut zurück. Wird der Xml-Knoten nicht gefunden,
        /// so wird eine Ausnahme ausgelöst.
        /// </summary>
        /// <param name="xmlNode">Xml-Knoten</param>
        /// <param name="strAttributeName">Attributname</param>
        /// <returns></returns>
        public static XmlAttribute QueryXmlAttribute(XmlNode xmlNode, String strAttributeName)
        {
            if (xmlNode == null)
            {
                throw new ArgumentNullException("xmlNode");
            }
            if (strAttributeName == null)
            {
                throw new ArgumentNullException("strAttributeName");
            }
            if (xmlNode.Attributes == null)
            {
                throw new ArgumentNullException("xmlNode", "xmlNode.Attributs == null");
            }

            XmlAttribute xmlAttribute = xmlNode.Attributes[strAttributeName];

            if (xmlAttribute == null)
            {
                throw new InvalidOperationException(String.Format(
                    CultureInfo.CurrentUICulture,
                    LocalizationBS.XmlHelper_AttributeNotFound, 
                    strAttributeName));
            }

            return xmlAttribute;
        }

        /// <summary>
        /// Fragt einen Xml-Knoten per XPath ab. 
        /// </summary>
        /// <param name="xmlNode">Xml-Knoten</param>
        /// <param name="strQuery">XPath-Query</param>
        /// <returns>Gefundener Xml-Knoten</returns>
        public static XmlNode QuerySingleXmlNode(XmlNode xmlNode, String strQuery)
        {
            if (xmlNode == null)
            {
                throw new ArgumentNullException("xmlNode");
            }
            XmlNode xmlFoundNode = xmlNode.SelectSingleNode(strQuery);
            if (xmlFoundNode == null)
            {
                throw new InvalidOperationException(String.Format(
                    CultureInfo.CurrentUICulture,
                    LocalizationBS.XmlHelper_NodeNotFound, 
                    strQuery));
            }
            return xmlFoundNode;
        }
    }
}
