namespace BurnSystems.Net.Json
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Stores a number
    /// </summary>
    [Obsolete("Use System.Web.Script.Serialization from System.Web.Extensions.dll")]
    public class JsonNumber : IJsonObject
    {
        /// <summary>
        /// Value of the instance
        /// </summary>
        private readonly double _value;

        /// <summary>
        /// Initializes a new instance of the JsonNumber class.
        /// </summary>
        /// <param name="value">Value to be set</param>
        public JsonNumber(double value)
        {
            this._value = value;
        }

        /// <summary>
        /// Converts the number to a json string
        /// </summary>
        /// <returns>This object as a json string</returns>
        public override string ToString()
        {
            return _value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
