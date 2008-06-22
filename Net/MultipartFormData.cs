//-----------------------------------------------------------------------
// <copyright file="MultipartFormData.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

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
        List<MultipartFormDataPart> _Parts =
            new List<MultipartFormDataPart>();

        /// <summary>
        /// The different parts of the formdata
        /// </summary>
        public List<MultipartFormDataPart> Parts
        {
            get { return _Parts; }
        }


        /// <summary>
        /// Creates a new instance
        /// </summary>
        public MultipartFormData()
        {
        }
    }
}
