namespace BurnSystems.Serialization
{
    using System;
    using System.Collections.Generic;
    using Test;

    /// <summary>
    /// This visitor visits an object and calls the necessary method in 
    /// Serializer and Writer to serialize the object into a stream.
    /// </summary>
    [Obsolete]
    public class Visitor
    {
        /// <summary>
        /// Serializer to be used
        /// </summary>
        private readonly Serializer _serializer;

        /// <summary>
        /// Binary writer to be used
        /// </summary>
        private readonly BinaryWriter _writer;

        /// <summary>
        /// Initializes a new instance of the Visitor class.
        /// </summary>
        /// <param name="serializer">Serializer to be used</param>
        /// <param name="writer">Writer to be used</param>
        public Visitor(Serializer serializer, BinaryWriter writer)
        {
            _serializer = serializer;
            _writer = writer;
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
                _writer.StartContainer(ContainerType.Data);
                _writer.StartDataContainer(DataType.Null);
                return;
            }

            // Gets type of object
            var type = value.GetType();

            long objectId = -1;

            // Write
            if (Helper.IsNativeType(type))
            {
                _writer.StartContainer(ContainerType.Data);
                _writer.StartDataContainer(DataType.Native);
                _writer.WriteNativeType(value);
            }
            else if (Helper.IsEnumeration(type))
            {
                ParseEnumObject(value);
            }
            else if (Helper.IsArray(type))
            {
                // Checks, if object is already in reference
                if (type.IsClass || type.IsArray)
                {
                    bool alreadyInserted;
                    objectId = _serializer.RegisterObject(value, out alreadyInserted);
                    if (alreadyInserted)
                    {
                        _writer.StartContainer(ContainerType.Reference);
                        _writer.WriteReference(objectId);
                        return;
                    }
                }

                ParseArrayObject(value, objectId);
            }
            else
            {
                // Checks, if object is already in reference
                if (type.IsClass || type.IsArray)
                {
                    bool alreadyInserted;
                    objectId = _serializer.RegisterObject(value, out alreadyInserted);
                    if (alreadyInserted)
                    {
                        _writer.StartContainer(ContainerType.Reference);
                        _writer.WriteReference(objectId);
                        return;
                    }
                }

                // Complex type
                ParseComplexObject(value, objectId);
            }
        }

        /// <summary>
        /// Parses an enumeration object
        /// </summary>
        /// <param name="value">Object to be parsed</param>
        private void ParseEnumObject(object value)
        {
            var type = value.GetType();

            var typeEntry = _serializer.RegisterType(type);

            _writer.StartContainer(ContainerType.Data);
            _writer.StartDataContainer(DataType.Enum);
            _writer.WriteEnumType(typeEntry.TypeId, value as Enum);
        }

        /// <summary>
        /// Parses a complex object
        /// </summary>
        /// <param name="value">Object to be parsed</param>
        /// <param name="objectId">Id of object</param>
        private void ParseComplexObject(object value, long objectId)
        {
            var type = value.GetType();

            var typeEntry = _serializer.RegisterType(type);

            // Get the properties of 'Type' class object.            
            _writer.StartContainer(ContainerType.Data);
            _writer.StartDataContainer(DataType.Complex);
            _writer.StartComplexType(typeEntry.TypeId, objectId, typeEntry.Fields.Count);

            foreach (var field in typeEntry.Fields) 
            {
                _writer.WritePropertyId(field.FieldId);
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

            var elementTypeEntry = _serializer.RegisterType(elementType);

            _writer.StartContainer(ContainerType.Data);
            _writer.StartDataContainer(DataType.Array);            

            // Got dimensions
            var dimensions = array.Rank;
            var dimensionList = new List<int>();
            for (var n = 0; n < dimensions; n++)
            {
                var length = array.GetLength(n);
                dimensionList.Add(length);
            }

            _writer.StartArrayType(
                elementTypeEntry.TypeId, 
                objectId, 
                dimensions, 
                dimensionList);

            // Go through all values
            int[] index = new int[dimensions];
            index.Initialize();

            var inloop = true;
            while (inloop)
            {
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

                if (inloop)
                {
                    ParseObject(array.GetValue(index));

                    // Increase index
                    index[0]++;
                }
            }
        }
    }
}
