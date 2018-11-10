namespace BurnSystems.Net.Json
{
    using System;

    /// <summary>
    /// Stores a boolean value
    /// </summary>
    [Obsolete("Use System.Web.Script.Serialization from System.Web.Extensions.dll")]
    public class JsonBoolean : IJsonObject
    {
        /// <summary>
        /// Stores the value
        /// </summary>
        private readonly bool _value;

        /// <summary>
        /// Initializes a new instance of the JsonBoolean class.
        /// </summary>
        /// <param name="value">Value to be set</param>
        public JsonBoolean(bool value)
        {
            _value = value;
        }

        /// <summary>
        /// Converts the boolean to a json string
        /// </summary>
        /// <returns>This object as a json string</returns>
        public override string ToString()
        {
            return _value ? "true" : "false";
        }
    }
}
