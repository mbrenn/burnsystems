//-----------------------------------------------------------------------
// <copyright file="HttpPostRequest.cs" company="Martin Brenn">
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
using System.Net;
using System.IO;
using System.Web;

namespace BurnSystems.Net
{
    /// <summary>
    /// über diese Klasse nutzt die internen .Net-Routinen und stellt
    /// ein einfaches Interface zur Erzeugung eines Post-HTTP-Requests
    /// zur Verfügung. 
    /// </summary>
    public class HttpPostRequest
    {
        /// <summary>
        /// WebRequest
        /// </summary>
        HttpWebRequest _Request;

        /// <summary>
        /// WebResponse
        /// </summary>
        HttpWebResponse _Response;

        /// <summary>
        /// PostVariables
        /// </summary>
        Dictionary<String, String> _PostVariables =
            new Dictionary<string, string>();

        /// <summary>
        /// Postvariables
        /// </summary>
        public Dictionary<String, String> PostVariables
        {
            get { return _PostVariables; }
        }

        /// <summary>
        /// Request
        /// </summary>
        public HttpPostRequest()
        {
        }

        /// <summary>
        /// Gibt die Webresponse für diesen Request zurück
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        public HttpWebResponse GetResponse(String strUrl)
        {
            return GetResponse(new Uri(strUrl));
        }

        /// <summary>
        /// Gibt die Webresponse für diesen Request zurück
        /// </summary>
        /// <returns></returns>
        public HttpWebResponse GetResponse(Uri oUrl)
        {
            if (_Request == null)
            {
                _Request = WebRequest.Create(oUrl) as HttpWebRequest;
                StringBuilder oBuilder = new StringBuilder();
                bool bFirst = true;
                foreach ( KeyValuePair<String, String> oPair in _PostVariables)
                {
                    if (!bFirst)
                    {
                        oBuilder.Append('&');
                    }
                    oBuilder.AppendFormat("{0}={1}",
                        HttpUtility.UrlEncode(oPair.Key),
                        HttpUtility.UrlEncode(oPair.Value));
                    bFirst = false;
                }

                byte[] aoPostData = Encoding.ASCII.GetBytes(oBuilder.ToString());
                _Request.Method = "POST";
                _Request.ContentLength = aoPostData.Length;
                _Request.ContentType = "application/x-www-form-urlencoded; encoding='utf-8'";

                using (Stream oStream = _Request.GetRequestStream())
                {
                    oStream.Write(aoPostData, 0, aoPostData.Length);
                }
            }
                        
            if (_Response == null)
            {
                _Response = _Request.GetResponse() as HttpWebResponse;
            }

            return _Response;
        }
    }
}
