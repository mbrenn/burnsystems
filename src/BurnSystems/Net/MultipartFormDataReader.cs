using System.Collections.Generic;
using System.IO;
using System.Text;
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
        /// Boundary which separates the parts 
        /// </summary>
        private readonly byte[] _boundary;

        /// <summary>
        /// Stores the maximumsize of stream, that will be read
        /// </summary>
        private long _maxStreamSize = long.MaxValue;

        /// <summary>
        /// Initializes a new instance of the MultipartFormDataReader class.
        /// </summary>
        /// <param name="boundary">Used Boundary</param>
        public MultipartFormDataReader(string boundary)
        {
            _boundary = Encoding.ASCII.GetBytes(boundary);
        }

        /// <summary>
        /// Initializes a new instance of the MultipartFormDataReader class.
        /// </summary>
        /// <param name="boundary">Used Boundary</param>
        /// <param name="encoding">Encoding of stream</param>
        public MultipartFormDataReader(string boundary, Encoding encoding)
        {
            _boundary = encoding.GetBytes(boundary);
        }

        /// <summary>
        /// Initializes a new instance of the MultipartFormDataReader class.
        /// </summary>
        /// <param name="boundary">Used Boundary</param>
        public MultipartFormDataReader(byte[] boundary)
        {
            _boundary = boundary;
        }

        /// <summary>
        /// Gets or sets the number of bytes, which is the maximum of the
        /// read bytes of stream
        /// </summary>
        public long MaxStreamSize
        {
            get => _maxStreamSize;
            set => _maxStreamSize = value;
        }

        /// <summary>
        /// Defines the chunksize for the reader
        /// </summary>
        public const int ChunkSize = 65536;

        /// <summary>
        /// Reads the stream and returns an instance of the multipartformdata
        /// </summary>
        /// <param name="stream">Stream with data</param>
        /// <returns>Instance with containing data</returns>
        public MultipartFormData ReadStream(Stream stream)
        {
            var result = new MultipartFormData();

            // Convert Stream to Bytes
            using (var memoryStream = new MemoryStream())
            {
                var numberOfBytes = 0L; 

                // TODO: Do not read bytewise
                var tempBuffer = new byte[ChunkSize];
                while (true)
                {
                    var readBytes = stream.Read(tempBuffer, 0, ChunkSize);

                    if (readBytes == 0 || numberOfBytes > MaxStreamSize)
                    {
                        break;
                    }

                    memoryStream.Write(tempBuffer, 0, readBytes);
                    numberOfBytes += readBytes;
                }

                var offset = 0;

                // First part: Search for boundary
                var buffer = memoryStream.GetBuffer();
                SearchForBoundary(ref offset, buffer);

                MultipartFormDataPart? part;
                do
                {
                    part = ReadPart(ref offset, buffer);
                    if (part != null)
                    {
                        result.Parts.Add(part);
                    }
                }
                while (part != null);
            }

            return result;
        }

        /// <summary>
        /// Evaluates a headertext.
        /// </summary>
        /// <param name="part">Part, getting the headertext</param>
        /// <param name="headerText">Headertext to be parsed</param>
        private static void EvaluateHeader(MultipartFormDataPart part, string headerText)
        {
            var posColon = headerText.IndexOf(':');
            if (posColon == -1)
            {
                return;
            }

            var left = headerText.Substring(0, posColon).Trim();
            var right = headerText.Substring(posColon + 1).Trim();

            part.Headers.Add(new KeyValuePair<string, string>(left, right));

            if (left == "Content-Disposition")
            {
                var splittedRight = right.Split(';');

                foreach (var headerPart in splittedRight)
                {
                    var posColon2 = headerPart.IndexOf('=');
                    if (posColon2 == -1)
                    {
                        continue;
                    }

                    var left2 = headerPart.Substring(0, posColon2).Trim();
                    var right2 = headerPart.Substring(posColon2 + 1).Trim();

                    part.ContentDisposition[left2] = right2;
                }
            }
        }

        /// <summary>
        /// Searches for the boundary and changes the value <c>nOffset</c>, so the
        /// offset is behind the found boundary. If the boundary is not found, 
        /// <c>nOffset</c> is set to -1.
        /// </summary>
        /// <param name="offset">Offset of search</param>
        /// <param name="buffer">Buffer storing the message</param>
        private void SearchForBoundary(ref int offset, byte[] buffer)
        {
            var result = ListHelper.IndexOf(buffer, _boundary, offset);

            if (result == -1)
            {
                offset = -1;
            }
            else
            {
                // 2 is added for CRLF
                offset = result + _boundary.Length + 2;
            }
        }

        /// <summary>
        /// Reads one part
        /// </summary>
        /// <param name="offset">Offset for reading</param>
        /// <param name="buffer">Buffer containing the values</param>
        /// <returns>A new part or null, if the stream is invalid</returns>
        private MultipartFormDataPart? ReadPart(ref int offset, byte[] buffer)
        {
            var result = new MultipartFormDataPart();

            // Reads the part. 
            // First: Read the headers
            var start = offset;
            while (true)
            {
                if (offset >= buffer.Length)
                {
                    offset = -1;
                    return null;
                }

                var currentByte = buffer[offset];
                if (currentByte == 13 || currentByte == 10)
                {
                    // Convert region between start and currentposition to String
                    var headerText = Encoding.ASCII.GetString(
                        buffer, 
                        start,
                        offset - start);

                    // Skip '\n'
                    offset += 2;
                    start = offset;
                    if (string.IsNullOrEmpty(headerText.Trim()))
                    {
                        // Header has been read
                        break;
                    }

                    EvaluateHeader(result, headerText);
                }
                else
                {
                    offset++;
                }
            }

            // Headers are read, now search for endboundary and quit
            var nextBoundaryPos = ListHelper.IndexOf(buffer, _boundary, offset);
            if (nextBoundaryPos == -1)
            {
                return null;
            }

            // Store Content (without finishing CRLF)
            result.Content = new byte[nextBoundaryPos - offset - 2];
            var pos = 0;
            for (var index = offset; index < (nextBoundaryPos - 2); index++)
            {
                result.Content[pos] = buffer[index];
                pos++;
            }

            // Start of Boundary + Length of Boundary + 2
            offset = nextBoundaryPos + _boundary.Length + 2;

            return result;
        }
    }
}
