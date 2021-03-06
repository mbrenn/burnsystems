﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using BurnSystems.Test;

// ReSharper disable FormatStringProblem

namespace BurnSystems
{
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
        private static readonly char[] HexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        /// <summary>
        /// Inserts backslash before Single-Quote, Double-Quote and Backslash
        /// </summary>
        /// <param name="value">Input string</param>
        /// <returns>Output string with additional backslashes</returns>
        public static string AddSlashes(this string value)
        {
            var result = value.Replace("\\", "\\\\");
            result = result.Replace("\"", "\\\"");
            return result.Replace("\'", "\\\'");
        }

        /// <summary>
        /// Strips backslashes before Single-Quote, Double-Quote and Backslash
        /// strValue = StripSlashes ( AddSlashes ( strValue ) );
        /// </summary>
        /// <param name="value">String with backslashes</param>
        /// <returns>String with less backslashes</returns>
        public static string StripSlashes(this string value)
        {
            var result = value.Replace("\\\"", "\"");
            result = result.Replace("\\\'", "\'");
            return result.Replace("\\\\", "\\");
        }

        /// <summary>
        /// Converts array of bytes to HexString
        /// </summary>
        /// <param name="bytes">Array of bytes</param>
        /// <returns>Hexadecimal string</returns>
        public static string ToHexString(this byte[] bytes)
        {
            // From MSDN, thx to MS
            var chars = new char[bytes.Length * 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i * 2] = HexDigits[b >> 4];
                chars[(i * 2) + 1] = HexDigits[b & 0xF];
            }

