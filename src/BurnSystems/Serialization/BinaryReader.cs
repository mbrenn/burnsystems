using System;
using System.Globalization;
using System.IO;
using System.Text;
using BurnSystems.Test;

namespace BurnSystems.Serialization
{
    /// <summary>
    /// This class is an implementation of a binary reader for serialization.
    /// </summary>
    public class BinaryReader
    {
        /// <summary>
        /// Stream to be used for binary
        /// </summary>
        private readonly Stream _stream;

        /// <summary>
        /// Initializes a new instance of the BinaryReader class.
        /// </summary>
        /// <param name="stream">Stream to be used for serialization</param>
        public BinaryReader(Stream stream)
        {
            _stream = stream;
        }

        /// <summary>
        /// Converts an array of bytes to an object
        /// </summary>
        /// <param name="value">Array of bytes to be converted</param>
        /// <param name="type">Type of target object</param>
        /// <returns>Converted object</returns>
        public static object ConvertObject(byte[] value, Type type)
        {
            if (type == typeof(bool))
            {
                return value[0] == 1;
            }

            if (type == typeof(byte))
            {
                return value[0];
            }

            if (type == typeof(short))
            {
                return BitConverter.ToInt16(value, 0);
            }

            if (type == typeof(int))
            {
                return BitConverter.ToInt32(value, 0);
            }

            if (type == typeof(long))
            {
                return BitConverter.ToInt64(value, 0);
            }

            if (type == typeof(ushort))
            {
                return BitConverter.ToUInt16(value, 0);
            }

            if (type == typeof(uint))
            {
                return BitConverter.ToUInt32(value, 0);
            }

            if (type == typeof(ulong))
            {
                return BitConverter.ToUInt64(value, 0);
            }

            if (type == typeof(float))
            {
                return BitConverter.ToSingle(value, 0);
            }

            if (type == typeof(double))
            {
                return BitConverter.ToDouble(value, 0);
            }

            if (type == typeof(char))
            {
                return BitConverter.ToChar(value, 0);
            }

            if (type == typeof(string))
            {
                return Encoding.UTF8.GetString(value);
            }

