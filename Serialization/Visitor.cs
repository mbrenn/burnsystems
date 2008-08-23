//-----------------------------------------------------------------------
// <copyright file="Visitor.cs" company="Martin Brenn">
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
    using System.Reflection;
    using BurnSystems.Test;

    /// <summary>
    /// This visitor visits an object and calls the necessary method in 
    /// Serializer and Writer to serialize the object into a stream.
    /// </summary>
    public class Visitor
    {
        /// <summary>
        /// Serializer to be used
        /// </summary>
        private Serializer serializer;

        /// <summary>
        /// Binary writer to be used
        /// </summary>
        private BinaryWriter writer;

        /// <summary>
        /// Creates a new instance and sets serializer and writer
        /// </summary>
        /// <param name="serializer">Serializer to be used</param>
        /// <param name="writer">Writer to be used</param>
        public Visitor(Serializer serializer, BinaryWriter writer)
        {
            this.serializer = serializer;
            this.writer = writer;
        }

        /// <summary>
        /// Parses the object and calls the methods for serialization
        /// </summary>
        /// <param name="value">Object to be parsed</param>
        public void ParseObject(object value)
        {
            // Null object
            if (value == null)
            {
                writer.StartContainer(ContainerType.Data);
                writer.StartDataContainer(DataType.Null);
                return;
            }

            // Gets type of object
            var type = value.GetType();

            long objectId = -1;
            // Checks, if object is already in reference
            if (type.IsClass || type.IsArray)
            {
                bool alreadyInserted;
                objectId = this.serializer.RegisterObject(value, out alreadyInserted);
                if (alreadyInserted)
                {
                    writer.StartContainer(ContainerType.Reference);
                    writer.WriteReference(objectId);
                    return;
                }
            }

            // Write
            if (Helper.IsNativeType(type))
            {
                writer.StartContainer(ContainerType.Data);
                writer.StartDataContainer(DataType.Native);
                writer.WriteNativeType(value);
            }
            else if (Helper.IsEnumeration(type))
            {
                this.ParseEnumObject(value);
            }
            else if (Helper.IsArray(type))
            {
                this.ParseArrayObject(value, objectId);
            }
            else
            {
                // Complex type
                this.ParseComplexObject(value, objectId);
            }
        }

        /// <summary>
        /// Parses an enumeration object
        /// </summary>
        /// <param name="value">Object to be parsed</param>
        private void ParseEnumObject(object value)
        {
            var type = value.GetType();

            var typeEntry = this.serializer.RegisterType(type);

            writer.StartContainer(ContainerType.Data);
            writer.StartDataContainer(DataType.Enum);
            writer.WriteEnumType(typeEntry.TypeId, value as Enum);
        }

        /// <summary>
        /// Parses a complex object
        /// </summary>
        /// <param name="value">Object to be parsed</param>
        /// <param name="objectId">Id of object</param>
        private void ParseComplexObject(object value, long objectId)
        {
            var type = value.GetType();

            var typeEntry = this.serializer.RegisterType(type);
            
            // Get the properties of 'Type' class object.            
            writer.StartContainer(ContainerType.Data);
            writer.StartDataContainer(DataType.Complex);
            writer.StartComplexType(typeEntry.TypeId, objectId, typeEntry.Fields.Count);

            foreach (var field in typeEntry.Fields) 
            {
                writer.WritePropertyId(field.FieldId);
                ParseObject(field.FieldInfo.GetValue(value));
            }
        }

        /// <summary>
        /// Parses an array object
        /// </summary>
        /// <param name="value">Object to be parsed</param>
        /// <param name="objectId">Id of object</param>
        private void ParseArrayObject(object value, long objectId)
        {
            Array array = value as Array;
            var arrayType = array.GetType();
            var elementType = arrayType.GetElementType();
            Ensure.IsNotNull(elementType);

            var elementTypeEntry = this.serializer.RegisterType(elementType);
            
            writer.StartContainer(ContainerType.Data);
            writer.StartDataContainer(DataType.Array);            

            // Got dimensions
            var dimensions = array.Rank;
            var dimensionList = new List<int>();
            for (var n = 0; n < dimensions; n++)
            {
                var length = array.GetLength(n);
                dimensionList.Add(length);
            }

            writer.StartArrayType(elementTypeEntry.TypeId, objectId, dimensions, dimensionList);

            // Go through all values
            int[] index = new int[dimensions];
            index.Initialize();

            var inloop = true;
            while (inloop)
            {
                this.ParseObject(array.GetValue(index));

                // Increase index
                index[0]++;

                // Check indizes
                for (var n = 0; n < dimensions; n++)
                {
                    if (index[n] >= array.GetLength(n))
                    {
                        index[n] = 0;

                        if (n == dimensions - 1)
                        {
                            // Everything is finished
                            inloop = false;
                        }
                        else
                        {
                            // Increase next index
                            index[n + 1]++;
                        }
                    }
                }
            }
        }
    }
}
