//-----------------------------------------------------------------------
// <copyright file="StringManipulation.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

// (c) by BurnSystems '06

namespace BurnSystems
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Globalization;
    using System.Security.Cryptography;

    /// <summary>
    /// Delegat, der für die Funktion <c>Join</c> genutzt wird
    /// </summary>
    /// <typeparam name="T">Datentyp, der für jede Schleife übergeben werden soll. </typeparam>
    /// <param name="item">Jeweiliges Objekt, aus dem ein Eintrag erzeugt werden soll. </param>
    /// <returns>Result of joindelegate</returns>
    public delegate string Join<T>(T item);

    /// <summary>
    /// Basisklasse, die einige Stringmanipulationen durchführt
    /// </summary>
    public static class StringManipulation
    {
        /// <summary>
        /// Array of hexadecimal digits.
        /// </summary>
        private static char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        /// <summary>
        /// Inserts backslash before Single-Quote, Double-Quote and Backslash
        /// </summary>
        /// <param name="value">Input string</param>
        /// <returns>Output string with additional backslashes</returns>
        public static string AddSlashes(string value)
        {
            var result = value.Replace("\\", "\\\\");
            result = result.Replace("\"", "\\\"");
            return result = result.Replace("\'", "\\\'");
        }

        /// <summary>
        /// Strips backslashes before Single-Quote, Double-Quote and Backslash
        /// strValue = StripSlashes ( AddSlashes ( strValue ) );
        /// </summary>
        /// <param name="value">String with backslashes</param>
        /// <returns>String with less backslashes</returns>
        public static string StripSlashes(string value)
        {
            var result = value.Replace("\\\"", "\"");
            result = result.Replace("\\\'", "\'");
            return result.Replace("\\\\", "\\");
        }

        /// <summary>
        /// This method is equivalent to HttpUtility, except the encoding of
        /// the slash '/'. 
        /// </summary>
        /// <param name="value">String to be encoded</param>
        /// <returns>Encoded string</returns>
        public static string UrlEncode(string value)
        {
            return System.Web.HttpUtility.UrlEncode(value).Replace("%2f", "/")
                .Replace("%3a", ":");
        }

        /// <summary>
        /// Converts array of bytes to HexString
        /// </summary>
        /// <param name="bytes">Array of bytes</param>
        /// <returns>Hexadecimal string</returns>
        public static string ToHexString(byte[] bytes)
        {
            // From MSDN, thx to MS
            var chars = new char[bytes.Length * 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }

            return new string(chars);
        }

        /// <summary>
        /// Converts hexadecimal value to string. The string can be 'AFC9' or 'afc9'
        /// </summary>
        /// <param name="hexString">Hexadecimal string</param>
        /// <returns>Converted decimal string</returns>
        public static int HexToInt(string hexString)
        {
            var result = 0;
            for (var counter = 0; counter < hexString.Length; counter++)
            {
                var current = hexString[counter];
                var currentValue = 0;
                switch (current)
                {
                    case '0':
                        currentValue = 0;
                        break;
                    case '1':
                        currentValue = 1;
                        break;
                    case '2':
                        currentValue = 2;
                        break;
                    case '3':
                        currentValue = 3;
                        break;
                    case '4':
                        currentValue = 4;
                        break;
                    case '5':
                        currentValue = 5;
                        break;
                    case '6':
                        currentValue = 6;
                        break;
                    case '7':
                        currentValue = 7;
                        break;
                    case '8':
                        currentValue = 8;
                        break;
                    case '9':
                        currentValue = 9;
                        break;
                    case 'a':
                        currentValue = 10;
                        break;
                    case 'A':
                        currentValue = 10;
                        break;
                    case 'b':
                        currentValue = 11;
                        break;
                    case 'B':
                        currentValue = 11;
                        break;
                    case 'c':
                        currentValue = 12;
                        break;
                    case 'C':
                        currentValue = 12;
                        break;
                    case 'd':
                        currentValue = 13;
                        break;
                    case 'D':
                        currentValue = 13;
                        break;
                    case 'e':
                        currentValue = 14;
                        break;
                    case 'E':
                        currentValue = 14;
                        break;
                    case 'f':
                        currentValue = 15;
                        break;
                    case 'F':
                        currentValue = 15;
                        break;
                }

                result *= 16;
                result += currentValue;
            }

            return result;
        }

        /// <summary>
        /// Shortens string to a specific length and adds ellipsis, if they are
        /// necessary. 
        /// </summary>
        /// <param name="value">String to be shortened</param>
        /// <param name="maxLength">Maximum length of string after calling this method. The ellipses
        /// is not counted</param>
        /// <returns>Shortened string</returns>
        public static string ShortenString(string value, int maxLength)
        {
            return ShortenString(value, maxLength, "...");
        }

        /// <summary>
        /// Shortens string to a specific length and adds ellipsis, if they are
        /// necessary. 
        /// </summary>
        /// <param name="value">String to be shortened</param>
        /// <param name="maxLength">Maximum length of string after calling this method. The ellipses
        /// is not counted</param>
        /// <param name="ellipsis">Die genutzte Ellipsis, wenn der String zu lang ist.</param>
        /// <returns>Shortened string</returns>
        public static string ShortenString(string value, int maxLength, string ellipsis)
        {
            if (maxLength < 0)
            {
                throw new ArgumentException("nMaxLength < 0");
            }

            if (value.Length > maxLength)
            {
                return value.Substring(0, maxLength) + ellipsis;
            }

            return value;
        }

        /// <summary>
        /// Gets the first <c>nLetters</c> of <c>value</c> or the complete string,
        /// if length of string is smaller than <c>letters</c>.
        /// </summary>
        /// <param name="value">String to be shortened</param>
        /// <param name="letters">Number of letters</param>
        /// <returns>Shortened string</returns>
        public static string FirstLetters(string value, int letters)
        {
            if (value.Length > letters)
            {
                return value.Substring(0, letters);
            }

            return value;
        }

        /// <summary>
        /// Hashes String with Sha1-Algorithm
        /// </summary>
        /// <param name="data">Data to be hashed</param>
        /// <returns>Result of hash</returns>
        public static string Sha1(string data)
        {
            if (data == null)
            {
                return null;
            }

            var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            var utf8 = new System.Text.UTF8Encoding();

            var bytes = utf8.GetBytes(data);
            byte[] result;
            result = sha1.ComputeHash(bytes);

            return ToHexString(result);
        }

        /// <summary>
        /// Gets content of a textfile 
        /// </summary>
        /// <param name="filename">Path to file to be read</param>
        /// <returns>Content of file</returns>
        /// <exception cref="FileNotFoundException">This exception is thrown, when
        /// the file was not found</exception>
        [Obsolete("Use System.IO.File.ReadAllText")]
        public static string GetTextfileContent(string filename)
        {
            return File.ReadAllText(filename);
        }

        /// <summary>
        /// Creates a simple random string, containing of <c>length</c> letters
        /// </summary>
        /// <param name="length">Length of requested randomstring</param>
        /// <returns>Random string</returns>
        public static string RandomString(int length)
        {
            var pool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var result = new StringBuilder();
            var random = MathHelper.Random;

            var randomBytes = new byte[length];
            random.NextBytes(randomBytes);
            for (var n = 0; n < length; n++)
            {
                result.Append(pool[randomBytes[n] % 32]);
            }

            return result.ToString();
        }

        /// <summary>
        /// Creates a secure random string, containing of <c>length</c> letters
        /// </summary>
        /// <param name="length">Length of requested randomstring</param>
        /// <returns>Random string</returns>
        public static string SecureRandomString(int length)
        {
            var pool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var builder = new StringBuilder();
            var randomGenerator = new System.Security.Cryptography.RNGCryptoServiceProvider();

            var randomBytes = new byte[length];
            randomGenerator.GetBytes(randomBytes);
            for (var n = 0; n < length; n++)
            {
                // Only 32 values to avoid unequal distribution
                builder.Append(pool[randomBytes[n] % 32]);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Converts newline to br-Tags. The br-Tags are XHtml-conform.
        /// </summary>
        /// <param name="value">String to be converted</param>
        /// <returns>String being converted</returns>
        public static string Nl2Br(string value)
        {
            return value.Replace("\n", "<br />").Replace("\r", "");
        }

        /// <summary>
        /// Fügt an das Ende des Strings solange Zeichen von <c>paddingChar</c>
        /// hinzu bis der String die Länge <c>length</c> erreicht.
        /// </summary>
        /// <param name="value">String, der geändert werden soll</param>
        /// <param name="length">Länge des Strings</param>
        /// <param name="paddingChar">Zeichen, das hinzugefügt werden soll. </param>
        /// <returns>Aufgefüllter String. Ist der String länger als <c>length</c>,
        /// so wird ein gekürzter String zurückgegeben. </returns>
        public static string PaddingRight(string value, int length, char paddingChar)
        {
            var stringLength = value.Length;

            if (stringLength > length)
            {
                return value.Substring(0, length);
            }

            if (stringLength == length)
            {
                return value;
            }

            var builder = new StringBuilder(value, length);
            while (stringLength < length)
            {
                builder.Append(paddingChar);
                stringLength++;
            }

            return builder.ToString();
        }

        /// <summary>
        /// Fügt an das Ende des Strings solange Leerzeichen
        /// hinzu bis der String die Länge <c>nLength</c> erreicht.
        /// </summary>
        /// <param name="value">String, der geändert werden soll</param>
        /// <param name="length">Länge des Strings</param>
        /// <returns>Aufgefüllter String. Ist der String länger als <c>length</c>,
        /// so wird ein gekürzter String zurückgegeben. </returns>
        public static string PaddingRight(string value, int length)
        {
            return PaddingRight(value, length, ' ');
        }

        /// <summary>
        /// Fügt an den Anfang des Strings solange Zeichen von <c>paddingChar</c>
        /// hinzu bis der String die Länge <c>length</c> erreicht.
        /// </summary>
        /// <param name="value">String, der geändert werden soll</param>
        /// <param name="length">Länge des Strings</param>
        /// <param name="paddingChar">Zeichen, das hinzugefügt werden soll. </param>
        /// <returns>Aufgefüllter String. Ist der String länger als <c>nLength</c>,
        /// so wird ein gekürzter String zurückgegeben. </returns>
        public static string PaddingLeft(string value, int length, char paddingChar)
        {
            var stringLength = value.Length;

            if (stringLength > length)
            {
                return value.Substring(0, length);
            }

            if (stringLength == length)
            {
                return value;
            }

            var builder = new StringBuilder(length);
            while (stringLength < length)
            {
                builder.Append(paddingChar);
                stringLength++;
            }

            builder.Append(value);
            return builder.ToString();
        }

        /// <summary>
        /// Fügt an den Anfang des Strings solange Leerzeichen
        /// hinzu bis der String die Länge <c>length</c> erreicht.
        /// </summary>
        /// <param name="value">String, der geändert werden soll</param>
        /// <param name="length">Länge des Strings</param>
        /// <returns>Aufgefüllter String. Ist der String länger als <c>length</c>,
        /// so wird ein gekürzter String zurückgegeben. </returns>
        public static string PaddingLeft(string value, int length)
        {
            return PaddingLeft(value, length, ' ');
        }

        /// <summary>
        /// Bricht eine Zeichenkette nach einer bestimmten Anzahl von Zeichen
        /// um, so dass kein Wort getrennt wird. 
        /// </summary>
        /// <param name="value">String of text, which should be wrapped</param>
        /// <param name="lineLength">Länge der Zeile</param>
        /// <returns>Array mit Stringinhalten</returns>
        public static string[] WordWrap(string value, int lineLength)
        {
            var lines = new List<string>();

            int currentPos = 0;
            int start = 0;

            while (true)
            {
                var index = value.IndexOfAny(
                    new char[] { ' ', '\r', '\n' },
                    currentPos);

                if (index == -1)
                {
                    while (value.Length - start >= lineLength)
                    {
                        if (currentPos == start)
                        {
                            AddLineToArray(
                                lines,
                                value.Substring(start, lineLength).Trim());
                            currentPos += lineLength;
                            start = currentPos;
                        }
                        else
                        {
                            AddLineToArray(
                                lines,
                                value.Substring(start, currentPos - start).Trim());
                            start = currentPos;
                        }
                    }

                    lines.Add(value.Substring(start).Trim());
                    break;
                }

                if (index - start >= lineLength)
                {
                    if (currentPos == start)
                    {
                        AddLineToArray(
                            lines,
                            value.Substring(start, lineLength).Trim());
                        currentPos += lineLength;
                        start = currentPos;
                    }
                    else
                    {
                        AddLineToArray(
                            lines,
                            value.Substring(start, currentPos - start).Trim());
                        start = currentPos;
                    }
                }
                else
                {
                    currentPos = index + 1;
                }
            }

            return lines.ToArray();
        }

        /// <summary>
        /// Erzeugt mit Hilfe eines Delegaten einen String, dessen einzelne Bestandteile
        /// durch einen Trenner unterteilt sind. 
        /// </summary>
        /// <typeparam name="T">Typ des Objektes aus dem die einzelnen Bestandteile
        /// erzeugt werden sollen</typeparam>
        /// <param name="elements">Elements, which should be joined</param>
        /// <param name="joinDelegate">Delegat, der die Bestandteile aus dem Objekttyp <c>T</c> 
        /// erzeugt.</param>
        /// <param name="separator">Trenner, im Regelfall wird dieser ', ' sein. </param>
        /// <returns>Joined string</returns>
        public static string Join<T>(IEnumerable<T> elements, Join<T> joinDelegate, string separator)
        {
            var result = new StringBuilder();
            var first = true;

            foreach (var element in elements)
            {
                if (!first)
                {
                    result.Append(separator);
                }

                result.Append(joinDelegate(element));
                first = false;
            }

            return result.ToString();
        }
        
        /// <summary>
        /// Fügt eine Auflistung von Strings mit einem Seperator zusammen
        /// </summary>
        /// <param name="list">Liste von Elementen</param>
        /// <param name="separator">Seperator for the text between the string</param>
        /// <returns>Zusammengesetzter String</returns>
        public static string Join(IEnumerable<string> list, string separator)
        {
            return Join(list, x => x, separator);
        }

        /// <summary>
        /// Entfernt alle Zeichen aus dem Dateinamen, die ungültig sind 
        /// </summary>
        /// <param name="filename">Filename to be cleaned</param>
        /// <returns>Gesäuberter String</returns>
        public static string RemoveInvalidFilenameChars(string filename)
        {
            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(new string(invalidChar, 1), ((int)invalidChar).ToString());
            }

            return filename;
        }

        /// <summary>
        /// Fügt ein Element zum übergebenen Listenobjekt hinzu, wenn
        /// dieses nicht null oder Leer ist. 
        /// </summary>
        /// <param name="list">Liste, die das Element erhalten soll</param>
        /// <param name="line">String to be added to list</param>
        private static void AddLineToArray(IList<string> list, string line)
        {
            if (!String.IsNullOrEmpty(line))
            {
                list.Add(line);
            }
        }
    }
}
