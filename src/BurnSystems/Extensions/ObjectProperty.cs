namespace BurnSystems.Extensions
{
    /// <summary>
    /// This helper class stores the property information
    /// </summary>
    public class ObjectProperty
    {
        /// <summary>
        /// Gets or sets the name of the property
        /// </summary>
        public string? Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value of the property
        /// </summary>
        public object? Value
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value of the property
        /// </summary>
        public string? ValueText
        {
            get;
            set;
        }

        /// <summary>
        /// This function returns a specific property, which is accessed by name
        /// </summary>
        /// <param name="name">Name of requested property</param>
        /// <returns>Property behind this object</returns>
        public object? GetProperty(string name)
        {
            return name switch
            {
                "Name" => Name,
                "Value" => Value,
                "ValueText" => ValueText,
                _ => null
            };
        }

        /// <summary>
        /// Converts to string
        /// </summary>
        /// <returns>Value of property</returns>
        public override string ToString()
        {
            return $"{Name}: {ValueText}";
        }
    }
}