            return new string(chars);
        }

        /// <summary>
        /// Converts hexadecimal value to string. The string can be 'AFC9' or 'afc9'
        /// </summary>
        /// <param name="hexValue">Hexadecimal string</param>
        /// <returns>Converted decimal string</returns>
        public static int HexToInt(this string hexValue)
        {
            Ensure.IsNotNull(hexValue);

            var result = 0;
            for (var counter = 0; counter < hexValue.Length; counter++)
            {
                var current = hexValue[counter];
                var currentValue = current switch
                {
                    '0' => 0,
                    '1' => 1,
                    '2' => 2,
                    '3' => 3,
                    '4' => 4,
                    '5' => 5,
                    '6' => 6,
                    '7' => 7,
                    '8' => 8,
                    '9' => 9,
                    'a' => 10,
                    'A' => 10,
                    'b' => 11,
                    'B' => 11,
                    'c' => 12,
                    'C' => 12,
                    'd' => 13,
                    'D' => 13,
                    'e' => 14,
                    'E' => 14,
                    'f' => 15,
                    'F' => 15,
                    _ => 0
                };

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
        /// <param name="ellipsis">Die genutzte Ellipsis, wenn der String zu lang ist.</param>
        /// <returns>Shortened string</returns>
        public static string ShortenString(this string value, int maxLength, string ellipsis = "...")
        {
            if (maxLength < 0)
            {
                throw new ArgumentException($"{nameof(maxLength)} < 0");
            }

            if (value.Length > maxLength)
            {
                return value.Substring(0, maxLength) + ellipsis;
            }

            return value;
        }

        /// <summary>
        /// Gets the first <c>letters</c> of <c>value</c> or the complete string,
        /// if length of string is smaller than <c>letters</c>.
        /// </summary>
        /// <param name="value">String to be shortened</param>
        /// <param name="letters">Number of letters</param>
        /// <returns>Shortened string</returns>
        public static string FirstLetters(this string value, int letters)
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
        public static string Sha1(this string data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var utf8 = new UTF8Encoding();

            var bytes = utf8.GetBytes(data);
            return Sha1(bytes);
        }

        /// <summary>
        /// Hashes a bytebuffer with Sha1
        /// </summary>
        /// <param name="bytes">Buffer to be hashed</param>
        /// <returns>Hash as string</returns>
        public static string Sha1(this byte[] bytes)
        {
            using var sha1 = new SHA1CryptoServiceProvider();
            var result = sha1.ComputeHash(bytes);

            return ToHexString(result);
        }

        /// <summary>
        /// Creates a simple random string, containing of <c>length</c> letters
        /// </summary>
        /// <param name="length">Length of requested randomstring</param>
        /// <param name="withoutNumbers">Flag, whether the creates string shall not contain numbers</param>
        /// <returns>Random string</returns>
        public static string RandomString(int length, bool withoutNumbers = false)
        {
            var pool = withoutNumbers ? 
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ":
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var result = new StringBuilder();
            var random = MathHelper.Random;

            var randomBytes = new byte[length];
            random.NextBytes(randomBytes);
            for (var n = 0; n < length; n++)
            {
                result.Append(pool[randomBytes[n] % pool.Length]);
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
            var randomGenerator = new RNGCryptoServiceProvider();

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
        public static string Nl2Br(this string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return value.Replace("\n", "<br />").Replace("\r", string.Empty);
        }

        /// <summary>
        /// Fügt an das Ende des Strings solange Zeichen von <c>paddingValue</c>
        /// hinzu bis der String die Länge <c>length</c> erreicht.
        /// </summary>
        /// <param name="value">String, der geändert werden soll</param>
        /// <param name="length">Länge des Strings</param>
        /// <param name="paddingValue">Zeichen, das hinzugefügt werden soll. </param>
        /// <returns>Aufgefüllter String. Ist der String länger als <c>length</c>,
        /// so wird ein gekürzter String zurückgegeben. </returns>
        public static string PaddingRight(this string value, int length, char paddingValue = ' ')
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
                builder.Append(paddingValue);
                stringLength++;
            }

            return builder.ToString();
        }

        /// <summary>
        /// Fügt an den Anfang des Strings solange Zeichen von <c>paddingValue</c>
        /// hinzu bis der String die Länge <c>length</c> erreicht.
        /// </summary>
        /// <param name="value">String, der geändert werden soll</param>
        /// <param name="length">Länge des Strings</param>
        /// <param name="paddingValue">Zeichen, das hinzugefügt werden soll. </param>
        /// <returns>Aufgefüllter String. Ist der String länger als <c>nLength</c>,
        /// so wird ein gekürzter String zurückgegeben. </returns>
        public static string PaddingLeft(this string value, int length, char paddingValue = ' ')
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
                builder.Append(paddingValue);
                stringLength++;
            }

            builder.Append(value);
            return builder.ToString();
        }

        /// <summary>
        /// Repeats a specific character several times and returns
        /// its result
        /// </summary>
        /// <param name="character">Character to be repeated</param>
        /// <param name="repetitions">Number of required repetitions</param>
        /// <returns>String with <c>repetitions</c> Characters of <c>character</c>.</returns>
        public static string Repeat(this char character, int repetitions)
        {
            Ensure.IsGreaterOrEqual(repetitions, 0);

            // Check for empty strings
            if (repetitions == 0)
            {
                return string.Empty;
            }

            // Creates the string builder
            var result = new StringBuilder();
            result.EnsureCapacity(repetitions);

            for (var n = 0; n < repetitions; n++)
            {
                result.Append(character);
            }

            return result.ToString();
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
            Ensure.IsGreaterOrEqual(lineLength, 0);

            var lines = new List<string>();

            var currentPos = 0;
            var start = 0;

            while (true)
            {
                var index = value.IndexOfAny(
                    new[] { ' ', '\r', '\n' },
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
        public static string Join<T>(this IEnumerable<T> elements, Join<T> joinDelegate, string separator)
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
        public static string Join(this IEnumerable<string> list, string separator)
        {
            return Join(list, x => x, separator);
        }

        /// <summary>
        /// Entfernt alle Zeichen aus dem Dateinamen, die ungültig sind 
        /// </summary>
        /// <param name="fileName">Filename to be cleaned</param>
        /// <returns>Gesäuberter String</returns>
        public static string RemoveInvalidFileNameChars(string fileName)
        {
            Ensure.IsNotNull(fileName);

            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(new string(invalidChar, 1), ((int)invalidChar).ToString(CultureInfo.InvariantCulture));
            }

            return fileName;
        }

        /// <summary>
        /// Fügt ein Element zum übergebenen Listenobjekt hinzu, wenn
        /// dieses nicht null oder Leer ist. 
        /// </summary>
        /// <param name="list">Liste, die das Element erhalten soll</param>
        /// <param name="line">String to be added to list</param>
        private static void AddLineToArray(IList<string> list, string line)
        {
            if (!string.IsNullOrEmpty(line))
            {
                list.Add(line);
            }
        }

        /// <summary>
        /// Converts the number of the length of a file to a short string, 
        /// which contains the filesize as a number and a SI-Prefix
        /// </summary>
        /// <param name="fileLength">Filelength to be converted</param>
        /// <param name="decimals">Number of decimals to be shown</param>
        /// <returns>String containing the length</returns>
        public static string GetFileLengthInfo(long fileLength, int decimals)
        {
            var doubleFileLength = (double)fileLength;
            var prefix = new[] { "Bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB" };
            var prefixNumber = 0;
            while (doubleFileLength > 1024 && prefixNumber < (prefix.Length - 1))
            {
                doubleFileLength /= 1024;
                prefixNumber++;
            }

            if (prefixNumber == 0)
            {
                return $"{doubleFileLength:n0} {prefix[0]}";
            }

            return string.Format("{0:n" + decimals + "} {1}", doubleFileLength, prefix[prefixNumber]);
        }

        /// <summary>
        /// Checks if the given mail address is a valid string.
        /// See also http://msdn.microsoft.com/en-us/library/01escwtf.aspx
        /// </summary>
        /// <param name="email">Email to be verified</param>
        /// <returns>true, if this is a valid mail</returns>
        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            // Use IdnMapping class to convert Unicode domain names. 
            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None);
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            // Return true if strIn is in valid e-mail format. 
            return Regex.IsMatch(email,
                  @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                  @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                  RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Internal conversion method for IDN Domains
        /// </summary>
        /// <param name="match">Match to be used</param>
        /// <returns>Converted domain</returns>
        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            var idn = new IdnMapping();

            var domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException();
            }
            return match.Groups[1].Value + domainName;
        }

        /// <summary>
        /// Returns a string, where the first letter is an upper letter, being transformed
        /// with 'ToUpper'. The rest of the text is not modified. 
        /// The input variable itself is also not modified.
        /// </summary>
        /// <param name="text">Text being converted</param>
        /// <returns>Converted text</returns>
        public static string ToUpperFirstLetter(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length == 1)
            {
                return text.ToUpper();
            }

            return $"{char.ToUpper(text[0])}{text.Substring(1)}";
        }
        
        /// <summary>
        /// Gets a deterministic hash code for strings
        /// Source by https://andrewlock.net/why-is-string-gethashcode-different-each-time-i-run-my-program-in-net-core/
        /// </summary>
        /// <param name="str">String to be used</param>
        /// <returns>Hashcode</returns>
        public static int GetDeterministicHashCode(this string str)
        {
            unchecked
            {
                var hash1 = (5381 << 16) + 5381;
                var hash2 = hash1;

                for (var i = 0; i < str.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }
    }
}