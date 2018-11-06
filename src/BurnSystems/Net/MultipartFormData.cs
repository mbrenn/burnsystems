namespace BurnSystems.Net
{
    using System.Collections.Generic;

    /// <summary>
    /// Implements the storage of a multipart formdata according
    /// to RFC 2388
    /// </summary>
    public class MultipartFormData
    {
        /// <summary>
        /// List of parts
        /// </summary>
        private List<MultipartFormDataPart> parts =
            new List<MultipartFormDataPart>();

        /// <summary>
        /// Gets the different parts of the formdata
        /// </summary>
        public List<MultipartFormDataPart> Parts
        {
            get { return parts; }
        }
    }
}
