namespace BurnSystems.Extensions
{
    /// <summary>
    /// This helper class stores the property information
    /// </summary>
    public record ObjectProperty
    {
        /// <summary>
        /// Gets or sets the name of the property
        /// </summary>
        public string? Name
        {
            get;
            init;
        }

        /// <summary>
        /// Gets or sets the value of the property
        /// </summary>
        public object? Value
        {
            get;
            init;
        }

        /// <summary>
        /// Gets or sets the value of the property
        /// </summary>
        public string? ValueText
        {
            get;
            init;
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
