using System.Text;

namespace BurnSystems.IO
{
    /// <summary>
    /// This TextReader is required when a part of the stream is to be treated as text. When this TextReader. It allows
    /// a reading of an individual line within the stream without reading too much data into a temporary buffer
    /// which is after wards not available anymore 
    /// In case the instande of that class is no longer needed, the underlying stream is not closed
    /// 
    /// </summary>
    public class DirectTextReader
    {
        /// <summary>
        /// Stream to be used for direct streaming
        /// </summary>
        private readonly Stream _stream;

        /// <summary>
        /// Initializes a new instance of the DirectTextReader class.
        /// </summary>
        /// <param name="stream">Stream to be used</param>
        public DirectTextReader(Stream stream)
        {
            _stream = stream;
        }

        /// <summary>
        /// Reads one line, and returns it. This method moves the stream only to the place of 
        /// the end of the line. This means, that no buffering will be used. It will finish the 
        /// reading of line, if a \r was received. A \n at first position will be skipped
        /// </summary>
        /// <returns>Line, which was read</returns>
        public string ReadLine()
        {
            using var memoryStream = new MemoryStream();

            int currentByte;

            while ((currentByte = _stream.ReadByte()) != -1)
            {
                if (currentByte == 10) // \n
                {
                    continue;
                }

                if (currentByte == 13) // /r
                {
                    break;
                }

                memoryStream.WriteByte((byte)currentByte);
            }

            var bytes = memoryStream.GetBuffer();
            return Encoding.UTF8.GetString(bytes, 0, (int)memoryStream.Length);
        }
    }
}
