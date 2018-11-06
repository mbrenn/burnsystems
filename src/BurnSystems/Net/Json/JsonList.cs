namespace BurnSystems.Net.Json
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    
    /// <summary>
    /// Stores a list of json entries
    /// </summary>
    [Obsolete("Use System.Web.Script.Serialization from System.Web.Extensions.dll")]
    public class JsonList : IJsonObject
    {
        /// <summary>
        /// Stores the list of json objects
        /// </summary>
        private readonly List<IJsonObject> _list =
            new List<IJsonObject>();

        /// <summary>
        /// Initializes a new instance of the JsonList class.
        /// </summary>
        public JsonList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the JsonList class.
        /// </summary>
        /// <param name="list">List of items to be added</param>
        public JsonList(IEnumerable list)
        {
            foreach (var value in list)
            {
                List.Add(
                    JsonObject.ConvertObject(value));
            }
        }

        /// <summary>
        /// Gets a list of json objects
        /// </summary>
        public List<IJsonObject> List => _list;

        /// <summary>
        /// Converts the list to a json string
        /// </summary>
        /// <returns>This object as a json string</returns>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append('[');

            var komma = string.Empty;

            foreach (var item in List)
            {
                stringBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "{0}{1}",
                    komma, 
                    item.ToString());
                komma = ",";
            }

            stringBuilder.Append(']');

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Adds an object to json list
        /// </summary>
        /// <param name="value">Value to be added</param>
        public void Add(object value)
        {
            List.Add(
                JsonObject.ConvertObject(value));
        }
    }
}
