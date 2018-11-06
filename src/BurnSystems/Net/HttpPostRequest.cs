namespace BurnSystems.Net
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Web;
    using Test;

    /// <summary>
    /// über diese Klasse nutzt die internen .Net-Routinen und stellt
    /// ein einfaches Interface zur Erzeugung eines Post-HTTP-Requests
    /// zur Verfügung. 
    /// </summary>
    public class HttpPostRequest
    {
        /// <summary>
        /// Used WebRequest
        /// </summary>
        private HttpWebRequest request;

        /// <summary>
        /// Used WebResponse
        /// </summary>
        private HttpWebResponse response;

        /// <summary>
        /// Variables, which should be send as POST variables
        /// </summary>
        private Dictionary<string, string> postVariables =
            new Dictionary<string, string>();

        /// <summary>
        /// Gets the dictionary with variables, which should be sent
        /// as POST-Request
        /// </summary>
        public Dictionary<string, string> PostVariables
        {
            get { return postVariables; }
        }

        /// <summary>
        /// Gibt die Webresponse für diesen Request zurück
        /// </summary>
        /// <param name="url">Url of request</param>
        /// <returns>Webresponse of request</returns>
        public HttpWebResponse GetResponse(string url)
        {
            return GetResponse(new Uri(url));
        }

        /// <summary>
        /// Gibt die Webresponse für diesen Request zurück
        /// </summary>
        /// <param name="url">Url of webrequest</param>
        /// <returns>Webresponse of request</returns>
        public HttpWebResponse GetResponse(Uri url)
        {
            if (request == null)
            {
                request = WebRequest.Create(url) as HttpWebRequest;
                Ensure.IsNotNull(request);

                var builder = new StringBuilder();
                var first = true;
                foreach (KeyValuePair<string, string> pair in postVariables)
                {
                    if (!first)
                    {
                        builder.Append('&');
                    }

                    builder.AppendFormat(
                        "{0}={1}",
                        HttpUtility.UrlEncode(pair.Key),
                        HttpUtility.UrlEncode(pair.Value));
                    first = false;
                }

                byte[] postData = Encoding.ASCII.GetBytes(builder.ToString());
                request.Method = "POST";
                request.ContentLength = postData.Length;
                request.ContentType = 
                    "application/x-www-form-urlencoded; encoding='utf-8'";

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(postData, 0, postData.Length);
                }
            }

            if (response == null)
            {
                response = request.GetResponse() as HttpWebResponse;
            }

            return response;
        }
    }
}
