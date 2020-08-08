using System.Collections.Generic;
using BurnSystems.Collections;

namespace BurnSystems.Net
{
    /// <summary>
    /// Contains one part of the multipartform data. 
    /// </summary>
    public class MultipartFormDataPart
    {
        /// <summary>
        /// Gets the elements of the content-disposition header
        /// </summary>
        public NiceDictionary<string, string> ContentDisposition { get; } = new NiceDictionary<string, string>();

        /// <summary>
        /// Gets a list of headers
        /// </summary>
        public List<KeyValuePair<string, string>> Headers { get; } = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// Gets or sets the content of this part
        /// </summary>
        public byte[]? Content
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a specific header
        /// </summary>
        /// <param name="headerName">Name of requested header</param>
        /// <returns>Found header or null</returns>
        public string this[string headerName]
        {
            get
            {
                var pair =
                    Headers.Find(x => x.Key == headerName);
                return pair.Value;
            }
        }
    }
}
