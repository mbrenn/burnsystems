#nullable enable
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.Logging.Pipe
{
    public static class SerializerMessageIDs
    {
        /// <summary>
        /// The given message contains the content of a LogMessage instance
        /// </summary>
        public const int SendLogMessage = 0x0001;
        
        /// <summary>
        /// The given message contains the content of a LogMetricMessage[int] instance
        /// </summary>
        public const int SendLogMetricMessageInteger = 0x0002;
        
        /// <summary>
        /// The given message contains the content of a LogMetricMessage[double} instance
        /// </summary>
        public const int SendLogMetricMessageDouble = 0x0003;
    }
    
    public static class LogMessageSerializer
    {
        /// <summary>
        /// Converts the given message to an array of bytes
        /// </summary>
        /// <param name="logMessage">Message to be converted</param>
        /// <returns>The array of bytes containing the message</returns>
        public static byte[] ConvertMessage(LogMessage logMessage)
        {
            var totalLength =
                GetByteCountForInteger16() // identifier
                + GetByteCountForInteger32() // length of total message
                + GetByteCountForInteger8() // length of loglevel
                + GetByteCountForString(logMessage.Category) // length information of category
                + GetByteCountForString(logMessage.Message); // length information of message

            var logMetricMessageInt = logMessage as LogMetricMessage<int>;
            var logMetricMessageDouble = logMessage as LogMetricMessage<double>;

            var messageId =
                logMetricMessageInt != null
                    ? SerializerMessageIDs.SendLogMetricMessageInteger
                    : logMetricMessageDouble != null
                        ? SerializerMessageIDs.SendLogMetricMessageDouble
                        : SerializerMessageIDs.SendLogMessage;

            switch (messageId)
            {
                case SerializerMessageIDs.SendLogMetricMessageInteger:
                    Debug.Assert(logMetricMessageInt != null, nameof(logMetricMessageInt) + " != null");
                    totalLength += GetByteCountForInteger32() +
                                   GetByteCountForString(logMetricMessageInt!.Unit);
                    break;
                
                
                case SerializerMessageIDs.SendLogMetricMessageDouble:
                    Debug.Assert(logMetricMessageDouble != null, nameof(logMetricMessageDouble) + " != null");
                    totalLength += GetByteCountForDouble() +
                                   GetByteCountForString(logMetricMessageDouble!.Unit);
                    break;
            }
            
            var bytes = new byte[totalLength];
            var offset = 0;

            // Identifier of message
            SetInteger16(messageId, bytes, ref offset);
            
            // Length of total message
            SetInteger32(totalLength - 6, bytes, ref offset);
            
            // LogLevel
            SetInteger8((byte) logMessage.LogLevel, bytes, ref offset);

            // Category
            SetString(logMessage.Category ?? string.Empty, bytes, ref offset);

            // Message
            SetString(logMessage.Message ?? string.Empty, bytes, ref offset);

            if (messageId == SerializerMessageIDs.SendLogMetricMessageInteger)
            {
                Debug.Assert(logMetricMessageInt != null, nameof(logMetricMessageInt) + " != null");
                
                // Message
                SetInteger32(logMetricMessageInt!.Value, bytes, ref offset);
                
                // Unit
                SetString(logMetricMessageInt!.Unit, bytes, ref offset);
                
            }
            else if (messageId == SerializerMessageIDs.SendLogMetricMessageDouble)
            {
                Debug.Assert(logMetricMessageDouble != null, nameof(logMetricMessageDouble) + " != null");
                
                // Message
                SetDouble(logMetricMessageDouble!.Value, bytes, ref offset);
                
                // Unit
                SetString(logMetricMessageDouble!.Unit, bytes, ref offset);
            }

            return bytes;
        }

        private static int GetByteCountForBytes(byte[] bytes) => bytes.Length;

        private static int GetByteCountForInteger8() => sizeof(Byte);

        private static int GetByteCountForInteger16() => sizeof(Int16);
        
        private static int GetByteCountForInteger32() => sizeof(Int32);
        
        private static int GetByteCountForDouble() => sizeof(double);
        
        private static int GetByteCountForString(string value) =>
            Encoding.UTF8.GetBytes(value ?? string.Empty).Length + GetByteCountForInteger32();
        

        public static void SetBytes(byte[] bytes, byte[] buffer, ref int offset)
        {
            Array.Copy(bytes, 0, buffer, offset, bytes.Length);

            offset += bytes.Length;
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

        public static void SetDouble(double value, byte[] buffer, ref int offset)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Copy(bytes, 0, buffer, offset, bytes.Length);
            offset += bytes.Length;
        }

        private static void SetString(string text, byte[] result, ref int offset)
        {
            text ??= string.Empty;
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

        public static double GetDouble(byte[] buffer, ref int offset)
        {
            var result = BitConverter.ToDouble(buffer, offset);
            offset += GetByteCountForDouble();

            return result;
        }

        private static string GetString(byte[] message, ref int offset)
        {
            var length = GetInteger32(message, ref offset);
            var result = Encoding.UTF8.GetString(message, offset, length);
            offset += length;
            return result;
        }

        public static async Task<LogMessage?> ParseMessage(Stream stream)
        {
            var buffer = new byte[4];
            var offset = 0;
            await ReadBytesFromStream(stream, buffer, 2);
            var messageId = GetInteger16(buffer, ref offset);
            offset = 0;

            if (messageId == SerializerMessageIDs.SendLogMessage ||
                messageId == SerializerMessageIDs.SendLogMetricMessageInteger ||
                messageId == SerializerMessageIDs.SendLogMetricMessageDouble)
            {
                await ReadBytesFromStream(stream, buffer, 4);
                var length = GetInteger32(buffer, ref offset);

                buffer = new byte[length];
                await ReadBytesFromStream(stream, buffer, length);
                return ParseLogMessage(messageId, buffer);
            }
            
            return null;
        }

        public static LogMessage? ParseLogMessage(int messageId, byte[] message)
        {
            var offset = 0;

            return messageId switch
            {
                SerializerMessageIDs.SendLogMessage => new LogMessage
                {
                    LogLevel = (LogLevel) GetInteger8(message, ref offset),
                    Category = GetString(message, ref offset),
                    Message = GetString(message, ref offset)
                },
                SerializerMessageIDs.SendLogMetricMessageInteger => new LogMetricMessage<int>
                {
                    LogLevel = (LogLevel) GetInteger8(message, ref offset),
                    Category = GetString(message, ref offset),
                    Message = GetString(message, ref offset),
                    Value = GetInteger32(message, ref offset),
                    Unit = GetString(message, ref offset)
                },
                SerializerMessageIDs.SendLogMetricMessageDouble => new LogMetricMessage<double>
                {
                    LogLevel = (LogLevel) GetInteger8(message, ref offset),
                    Category = GetString(message, ref offset),
                    Message = GetString(message, ref offset),
                    Value = GetDouble(message, ref offset),
                    Unit = GetString(message, ref offset)
                },
                _ => null
            };
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