            throw new InvalidOperationException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    LocalizationBS.BinaryReader_ObjectNotConverted,
                    value.GetType()));
        }

        /// <summary>
        /// Reads and checks the header in stream and throws an exception, if header cann't be read.
        /// </summary>
        public void CheckHeader()
        {
            // Reads string
            var headerBytes = Encoding.UTF8.GetBytes(Helper.StreamHeaderText);
            var bytes = new byte[headerBytes.Length];
            _stream.Read(bytes, 0, bytes.Length);
            if (Encoding.UTF8.GetString(bytes) != Helper.StreamHeaderText)
            {
                throw new InvalidOperationException(LocalizationBS.BinaryReader_InvalidHeader);
            }

            // Reads version
            var versionBytes = new byte[4];
            _stream.Read(versionBytes, 0, versionBytes.Length);
            if (versionBytes[0] != 0x01 || versionBytes[1] != 0x00 || versionBytes[2] != 0x00 || versionBytes[3] != 0x00)
            {
                throw new InvalidOperationException(LocalizationBS.BinaryReader_InvalidHeader);
            }
        }

        /// <summary>
        /// Reads the containertype
        /// </summary>
        /// <returns>Read containertype</returns>
        public ContainerType ReadContainerType()
        {
            var containerTypeValue = ReadByte();
            return containerTypeValue switch
            {
                0x01 => ContainerType.Type,
                0x02 => ContainerType.Data,
                0x03 => ContainerType.Reference,
                _ => throw new InvalidCastException(LocalizationBS.BinaryWriter_UnknownContainerType)
            };
        }

        /// <summary>
        /// Reads the datatype
        /// </summary>
        /// <returns>Read datatype</returns>
        public DataType ReadDataType()
        {
            var dataTypeValue = ReadByte();
            return dataTypeValue switch
            {
                0x01 => DataType.Null,
                0x02 => DataType.Native,
                0x03 => DataType.Array,
                0x04 => DataType.Complex,
                0x05 => DataType.Enum,
                _ => throw new InvalidCastException(LocalizationBS.BinaryWriter_UnknownDataType)
            };
        }

        /// <summary>
        /// Reads a native object and returns it.
        /// </summary>
        /// <returns>Type, which has read</returns>
        public object ReadNativeObject()
        {
            var nativeTypeId = ReadInt32();
            var nativeType = Helper.ConvertNumberToNativeType(nativeTypeId);
            var length = ReadInt32();
            Ensure.IsGreaterOrEqual(length, 0);

            var bytes = new byte[length];
            _stream.Read(bytes, 0, length);
            return ConvertObject(bytes, nativeType);
        }

        /// <summary>
        /// Reads a type from stream
        /// </summary>
        /// <returns>Type, which has been read</returns>
        public TypeEntry ReadTypeEntry()
        {
            // Reads information
            var typeId = ReadInt64();
            var typeNameLength = ReadInt32();
            Ensure.IsGreater(typeNameLength, 0);
            Ensure.IsSmaller(typeNameLength, 10000);

            var typeNameBytes = new byte[typeNameLength];
            _stream.Read(typeNameBytes, 0, typeNameLength);
            
            // Create type entry
            var typeEntry = new TypeEntry
            {
                TypeId = typeId, 
                Name = Encoding.UTF8.GetString(typeNameBytes)
            };

            // Read generic arguments
            var genericArgumentCount = ReadInt32();
            for (var n = 0; n < genericArgumentCount; n++)
            {
                var genericTypeId = ReadInt64();
                typeEntry.GenericArguments.Add(genericTypeId);
            }

            // Add fields
            var fieldCount = ReadInt32();
            Ensure.IsGreaterOrEqual(fieldCount, 0);

            // Read properties
            for (var n = 0; n < fieldCount; n++)
            {
                var fieldId = ReadInt32();
                var fieldNameLength = ReadInt32();
                Ensure.IsGreater(fieldNameLength, 0);
                Ensure.IsSmaller(fieldNameLength, 10000);

                var fieldNameBytes = new byte[fieldNameLength];
                _stream.Read(fieldNameBytes, 0, fieldNameLength);

                var fieldEntry = new FieldEntry
                {
                    FieldId = fieldId, 
                    Name = Encoding.UTF8.GetString(fieldNameBytes)
                };

                typeEntry.Fields.Add(fieldEntry);
            }

            return typeEntry;
        }

        /// <summary>
        /// Reads the array header from stream
        /// </summary>
        /// <returns>Read array header</returns>
        public ArrayHeader ReadArrayHeader()
        {
            var arrayHeader = new ArrayHeader
            {
                TypeId = ReadInt64(), 
                ObjectId = ReadInt64(), 
                DimensionCount = ReadInt32()
            };

            Ensure.IsGreaterOrEqual(arrayHeader.DimensionCount, 0);

            for (var n = 0; n < arrayHeader.DimensionCount; n++)
            {
                arrayHeader.Dimensions.Add(ReadInt32());
            }

            return arrayHeader;
        }

        /// <summary>
        /// Reads a complex header from stream
        /// </summary>
        /// <returns>Read complex header</returns>
        public ComplexHeader ReadComplexHeader()
        {
            var complexHeader = new ComplexHeader
            {
                TypeId = ReadInt64(),
                ObjectId = ReadInt64(),
                FieldCount = ReadInt32()
            };

            return complexHeader;
        }

        /// <summary>
        /// Reads the referenceheader from stream
        /// </summary>
        /// <returns>Read reference header</returns>
        public ReferenceHeader ReadReferenceHeader()
        {
            var referenceHeader = new ReferenceHeader
            {
                ObjectId = ReadInt64()
            };

            return referenceHeader;
        }

        /// <summary>
        /// Reads a 32 bit integer from stream
        /// </summary>
        /// <returns>Read integer</returns>
        public byte ReadByte()
        {
            return (byte)_stream.ReadByte();
        }

        /// <summary>
        /// Reads a 32 bit integer from stream
        /// </summary>
        /// <returns>Read integer</returns>
        public int ReadInt32()
        {
            var bytes = new byte[4];
            _stream.Read(bytes, 0, 4);

            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Reads a 64 bit integer from stream
        /// </summary>
        /// <returns>Read integer</returns>
        public long ReadInt64()
        {
            var bytes = new byte[8];
            _stream.Read(bytes, 0, 8);

            return BitConverter.ToInt64(bytes, 0);
        }
    }
}
