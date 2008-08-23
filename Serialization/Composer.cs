//-----------------------------------------------------------------------
// <copyright file="Composer.cs" company="Martin Brenn">
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
    using BurnSystems.Test;
    using System.Runtime.Serialization;

    /// <summary>
    /// The composer class helps to recompose the object
    /// </summary>
    public class Composer
    {        
        /// <summary>
        /// Creates new instance
        /// </summary>
        /// <param name="deserializer">Used deserializer</param>
        /// <param name="binaryReader">Used binaray reader</param>
        public Composer(Deserializer deserializer, BinaryReader binaryReader)
        {
            this.Deserializer = deserializer;
            this.BinaryReader = binaryReader;
        }

        /// <summary>
        /// Gets or sets the deserializer
        /// </summary>
        public Deserializer Deserializer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the binary reader
        /// </summary>
        public BinaryReader BinaryReader
        {
            get;
            set;
        }

        /// <summary>
        /// Reads the object from the binary reader
        /// </summary>
        /// <returns>Read object</returns>
        public object ReadObject()
        {
            while (true)
            {
                var containerType = this.BinaryReader.ReadContainerType();

                switch (containerType)
                {
                    case ContainerType.Type:
                        this.ReadType();
                        break;
                    case ContainerType.Data:
                        return this.ReadData();
                    case ContainerType.Reference:
                        return this.ReadReference();
                    default:
                        throw new InvalidOperationException(
                            LocalizationBS.BinaryWriter_UnknownContainerType);
                }
            }
        }

        /// <summary>
        /// Reads a dataobject
        /// </summary>
        /// <returns>Read object</returns>
        public object ReadData()
        {
            var dataType = this.BinaryReader.ReadDataType();

            switch (dataType)
            {
                case DataType.Null:
                    return null;
                case DataType.Native:
                    return this.ReadNativeType();
                case DataType.Array:
                    return this.ReadArrayType();
                case DataType.Complex:
                    return this.ReadComplexType();
                case DataType.Enum:
                    return this.ReadEnumType();
                default:
                    throw new InvalidOperationException(
                        LocalizationBS.BinaryWriter_UnknownDataType);
            }
        }

        /// <summary>
        /// Reads a new datatype
        /// </summary>
        public void ReadType()
        {
            var typeEntry = this.BinaryReader.ReadTypeEntry();

            this.Deserializer.RegisterType(typeEntry);            
        }

        /// <summary>
        /// Reads the reference
        /// </summary>
        /// <returns>Read reference</returns>
        public object ReadReference()
        {
            var referenceHeader = this.BinaryReader.ReadReferenceHeader();

            return this.Deserializer.ObjectContainer.FindObjectById(referenceHeader.ObjectId);
        }

        /// <summary>
        /// Reads an enumeration
        /// </summary>
        /// <returns>Read enumeration</returns>
        private object ReadEnumType()
        {
            var typeId = this.BinaryReader.ReadInt64();
            var value = this.BinaryReader.ReadNativeObject();

            var typeEntry = this.Deserializer.TypeContainer.FindType(typeId);
            Ensure.IsNotNull(typeEntry);

            return Enum.ToObject(typeEntry.Type, value);
        }

        /// <summary>
        /// Reads and returns a complex type
        /// </summary>
        /// <returns>Read Complex type</returns>
        private object ReadComplexType()
        {
            // Reads complex header
            var complexHeader = this.BinaryReader.ReadComplexHeader();

            var type = this.Deserializer.TypeContainer.FindType(
                complexHeader.TypeId);
            Ensure.IsNotNull(type);

            var value = FormatterServices.GetSafeUninitializedObject(type.Type);
            this.Deserializer.ObjectContainer.AddObject(complexHeader.ObjectId, value);

            for (var n = 0; n < complexHeader.FieldCount; n++)
            {
                var propertyId = this.BinaryReader.ReadInt32();

                var valueProperty = this.ReadObject();

                // Tries to get field
                var field = type.FindField(propertyId);
                Ensure.IsNotNull(field);

                if (field.FieldInfo != null)
                {
                    field.FieldInfo.SetValue(value, valueProperty);
                }
            }

            return value;
        }

        /// <summary>
        /// Reads and returns an array type
        /// </summary>
        /// <returns>Read Array Type</returns>
        private object ReadArrayType()
        {
            var arrayHeader = this.BinaryReader.ReadArrayHeader();

            // Gets the list
            var dimensions = arrayHeader.DimensionCount;
            var dimensionList = new List<int>();

            for (var n = 0; n < arrayHeader.DimensionCount; n++)
            {
                var length = arrayHeader.Dimensions[n];
                dimensionList.Add(length);
            }

            // Creates the array
            var elementType = this.Deserializer.TypeContainer.FindType(
                arrayHeader.TypeId);
            var array = Array.CreateInstance(elementType.Type, dimensionList.ToArray());
            this.Deserializer.ObjectContainer.AddObject(arrayHeader.ObjectId, array);

            // Enumerates the array
            int[] index = new int[dimensions];
            index.Initialize();

            var inloop = true;
            while (inloop)
            {
                var value = this.ReadObject();
                array.SetValue(value, index);

                // Increase index
                index[0]++;

                // Check indizes
                for (var n = 0; n < dimensions; n++)
                {
                    if (index[n] >= dimensionList[n])
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

            return array;
        }

        /// <summary>
        /// Reads native type
        /// </summary>
        /// <returns>Read native type</returns>
        private object ReadNativeType()
        {
            var value = this.BinaryReader.ReadNativeObject();
            return value;
        }
    }
}
