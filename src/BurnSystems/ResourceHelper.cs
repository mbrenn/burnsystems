using System;
using System.IO;
using System.Xml.Linq;

namespace BurnSystems
{
    public static class ResourceHelper
    {
        /// <summary>
        /// Loads the string from assembly
        /// </summary>
        /// <param name="typeInAssembly">Type of the assembly from which the data is loaded</param>
        /// <param name="resourcePath">Path to the resources</param>
        /// <returns></returns>
        public static string LoadStringFromAssembly(Type typeInAssembly, string resourcePath)
        {
            using var stream =
                typeInAssembly.Assembly.GetManifestResourceStream(resourcePath);
            using var reader = new StreamReader(stream ?? throw new InvalidOperationException("Stream is empty"));
            return reader.ReadToEnd();
        }

        public static string LoadStringFromStream(Stream stream)
        {
            using var reader = new StreamReader(stream ?? throw new InvalidOperationException("Stream is empty"));
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Loads an xml document from the assembly
        /// </summary>
        /// <param name="typeInAssembly">Type containing the assembly in which the resource is located</param>
        /// <param name="resourcePath">Path to the resource</param>
        /// <returns>Document to be located</returns>
        public static XDocument LoadXmlFromAssembly(Type typeInAssembly, string resourcePath)
        {
            using var stream =
                typeInAssembly.Assembly.GetManifestResourceStream(resourcePath);

            if (stream == null)
            {
                throw new InvalidOperationException($"The Manifest Resource Stream {resourcePath} was not found");
            }

            return XDocument.Load(stream);
        }
    }
}