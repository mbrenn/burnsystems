namespace BurnSystems.Extensions
{
    using System.Collections.Generic;

    /// <summary>
    /// This helper class stores the property information
    /// </summary>
    public class ObjectProperty
    {
        /// <summary>
        /// Gets or sets the name of the property
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value of the property
        /// </summary>
        public object Value
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value of the property
        /// </summary>
        public string ValueText
        {
            get;
            set;
        }

        /// <summary>
        /// This function returns a specific property, which is accessed by name
        /// </summary>
        /// <param name="name">Name of requested property</param>
        /// <returns>Property behind this object</returns>
        public object GetProperty(string name)
        {
            switch (name)
            {
                case "Name":
                    return Name;
                case "Value":
                    return Value;
                case "ValueText":
                    return ValueText;
                default:
                    return null;
            }
        }

        /// <summary>
        /// This function has to execute a function and to return an object
        /// </summary>
        /// <param name="functionName">Name of function</param>
        /// <param name="parameters">Parameters for the function</param>
        /// <returns>Return of function</returns>
        public object ExecuteFunction(string functionName, IList<object> parameters)
        {
            return null;
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
