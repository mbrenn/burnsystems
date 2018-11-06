namespace BurnSystems.Serialization
{
    using System.Reflection;

    /// <summary>
    /// This class defines the entry for the different fields
    /// </summary>
    public class FieldEntry
    {
        /// <summary>
        /// Gets or sets the id of the field
        /// </summary>
        public int FieldId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the field
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the methodinfo
        /// </summary>
        public FieldInfo FieldInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Converts this instance to string
        /// </summary>
        /// <returns>Name of field</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
