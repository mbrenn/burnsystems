namespace BurnSystems.Serialization
{
    /// <summary>
    /// The base class for serialization and deserialization.
    /// </summary>
    public class SerializationBase
    {
        /// <summary>
        /// Container, storing the objects
        /// </summary>
        private ObjectContainer objectContainer = new ObjectContainer();

        /// <summary>
        /// Container, storing the types
        /// </summary>
        private TypeContainer typeContainer = new TypeContainer();

        /// <summary>
        /// Gets the typecontainer
        /// </summary>
        public TypeContainer TypeContainer
        {
            get { return typeContainer; }
        }

        /// <summary>
        /// Gets the container, storing the object
        /// </summary>
        internal ObjectContainer ObjectContainer
        {
            get { return objectContainer; }
        }

        /// <summary>
        /// Registers a new object
        /// </summary>
        /// <param name="value">Value to be registered</param>
        /// <param name="alreadyInserted">Value, indicating whether this object has alredy been inserted.</param>
        /// <returns>Id of registered object</returns>
        public long RegisterObject(object value, out bool alreadyInserted)
        {
            return objectContainer.AddObject(value, out alreadyInserted);
        }
    }
}
