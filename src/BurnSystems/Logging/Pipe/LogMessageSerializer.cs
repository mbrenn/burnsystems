using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.Logging.Pipe
{
    public static class LogMessageSerializer
    {
        public static byte[] ConvertMessage(LogMessage logMessage)
        {
            var category = Encoding.UTF8.GetBytes(logMessage.Category ?? string.Empty);
            var message = Encoding.UTF8.GetBytes(logMessage.Message ?? string.Empty);
            var categoryLength = category.Length;
            var messageLength = message.Length;

            var totalLength =
                2 // identifier
                + 4 // length of total message
                + 1 // length of loglevel
                + 4 // length information of category
                + categoryLength // the category itself
                + 4 // length information of message
                + messageLength; // the message

            var bytes = new byte[totalLength];
            var offset = 0;

            // Identifier of message
            SetInteger16(0x0001, bytes, ref offset);
            // Length of total message
            SetInteger32(totalLength - 6, bytes, ref offset);
            // LogLevel
            SetInteger8((byte)logMessage.LogLevel, bytes, ref offset);

            // Category
            SetString(logMessage.Category ?? string.Empty, bytes, ref offset);

            // Message
            SetString(logMessage.Message ?? string.Empty, bytes, ref offset);

            return bytes;
        }

        public static void SetInteger32(int value, byte[] buffer, ref int offset)
        {
            buffer[0 + offset] = (byte)(value >> 0);
            buffer[1 + offset] = (byte)(value >> 8);
            buffer[2 + offset] = (byte)(value >> 16);
            buffer[3 + offset] = (byte)(value >> 24);
            offset += 4;
        }

        public static void SetInteger16(int value, byte[] buffer, ref int offset)
        {
            buffer[0 + offset] = (byte)(value >> 0);
            buffer[1 + offset] = (byte)(value >> 8);
            offset += 2;
        }

        public static void SetInteger8(int value, byte[] buffer, ref int offset)
        {
            buffer[0 + offset] = (byte)value;
            offset += 1;
        }

        private static void SetString(string text, byte[] result, ref int offset)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            SetInteger32(bytes.Length, result, ref offset);

            Array.Copy(bytes, 0, result, offset, bytes.Length);

            offset += bytes.Length;
        }

        public static int GetInteger8(byte[] bytes, ref int offset)
        {
            var result = bytes[offset];
            offset += 1;
            return result;
        }

        public static int GetInteger16(byte[] bytes, ref int offset)
        {
            var result =
                (bytes[offset + 0]) +
                (bytes[offset + 1] << 8);
            offset += 2;
            return result;
        }

        public static int GetInteger32(byte[] bytes, ref int offset)
        {
            var result =
                bytes[offset + 0] +
                (bytes[offset + 1] << 8) +
                (bytes[offset + 2] << 16) +
                (bytes[offset + 3] << 24);
            offset += 4;
            return result;
        }

        private static string GetString(byte[] message, ref int offset)
        {
            var length = GetInteger32(message, ref offset);
            var result = Encoding.UTF8.GetString(message, offset, length);
            offset += length;
            return result;
        }

        public static async Task<LogMessage> ParseMessage(Stream stream)
        {
            var buffer = new byte[4];
            var offset = 0;
            await ReadBytesFromStream(stream, buffer, 2);
            var messageId = GetInteger16(buffer, ref offset);
            offset = 0;

            if (messageId == 0x0001)
            {
                await ReadBytesFromStream(stream, buffer, 4);
                var length = GetInteger32(buffer, ref offset);

                buffer = new byte[length];
                await ReadBytesFromStream(stream, buffer, length);
                return ParseLogMessage(buffer);
            }

            return null;
        }

        public static LogMessage ParseLogMessage(byte[] message)
        {
            var offset = 0;
            var result = new LogMessage
            {
                LogLevel = (LogLevel) GetInteger8(message, ref offset),
                Category = GetString(message, ref offset),
                Message = GetString(message, ref offset)
            };
            return result;
        }

        /// <summary>
        /// Reads data into a complete array, throwing an EndOfStreamException
        /// if the stream runs out of data first, or if an IOException
        /// naturally occurs.
        ///
        /// Source: http://jonskeet.uk/csharp/readbinary.html
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <param name="data">The array to read bytes into. The array
        /// will be completely filled from the stream, so an appropriate
        /// size must be given.</param>
        /// <param name="length">Length of the array to read</param>
        public static async Task ReadBytesFromStream(Stream stream, byte[] data, int length)
        {
            var offset = 0;
            var remaining = length;
            while (remaining > 0)
            {
                var read = await stream.ReadAsync(data, offset, remaining);
                if (read <= 0)
                    throw new EndOfStreamException
                        ($"End of stream reached with {remaining} bytes left to read");
                remaining -= read;
                offset += read;
            }
        }

    }
}