using System.Xml;

namespace BurnSystems.Interfaces
{
    /// <summary>
    /// This interface is implemented by all classes, whose
    /// state shall be read from an xml node or shall be written to 
    /// an xml node
    /// </summary>
    public interface IXmlNode
    {
        /// <summary>
        /// Reads properties and attributes from an xml node
        /// </summary>
        /// <param name="xmlNode">Xmlnode, which should be read.</param>
        void ReadFromXmlNode(XmlNode xmlNode);

        /// <summary>
        /// Stores the properties of the object into the xml node
        /// </summary>
        /// <param name="xmlNode">Xmlnode, where properties are stored</param>
        void WriteIntoXmlNode(XmlNode xmlNode);
    }
}
