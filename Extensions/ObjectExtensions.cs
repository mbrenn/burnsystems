using System.Collections.Generic;
using System.Reflection;
using BurnSystems.Interfaces;

namespace BurnSystems.Extensions
{
    /// <summary>
    /// This static class stores the extension methods for every object.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Gets all properties of an object, 
        /// </summary>
        /// <param name="item">Object to be converted to the property table</param>
        /// <returns>List of properties</returns>
        public static IList<Property> GetFieldValues(this object item)
        {
            List<Property> result = new List<Property>();

            var type = item.GetType();
            while (type != null)
            {
                foreach (var property in type.GetFields(
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                {
                    var value = property.GetValue(item);
                    string valueText;

                    if (value == null)
                    {
                        valueText = "null";
                    }
                    else
                    {
                        valueText = value.ToString();
                    }

                    result.Add(
                        new Property()
                        {
                            Name = property.Name,
                            Value = value,
                            ValueText = valueText
                        });
                }

                // Gets basetype
                type = type.BaseType;
            }            

            return result;
        }

        /// <summary>
        /// This helper class stores the property information
        /// </summary>
        public class Property : IParserObject
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
                        return this.Name;
                    case "Value":
                        return this.Value;
                    case "ValueText":
                        return this.ValueText;
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
                return string.Format(
                    "{0}: {1}",
                    this.Name,
                    this.ValueText);
            }
        };
    }
}
