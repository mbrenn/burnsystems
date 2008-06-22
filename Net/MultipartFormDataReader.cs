//-----------------------------------------------------------------------
// <copyright file="MultipartFormDataReader.cs" company="Martin Brenn">
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
using System.IO;
using BurnSystems.Collections;

namespace BurnSystems.Net
{
    /// <summary>
    /// Reads a stream and returns a MultipartFormData-Instance. 
    /// This method is NOT threadsafe
    /// </summary>
    public class MultipartFormDataReader
    {
        /// <summary>
        /// Boundary
        /// </summary>
        byte[] _Boundary;

        /// <summary>
        /// Creates a new instance and stores the boundary for the incoming stream
        /// </summary>
        /// <param name="strBoundary">Boundary</param>
        public MultipartFormDataReader(String strBoundary)
        {
            _Boundary = ASCIIEncoding.ASCII.GetBytes(strBoundary);
        }

        /// <summary>
        /// Creates a new instance and stores the boundary for the incoming stream
        /// </summary>
        /// <param name="strBoundary">Boundary</param>
        /// <param name="oEncoding">Encoding of stream</param>
        public MultipartFormDataReader(String strBoundary, Encoding oEncoding)
        {
            _Boundary = oEncoding.GetBytes(strBoundary);
        }

        /// <summary>
        /// Creates a new instance and stores the boundary for the incoming stream
        /// </summary>
        /// <param name="aoBoundary">Boundary</param>
        public MultipartFormDataReader(byte[] aoBoundary)
        {
            _Boundary = aoBoundary;
        }

        /// <summary>
        /// Reads the stream and returns an instance of the multipartformdata
        /// </summary>
        /// <param name="oStream">Stream with data</param>
        /// <returns>Instance with containing data</returns>
        public MultipartFormData ReadStream(Stream oStream)
        {
            var oReturn = new MultipartFormData();
            
            // Convert Stream to Bytes
            using (var oMemoryStream = new MemoryStream())
            {
                int nByte;
                while ((nByte = oStream.ReadByte()) != -1)
                {
                    oMemoryStream.WriteByte((byte)nByte);
                }

                var nOffset = 0;
                // First part: Search for boundary
                var aoBuffer = oMemoryStream.GetBuffer();
                SearchForBoundary(ref nOffset, aoBuffer);

                MultipartFormDataPart oPart;
                do
                {
                    oPart = ReadPart(ref nOffset, aoBuffer);
                    if (oPart != null)
                    {
                        oReturn.Parts.Add(oPart);
                    }
                }
                while (oPart != null);
            }
            return oReturn;
        }

        /// <summary>
        /// Searches for the boundary and changes the value <c>nOffset</c>, so the
        /// offset is behind the found boundary. If the boundary is not found, 
        /// <c>nOffset</c> is set to -1.
        /// </summary>
        /// <param name="nOffset">Offset</param>
        /// <param name="aoBuffer">Buffer</param>
        private void SearchForBoundary(ref int nOffset, byte[] aoBuffer)
        {
            var nReturn = ListHelper.IndexOf(aoBuffer, _Boundary, nOffset);

            if (nReturn == -1)
            {
                nOffset = -1;
            }
            else
            {
                // 2 is added for CRLF
                nOffset = nReturn + _Boundary.Length + 2;
            }
            
        }

        /// <summary>
        /// Reads one part
        /// </summary>
        /// <param name="nOffset">Offset</param>
        /// <param name="aoBuffer">Buffer containing the values</param>
        /// <returns>A new part or null, if the stream is invalid</returns>
        private MultipartFormDataPart ReadPart(ref int nOffset, byte[] aoBuffer)
        {
            var oReturn = new MultipartFormDataPart();
            // Reads the part. 
            // First: Read the headers
            var nStart = nOffset;
            while (true)
            {
                if (nOffset >= aoBuffer.Length)
                {
                    nOffset = -1;
                    return null;
                }

                byte oCurrentByte = aoBuffer[nOffset];
                if (oCurrentByte == 13 || oCurrentByte == 10)
                {
                    // Convert region between start and currentposition to String

                    var strHeaderText = ASCIIEncoding.ASCII.GetString(aoBuffer, nStart,
                        nOffset - nStart);

                    //  Skip '\n'
                    nOffset += 2;
                    nStart = nOffset;
                    if (String.IsNullOrEmpty(strHeaderText.Trim()))
                    {
                        // Header has been read
                        break;
                    }
                    else
                    {
                        EvaluateHeader(oReturn, strHeaderText);
                    }
                }
                else
                {
                    nOffset++;
                }
            }

            // Headers are read, now search for endboundary and quit
            var nReturn = ListHelper.IndexOf(aoBuffer, _Boundary, nOffset);
            if (nReturn == -1)
            {
                return null;
            }

            // Store Content (without finishing CRLF)
            oReturn.Content = new byte[nReturn - nOffset - 2];
            var nPos = 0;
            for (var nIndex = nOffset; nIndex < (nReturn - 2); nIndex++)
            {
                oReturn.Content[nPos] = aoBuffer[nIndex];
                nPos++;
            }

            // Start of Boundary + Length of Boundary + 2
            nOffset = nReturn + _Boundary.Length + 2;

            return oReturn;
        }

        /// <summary>
        /// Evaluates a headertext.
        /// </summary>
        /// <param name="strHeaderText">Headertext to be parsed</param>
        /// <param name="oPart">Part, getting the headertext</param>
        private void EvaluateHeader(MultipartFormDataPart oPart, string strHeaderText)
        {
            int nPosColon = strHeaderText.IndexOf(':');
            if (nPosColon == -1)
            {
                return;
            }

            var strLeft = strHeaderText.Substring(0, nPosColon).Trim();
            var strRight = strHeaderText.Substring(nPosColon + 1).Trim();

            oPart.Headers.Add(new Pair<string, string>(strLeft, strRight));

            if (strLeft == "Content-Disposition")
            {
                var astrRight = strRight.Split(new[] { ';' });

                foreach ( var strHeaderPart in astrRight )
                {
                    int nPosColon2 = strHeaderPart.IndexOf ( '=' );
                    if (nPosColon2 == -1)
                    {
                        continue;
                    }

                    var strLeft2 = strHeaderPart.Substring(0, nPosColon2).Trim();
                    var strRight2 = strHeaderPart.Substring(nPosColon2 + 1).Trim();

                    oPart.ContentDisposition[strLeft2] = strRight2;
                }
            }
        }
    }
}
