namespace BurnSystems.Net.Json
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Stores a string
    /// </summary>
    [Obsolete("Use System.Web.Script.Serialization from System.Web.Extensions.dll")]
    public class JsonString : IJsonObject
    {
        /// <summary>
        /// Value of the instance
        /// </summary>
        private readonly string _value;

        /// <summary>
        /// Initializes a new instance of the JsonString class.
        /// </summary>
        /// <param name="value">Value to be set</param>
        public JsonString(string value)
        {
            this._value = value;
        }

        /// <summary>
        /// Converts the string to a json string
        /// </summary>
        /// <returns>This object as a json string</returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "\"{0}\"",
                _value
                    .Replace("\\", "\\\\")
                    .Replace("\"", "\\\"")
                    .Replace("\t", "\\t")
                    .Replace("\r", "\\r")
                    .Replace("\n", "\\n"));
        }
    }
}
