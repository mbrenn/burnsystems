//-----------------------------------------------------------------------
// <copyright file="MultipartFormDataPart.cs" company="Martin Brenn">
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
using BurnSystems.Collections;

namespace BurnSystems.Net
{
    /// <summary>
    /// Contains one part of the multipartform data. 
    /// </summary>
    public class MultipartFormDataPart
    {
        /// <summary>
        /// Stores the elements of the content-disposition header 
        /// </summary>
        NiceDictionary<String, String> _ContentDisposition =
            new NiceDictionary<string, string>();

        /// <summary>
        /// Stores a list of all header
        /// </summary>
        List<Pair<String, String>> _Headers = new List<Pair<String, String>>();

        /// <summary>
        /// Content of this part
        /// </summary>
        byte[] _Content;

        /// <summary>
        /// Stores the elements of the content-disposition header
        /// </summary>
        public NiceDictionary<String, String> ContentDisposition
        {
            get { return _ContentDisposition; }
        }

        /// <summary>
        /// Stores a list of headers
        /// </summary>
        public List<Pair<String, String>> Headers
        {
            get { return _Headers; }
        }

        /// <summary>
        /// Gets a specific header
        /// </summary>
        /// <param name="strHeaderName">Name of requested header</param>
        /// <returns>Found header or null</returns>
        public String this[String strHeaderName]
        {
            get
            {
                var oPair =
                    Headers.Find(x => x.First == strHeaderName);
                if (oPair != null)
                {
                    return oPair.Second;
                }
                return null;
            }
        }

        /// <summary>
        /// Content of this part
        /// </summary>
        public byte[] Content
        {
            get { return _Content; }
            set { _Content = value; }
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public MultipartFormDataPart()
        {
        }
    }
}
