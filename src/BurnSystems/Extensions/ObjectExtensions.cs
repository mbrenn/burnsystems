using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

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
        public static IList<ObjectProperty> GetFieldValues(this object item)
        {
            var result = new List<ObjectProperty>();

            var type = item.GetType();
            while (type != null)
            {
                foreach (var property in type.GetFields(
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                {
                    result.Add(ConvertToProperty(item, property));
                }

                // Gets basetype
                type = type.BaseType;
            }            

            return result;
        }
        
        /// <summary>
        /// Gets all properties of an object, 
        /// </summary>
        /// <param name="item">Object to be converted to the property table</param>
        /// <returns>List of properties</returns>
        public static IList<ObjectProperty> GetPropertyValues(this object item)
        {
            var result = new List<ObjectProperty>();

            var type = item.GetType();
            while (type != null)
            {
                foreach (var property in
                    type.GetProperties(
                        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.CanRead)
                    .Where(x => !result.Any(y => x.Name == y.Name)))
                {
                    result.Add(ConvertToProperty(item, property));
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
            var values = GetFieldValues(value)
                    .Select(x => string.Format(
                            CultureInfo.InvariantCulture,
                            "{0}: {1}",
                            x.Name,
                            x.ValueText))
                    .ToArray();

            return string.Join(
                Environment.NewLine,
                values);
        }

        /// <summary>
        /// Converts an object to a string value
        /// </summary>
        /// <param name="value">Value to be converted to a string</param>
        /// <returns>String value</returns>
        public static string ConvertPropertiesToString(this object value)
        {
            var values = GetPropertyValues(value)
                    .Select(x => string.Format(
                            CultureInfo.InvariantCulture,
                            "{0}: {1}",
                            x.Name,
                            x.ValueText))
                    .ToArray();

            return string.Join(
                Environment.NewLine,
                values);
        }

        /// <summary>
        /// Converts the property or field of an item to a Property object
        /// </summary>
        /// <param name="item">Item to be looked up</param>
        /// <param name="field">Property or field to be looked up</param>
        /// <returns>Resulting property</returns>
        private static ObjectProperty ConvertToProperty(object item, FieldInfo field)
        {
            var value = field.GetValue(item);
            return ConvertToProperty(value, field.Name);
        }

        /// <summary>
        /// Converts the property or field of an item to a Property object
        /// </summary>
        /// <param name="item">Item to be looked up</param>
        /// <param name="property">Property or field to be looked up</param>
        /// <returns>Resulting property</returns>
        private static ObjectProperty ConvertToProperty(object item, PropertyInfo property)
        {
            var value = property.GetValue(item, null);
            return ConvertToProperty(value, property.Name);
        }

        /// <summary>
        /// Converts a value to a property object
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="name">Name of the property</param>
        /// <returns>Resulting property</returns>
        private static ObjectProperty ConvertToProperty(object value, string name)
        {
            string valueText;

            if (value == null)
            {
                valueText = "null";
            }
            else if (value is string)
            {
                valueText = value.ToString();
            }
            else if (value is IEnumerable valueAsEnumerable)
            {
                var builder = new StringBuilder();
                builder.Append('{');

                var komma = string.Empty;
                foreach (var subItem in valueAsEnumerable)
                {
                    builder.Append(komma);

                    if (subItem != null)
                    {
                        builder.Append(subItem);
                    }
                    else
                    {
                        builder.Append("null");
                    }

                    komma = ", ";
                }

                builder.Append('}');

                valueText = builder.ToString();
            }
            else
            {
                valueText = value.ToString();
            }

            return
                new ObjectProperty
                {
                    Name = name,
                    Value = value,
                    ValueText = valueText
                };
        }
        
        /// <summary>
        /// Converts an object to a specific type
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="type">Type being requested</param>
        /// <returns>Converted Object</returns>
        public static object ConvertTo(this object value, Type type)
        {
            if (type == typeof(string))
            {
                return value.ToString();
            }

            if (type == typeof(short))
            {
                return Convert.ToInt16(value);
            }

            if (type == typeof(int))
            {
                return Convert.ToInt32(value);
            }

            if (type == typeof(long))
            {
                return Convert.ToInt64(value);
            }

            if (type == typeof(float))
            {
                return Convert.ToSingle(value);
            }

            if (type == typeof(double))
            {
                return Convert.ToDouble(value);
            }

            if (type == typeof(decimal))
            {
                return Convert.ToDecimal(value);
            }

            if (type == typeof(DateTime))
            {
                return Convert.ToDateTime(value);
            }

            if (type == typeof(bool))
            {
                return Convert.ToBoolean(value);
            }

            if (type.IsEnum)
            {
                return Enum.Parse(type, value.ToString());
            }

            throw new InvalidOperationException(string.Format(LocalizationBS.Mapper_NotSupportedType, type));
        }
    }
}
