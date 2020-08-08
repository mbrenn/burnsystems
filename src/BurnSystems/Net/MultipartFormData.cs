using System.Collections.Generic;

namespace BurnSystems.Net
{
    /// <summary>
    /// Implements the storage of a multipart formdata according
    /// to RFC 2388
    /// </summary>
    public class MultipartFormData
    {
        /// <summary>
        /// List of parts
        /// </summary>
        private readonly List<MultipartFormDataPart> _parts =
            new List<MultipartFormDataPart>();

        /// <summary>
        /// Gets the different parts of the formdata
        /// </summary>
        public List<MultipartFormDataPart> Parts => _parts;
    }
}
