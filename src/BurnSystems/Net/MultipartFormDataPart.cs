namespace BurnSystems.Net
{
    using System.Collections.Generic;
    using Collections;

    /// <summary>
    /// Contains one part of the multipartform data. 
    /// </summary>
    public class MultipartFormDataPart
    {
        /// <summary>
        /// Stores the elements of the content-disposition header 
        /// </summary>
        private NiceDictionary<string, string> contentDisposition =
            new NiceDictionary<string, string>();

        /// <summary>
        /// Stores a list of all header
        /// </summary>
        private List<Pair<string, string>> headers = new List<Pair<string, string>>();

        /// <summary>
        /// Initializes a new instance of the MultipartFormDataPart class.
        /// </summary>
        public MultipartFormDataPart()
        {
        }

        /// <summary>
        /// Gets the elements of the content-disposition header
        /// </summary>
        public NiceDictionary<string, string> ContentDisposition => contentDisposition;

        /// <summary>
        /// Gets a list of headers
        /// </summary>
        public List<Pair<string, string>> Headers => headers;

        /// <summary>
        /// Gets or sets the content of this part
        /// </summary>
        public byte[] Content
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
                    Headers.Find(x => x.First == headerName);
                if (pair != null)
                {
                    return pair.Second;
                }

                return null;
            }
        }
    }
}
