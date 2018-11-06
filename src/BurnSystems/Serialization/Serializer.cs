namespace BurnSystems.Serialization
{
    using System;
    using System.IO;
    using Test;

    /// <summary>
    /// Serializes an object into a stream
    /// </summary>
    [Obsolete("Do not use. Serializer is not aware of binary differences between x86 and x64")]
    public class Serializer : SerializationBase
    {
        /// <summary>
        /// Stream, where the object will be stored
        /// </summary>
        private readonly Stream _stream;

        /// <summary>
        /// The binarywriter to be used
        /// </summary>
        private BinaryWriter _writer;

        /// <summary>
        /// Initializes a new instance of the Serializer class.
        /// </summary>
        /// <param name="stream">Stream to be used</param>
        public Serializer(Stream stream)
        {
            Ensure.IsNotNull(stream);
            this._stream = stream;
        }

        /// <summary>
        /// Serializes the object into the stream
        /// </summary>
        /// <param name="value">Value to be serialized</param>
        public void Serialize(object value)
        {
            _writer = new BinaryWriter(_stream);
            var visitor = new Visitor(this, _writer);

            // Writes header
            _writer.WriteHeader();

            visitor.ParseObject(value);
        }

        /// <summary>
        /// Registers a type and returns the created entry. If this type is already registered, the
        /// registration data is returned without readding
        /// </summary>
        /// <param name="type">Type to be registered</param>
        /// <returns>Created entry</returns>
        public TypeEntry RegisterType(Type type)
        {
            var typeEntry = TypeContainer.FindType(type);

            if (typeEntry == null)
            {
                // Adds subgeneric
                if (type.IsGenericType)
                {
                    foreach (var genericType in type.GetGenericArguments())
                    {
                        RegisterType(genericType);
                    }
                }

                // Adds a new type
                typeEntry = TypeContainer.AddType(type);

                _writer.StartContainer(ContainerType.Type);
                _writer.WriteType(typeEntry);
            }

            return typeEntry;
        }
    }
}
