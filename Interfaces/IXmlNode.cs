//-----------------------------------------------------------------------
// <copyright file="IXmlNode.cs" company="Martin Brenn">
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

namespace BurnSystems.Interfaces
{
    /// <summary>
    /// Dieses Interface muss von allen Objekten implementiert werden,
    /// die sich irgendwie in einem Xml-Knoten speichern künnen.
    /// </summary>
    public interface IXmlNode
    {
        /// <summary>
        /// Liest die Eigenschaften und Attribute aus einem Xml-Knoten
        /// </summary>
        /// <param name="xmlNode">Xml-Knoten aus dem gelesen werden soll.</param>
        void ReadFromXmlNode(XmlNode xmlNode);

        /// <summary>
        /// Speichert die Eigenschaften des Objektes in dem übergebenen Xml-Knoten
        /// </summary>
        /// <param name="xmlNode">Xml-Knoten in dem die aktuellen 
        /// Objekte gespeichert werden sollen. </param>
        void WriteIntoXmlNode(XmlNode xmlNode);
    }
}
