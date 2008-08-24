//-----------------------------------------------------------------------
// <copyright file="Deserializer.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using BurnSystems.Test;
    using System.Reflection;
    using System.Threading;
    using BurnSystems.Collections;

    /// <summary>
    /// Deserializes a stream and returns an object. 
    /// </summary>
    public class Deserializer : SerializationBase
    {
        /// <summary>
        /// Stream, where the object will be stored
        /// </summary>
        private Stream stream;

        /// <summary>
        /// Creates new instance and sets stream
        /// </summary>
        /// <param name="stream">Stream with serialized objects</param>
        public Deserializer(Stream stream)
        {
            this.stream = stream;
        }

        /// <summary>
        /// Serializes the given object into the stream
        /// </summary>
        /// <returns>Returns deserialized object</returns>
        public object Deserialize()
        {
            var binaryReader = new BinaryReader(this.stream);

            // Reads the header
            binaryReader.CheckHeader();

            // Composes the new object
            var composer = new Composer(this, binaryReader);
            return composer.ReadObject();
        }

        /// <summary>
        /// Initializes the type information in typeentry 
        /// and registers the new created type
        /// </summary>
        /// <param name="typeEntry">Type-Entry to be created</param>
        public void RegisterType(TypeEntry typeEntry)
        {
            // Gets the type
            Type type = null;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = assembly.GetType(typeEntry.Name);
                if (type != null)
                {
                    break;
                }
            }

            Ensure.IsNotNull(type, typeEntry.Name);

            // Gets generic arguments
            if (typeEntry.GenericArguments.Count > 0)
            {
                var genericTypes = ListHelper.ConvertToArray(typeEntry.GenericArguments,
                    x => this.TypeContainer.FindType(x).Type);

                type = type.MakeGenericType(genericTypes);
            }

            // Sets the type
            typeEntry.Type = type;

            // Go through field infos and sets them            
            foreach (var field in typeEntry.Fields)
            {
                var thisType = type;

                while (thisType != null && field.FieldInfo == null)
                {
                    var fieldInfo = type.GetField(
                        field.Name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    field.FieldInfo = fieldInfo;

                    thisType = thisType.BaseType;
                }
            }

            this.TypeContainer.AddType(typeEntry);
        }
    }
}
