//-----------------------------------------------------------------------
// <copyright file="DirectTextReader.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

// (c) by BurnSystems '06

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BurnSystems.IO
{
    /// <summary>
    /// Dieser TextReader wird benötigt, wenn ein Teil des Streams als Text
    /// behandelt werden soll. Wird dieser TextReader nicht mehr benötigt,
    /// so wird nicht der darunterliegende Stream geschlossen. 
    /// </summary>
    public class DirectTextReader
    {
        /// <summary>
        /// Stream
        /// </summary>
        Stream m_oStream;

        /// <summary>
        /// Creates a new textreader
        /// </summary>
        /// <param name="oStream"></param>
        public DirectTextReader(Stream oStream)
        {
            m_oStream = oStream;
        }
        /// <summary>
        /// Reads one line, and returns it. This method moves the stream only to the place of 
        /// the end of the line. This means, that no buffering will be used. It will finish the 
        /// reading of line, if a \r was received. A \n at first position will be skipped
        /// </summary>
        /// <returns>Line, which was read</returns>
        public String ReadLine()
        {
            using (MemoryStream oMemoryStream = new MemoryStream())
            {
                int nCurrentByte;

                while ((nCurrentByte = m_oStream.ReadByte()) != -1)
                {
                    if (nCurrentByte == 10)
                    {
                        continue;
                    }
                    if (nCurrentByte == 13)
                    {
                        break;
                    }
                    oMemoryStream.WriteByte((byte) nCurrentByte);
                }


                UTF8Encoding oEnc = new UTF8Encoding();
                byte[] aoBytes = oMemoryStream.GetBuffer();
                return oEnc.GetString(aoBytes, 0, (int)oMemoryStream.Length);
            }

        }
    }
}
