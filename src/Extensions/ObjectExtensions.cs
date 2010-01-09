//-----------------------------------------------------------------------
// <copyright file="ObjectExtensions.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Extensions
{
    using System.Linq;
    using System.Collections.Generic;
    using System.Reflection;
    using BurnSystems.Interfaces;
    using System.Text;
    using System;
    using System.Collections;

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
                    else if (value is IEnumerable)
                    {
                        var enumeration = value as IEnumerable;
                        var builder = new StringBuilder();
                        builder.Append('{');

                        var komma = string.Empty;
                        foreach (var subItem in enumeration)
                        {
                            builder.Append(subItem.ToString());
                            builder.Append(komma);
                            komma = ", ";
                        }

                        builder.Append('}');

                        valueText = builder.ToString();
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
        /// Converts an object to a string value
        /// </summary>
        /// <param name="value">Value to be converted to a string</param>
        /// <returns>String value</returns>
        public static string ConvertToString(this object value)
        {
            return string.Join(
                Environment.NewLine,
                GetFieldValues(value)
                    .Select(x => string.Format(
                            "{0}: {1}", 
                            x.Name, 
                            x.ValueText))
                    .ToArray());
                
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
        }
    }
}
