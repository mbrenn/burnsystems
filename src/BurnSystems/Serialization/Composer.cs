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

using System.Diagnostics;
using BurnSystems.Logging;

namespace BurnSystems.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.Serialization;

    /// <summary>
    /// The composer class helps to recompose the object
    /// </summary>
    public class Composer
    {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(Composer));

        /// <summary>
        /// These translations are used to translate the
        /// read values into the target type. 
        /// The dictionary stores in the key the pair of source and target-Type
        /// and in the value the transformation from source to targetobject
        /// </summary>
        private readonly Dictionary<KeyValuePair<Type, Type>, Func<object, object?>> _translations =
            new Dictionary<KeyValuePair<Type, Type>, Func<object, object?>>();

        /// <summary>
        /// Initializes a new instance of the Composer class.
        /// </summary>
        /// <param name="deserializer">Used deserializer</param>
        /// <param name="binaryReader">Used binaray reader</param>
        public Composer(Deserializer deserializer, BinaryReader binaryReader)
        {
            Deserializer = deserializer;
            BinaryReader = binaryReader;

            AddDefaultTranslations();
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
        /// Adds a translation to the composer
        /// </summary>
        /// <typeparam name="TSource">Type of the source object</typeparam>
        /// <typeparam name="TTarget">Type of the target object</typeparam>
        /// <param name="translation">Translation function from source
        /// to target function</param>
        public void AddTranslation<TSource, TTarget>(Func<TSource, TTarget> translation)
        {
            if (translation == null) throw new ArgumentNullException(nameof(translation));

            _translations[new KeyValuePair<Type, Type>(typeof(TSource), typeof(TTarget))]
                = x => translation((TSource) x);

        }

        /// <summary>
        /// Reads the object from the binary reader
        /// </summary>
        /// <returns>Read object</returns>
        public object? ReadObject()
        {
            while (true)
            {
                var containerType = BinaryReader.ReadContainerType();

                switch (containerType)
                {
                    case ContainerType.Type:
                        ReadType();
                        break;
                    case ContainerType.Data:
                        return ReadData();
                    case ContainerType.Reference:
                        return ReadReference();
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
        public object? ReadData()
        {
            var dataType = BinaryReader.ReadDataType();
            object? result;

            switch (dataType)
            {
                case DataType.Null:
                    result = null;
                    break;
                case DataType.Native:
                    result = ReadNativeType();
                    break;
                case DataType.Array:
                    result = ReadArrayType();
                    break;
                case DataType.Complex:
                    result = ReadComplexType();
                    break;
                case DataType.Enum:
                    result = ReadEnumType();
                    break;
                default:
                    throw new InvalidOperationException(
                        LocalizationBS.BinaryWriter_UnknownDataType);
            }

            return result;
        }

        /// <summary>
        /// Reads a new datatype
        /// </summary>
        public void ReadType()
        {
            var typeEntry = BinaryReader.ReadTypeEntry();

            Deserializer.RegisterType(typeEntry);            
        }

        /// <summary>
        /// Reads the reference
        /// </summary>
        /// <returns>Read reference</returns>
        public object ReadReference()
        {
            var referenceHeader = BinaryReader.ReadReferenceHeader();

            return Deserializer.ObjectContainer.FindObjectById(referenceHeader.ObjectId);
        }

        /// <summary>
        /// Adds the default translations
        /// </summary>
        private void AddDefaultTranslations()
        {
            AddTranslation<long, int>(Convert.ToInt32);
            AddTranslation<int, long>(Convert.ToInt64);
            AddTranslation<float, double>(Convert.ToDouble);
            AddTranslation<double, float>(Convert.ToSingle);
        }

        /// <summary>
        /// Reads an enumeration
        /// </summary>
        /// <returns>Read enumeration</returns>
        private object ReadEnumType()
        {
            var typeId = BinaryReader.ReadInt64();
            var value = BinaryReader.ReadNativeObject();

            var typeEntry = Deserializer.TypeContainer.FindType(typeId);
            Debug.Assert(typeEntry != null);

            return Enum.ToObject(typeEntry!.Type, value);
        }

        /// <summary>
        /// Reads and returns a complex type
        /// </summary>
        /// <returns>Read Complex type</returns>
        private object ReadComplexType()
        {
            // Reads complex header
            var complexHeader = BinaryReader.ReadComplexHeader();

            var type = Deserializer.TypeContainer.FindType(complexHeader.TypeId);
            if ( type == null ) throw new InvalidOperationException("type is null");

            var value = FormatterServices.GetSafeUninitializedObject(type.Type);
            Deserializer.ObjectContainer.AddObject(complexHeader.ObjectId, value);

            for (var n = 0; n < complexHeader.FieldCount; n++)
            {
                var propertyId = BinaryReader.ReadInt32();

                var valueProperty = ReadObject();

                // Tries to get field
                var field = type.FindField(propertyId);
                if (field == null) throw new InvalidOperationException("field is null");

                if (field.FieldInfo != null)
                {
                    if (valueProperty != null &&
                        !field.FieldInfo.FieldType.IsInstanceOfType(valueProperty))
                    {
                        // Try to convert. 
                        var pair = new KeyValuePair<Type, Type>(
                            valueProperty.GetType(),
                            field.FieldInfo.FieldType);
                        if (_translations.TryGetValue(pair, out var translator))
                        {
                            field.FieldInfo.SetValue(
                                value,
                                translator(valueProperty));

                            var logMessage = string.Format(
                                CultureInfo.InvariantCulture,
                                LocalizationBS.Composer_WrongTypeTransformed,
                                valueProperty.GetType().FullName,
                                field.FieldInfo.FieldType.FullName);

                            Logger.Warn(logMessage);
                        }
                        else
                        {
                            var logMessage = string.Format(
                                CultureInfo.InvariantCulture,
                                LocalizationBS.Composer_WrongTypeFound,
                                valueProperty.GetType().FullName,
                                field.FieldInfo.FieldType.FullName);

                            Logger.Warn(logMessage);
                        }
                    }
                    else
                    {
                        field.FieldInfo.SetValue(value, valueProperty);
                    }
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
            var arrayHeader = BinaryReader.ReadArrayHeader();

            // Gets the list
            var dimensions = arrayHeader.DimensionCount;
            var dimensionList = new List<int>();

            for (var n = 0; n < arrayHeader.DimensionCount; n++)
            {
                var length = arrayHeader.Dimensions[n];
                dimensionList.Add(length);
            }

            // Creates the array
            var elementType = Deserializer.TypeContainer.FindType(
                arrayHeader.TypeId);
            var array = Array.CreateInstance(elementType.Type, dimensionList.ToArray());
            Deserializer.ObjectContainer.AddObject(arrayHeader.ObjectId, array);

            // Enumerates the array
            var index = new int[dimensions];
            index.Initialize();

            var inloop = true;
            while (inloop)
            {
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

                if (inloop)
                {
                    var value = ReadObject();
                    array.SetValue(value, index);

                    // Increase index
                    index[0]++;
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
            var value = BinaryReader.ReadNativeObject();
            return value;
        }
    }
}
