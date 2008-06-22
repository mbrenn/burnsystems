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

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;

namespace BurnSystems
{
    /// <summary>
    /// Delegat, der für die Funktion <c>Join</c> genutzt wird
    /// </summary>
    /// <typeparam name="T">Datentyp, der für jede Schleife übergeben werden soll. </typeparam>
    /// <param name="oItem">Jeweiliges Objekt, aus dem ein Eintrag erzeugt werden soll. </param>
    /// <returns>Ergebnis</returns>
    public delegate String JoinDelegate<T>(T oItem);

    /// <summary>
    /// Basisklasse, die einige Stringmanipulationen durchführt
    /// </summary>
    public static class StringManipulation
    {
        /// <summary>
        /// Inserts backslash before Single-Quote, Double-Quote and Backslash
        /// </summary>
        /// <param name="strValue">Input string</param>
        /// <returns>Output string with additional backslashes</returns>
        public static String AddSlashes(String strValue)
        {
            String strResult = strValue.Replace("\\", "\\\\");
            strResult = strResult.Replace("\"", "\\\"");
            return strResult = strResult.Replace("\'", "\\\'");
        }

        /// <summary>
        /// Strips backslashes before Single-Quote, Double-Quote and Backslash
        /// strValue = StripSlashes ( AddSlashes ( strValue ) );
        /// </summary>
        /// <param name="strValue">String with backslashes</param>
        /// <returns>String with less backslashes</returns>
        public static String StripSlashes(String strValue)
        {
            String strResult = strValue.Replace("\\\"", "\"");
            strResult = strResult.Replace("\\\'", "\'");
            return strResult.Replace("\\\\", "\\");
        }

        /// <summary>
        /// This method is equivalent to HttpUtility, except the encoding of
        /// the slash '/'. 
        /// </summary>
        /// <param name="strValue">String to be encoded</param>
        /// <returns>Encoded string</returns>
        public static String UrlEncode(String strValue)
        {
            return System.Web.HttpUtility.UrlEncode(strValue).Replace("%2f", "/")
                .Replace("%3a", ":");
        }

        /// <summary>
        /// hexDigits
        /// </summary>
        static char[] hexDigits = {
				'0', '1', '2', '3', '4', '5', '6', '7',
				'8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        /// <summary>
        /// Converts array of bytes to HexString
        /// </summary>
        /// <param name="bytes">Array of bytes</param>
        /// <returns>Hexstring</returns>
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
        /// <param name="strHexString">Hexadecimal string</param>
        /// <returns>Integer</returns>
        public static int HexToInt(String strHexString)
        {
            var nResult = 0;
            for (var nCounter = 0; nCounter < strHexString.Length; nCounter++)
            {
                var cCurrent = strHexString[nCounter];
                var nCurrent = 0;
                switch (cCurrent)
                {
                    case '0':
                        nCurrent = 0;
                        break;
                    case '1':
                        nCurrent = 1;
                        break;
                    case '2':
                        nCurrent = 2;
                        break;
                    case '3':
                        nCurrent = 3;
                        break;
                    case '4':
                        nCurrent = 4;
                        break;
                    case '5':
                        nCurrent = 5;
                        break;
                    case '6':
                        nCurrent = 6;
                        break;
                    case '7':
                        nCurrent = 7;
                        break;
                    case '8':
                        nCurrent = 8;
                        break;
                    case '9':
                        nCurrent = 9;
                        break;
                    case 'a':
                        nCurrent = 10;
                        break;
                    case 'A':
                        nCurrent = 10;
                        break;
                    case 'b':
                        nCurrent = 11;
                        break;
                    case 'B':
                        nCurrent = 11;
                        break;
                    case 'c':
                        nCurrent = 12;
                        break;
                    case 'C':
                        nCurrent = 12;
                        break;
                    case 'd':
                        nCurrent = 13;
                        break;
                    case 'D':
                        nCurrent = 13;
                        break;
                    case 'e':
                        nCurrent = 14;
                        break;
                    case 'E':
                        nCurrent = 14;
                        break;
                    case 'f':
                        nCurrent = 15;
                        break;
                    case 'F':
                        nCurrent = 15;
                        break;
                }

                nResult *= 16;
                nResult += nCurrent;
            }

            return nResult;
        }

        /// <summary>
        /// Shortens string to a specific length and adds ellipsis, if they are
        /// necessary. 
        /// </summary>
        /// <param name="strValue">String to be shortened</param>
        /// <param name="nMaxLength">Maximum length of string after calling this method. The ellipses
        /// is not counted</param>
        /// <returns>Shortened string</returns>
        public static String ShortenString(String strValue, int nMaxLength)
        {
            return ShortenString(strValue, nMaxLength, "...");
        }

        /// <summary>
        /// Shortens string to a specific length and adds ellipsis, if they are
        /// necessary. 
        /// </summary>
        /// <param name="strValue">String to be shortened</param>
        /// <param name="nMaxLength">Maximum length of string after calling this method. The ellipses
        /// is not counted</param>
        /// <param name="strEllipsis">Die genutzte Ellipsis, wenn der String zu lang ist.</param>
        /// <returns>Shortened string</returns>
        public static string ShortenString(string strValue, int nMaxLength, string strEllipsis)
        {
            if (nMaxLength < 0)
            {
                throw new ArgumentException("nMaxLength < 0");
            }
            if (strValue.Length > nMaxLength)
            {
                return strValue.Substring(0, nMaxLength) + strEllipsis;
            }
            return strValue;
        }

        /// <summary>
        /// Gets the first <c>nLetters</c> of <c>strValue</c> or the complete string,
        /// if length of string is smaller than <c>nLetters</c>.
        /// </summary>
        /// <param name="strValue">String to be shortened</param>
        /// <param name="nLetters">Number of letters</param>
        /// <returns>Shortened string</returns>
        public static String FirstLetters(String strValue, int nLetters)
        {
            if (strValue.Length > nLetters)
            {
                return strValue.Substring(0, nLetters);
            }
            return strValue;
        }

        /// <summary>
        /// Hashes String with Sha1-Algorithm
        /// </summary>
        /// <param name="strData">Data to be hashed</param>
        /// <returns>Hash</returns>
        public static String Sha1(String strData)
        {
            if (strData == null)
            {
                return null;
            }
            var oSHA1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            var oUtf8 = new System.Text.UTF8Encoding();

            var abBytes = oUtf8.GetBytes(strData);
            byte[] abResult;
            abResult = oSHA1.ComputeHash(abBytes);

            return ToHexString(abResult);
        }

        /// <summary>
        /// Gets content of a textfile 
        /// </summary>
        /// <param name="strFilename">Filename</param>
        /// <returns>Content of file</returns>
        /// <exception cref="FileNotFoundException">This exception is thrown, when
        /// the file was not found</exception>
        [Obsolete("Use System.IO.File.ReadAllText")]
        public static String GetTextfileContent(String strFilename)
        {
            return File.ReadAllText(strFilename);
        }

        /// <summary>
        /// Creates a simple random string, containing of <c>nLength</c> letters
        /// </summary>
        /// <param name="nLength">Length</param>
        /// <returns>Random string</returns>
        public static String RandomString(int nLength)
        {
            String strPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder oBuilder = new StringBuilder();
            Random oRandom = new Random();

            for (int nCounter = 0; nCounter < nLength; nCounter++)
            {
                oBuilder.Append(strPool[oRandom.Next(0, strPool.Length)]);
            }

            return oBuilder.ToString();
        }

        /// <summary>
        /// Converts newline to br-Tags. The br-Tags are XHtml-conform.
        /// </summary>
        /// <param name="strValue">String to be converted</param>
        /// <returns>String to be converted</returns>
        public static string Nl2Br(String strValue)
        {
            return strValue.Replace("\n", "<br />").Replace("\r", "");
        }

        /// <summary>
        /// Fügt an das Ende des Strings solange Zeichen von <c>cPaddingChar</c>
        /// hinzu bis der String die Länge <c>nLength</c> erreicht.
        /// </summary>
        /// <param name="strValue">String, der geändert werden soll</param>
        /// <param name="nLength">Länge des Strings</param>
        /// <param name="cPaddingChar">Zeichen, das hinzugefügt werden soll. </param>
        /// <returns>Aufgefüllter String. Ist der String länger als <c>nLength</c>,
        /// so wird ein gekürzter String zurückgegeben. </returns>
        public static String PaddingRight(String strValue, int nLength, char cPaddingChar)
        {
            int nStringLength = strValue.Length;

            if (nStringLength > nLength)
            {
                return strValue.Substring(0, nLength);
            }
            if (nStringLength == nLength)
            {
                return strValue;
            }
            StringBuilder oBuilder = new StringBuilder(strValue, nLength);
            while (nStringLength < nLength)
            {
                oBuilder.Append(cPaddingChar);
                nStringLength++;
            }
            return oBuilder.ToString();
        }

        /// <summary>
        /// Fügt an das Ende des Strings solange Leerzeichen
        /// hinzu bis der String die Länge <c>nLength</c> erreicht.
        /// </summary>
        /// <param name="strValue">String, der geändert werden soll</param>
        /// <param name="nLength">Länge des Strings</param>
        /// <returns>Aufgefüllter String. Ist der String länger als <c>nLength</c>,
        /// so wird ein gekürzter String zurückgegeben. </returns>
        public static String PaddingRight(String strValue, int nLength)
        {
            return PaddingRight(strValue, nLength, ' ');
        }


        /// <summary>
        /// Fügt an den Anfang des Strings solange Zeichen von <c>cPaddingChar</c>
        /// hinzu bis der String die Länge <c>nLength</c> erreicht.
        /// </summary>
        /// <param name="strValue">String, der geändert werden soll</param>
        /// <param name="nLength">Länge des Strings</param>
        /// <param name="cPaddingChar">Zeichen, das hinzugefügt werden soll. </param>
        /// <returns>Aufgefüllter String. Ist der String länger als <c>nLength</c>,
        /// so wird ein gekürzter String zurückgegeben. </returns>
        public static String PaddingLeft(String strValue, int nLength, char cPaddingChar)
        {
            int nStringLength = strValue.Length;

            if (nStringLength > nLength)
            {
                return strValue.Substring(0, nLength);
            }
            if (nStringLength == nLength)
            {
                return strValue;
            }
            StringBuilder oBuilder = new StringBuilder(nLength);
            while (nStringLength < nLength)
            {
                oBuilder.Append(cPaddingChar);
                nStringLength++;
            }

            oBuilder.Append(strValue);
            return oBuilder.ToString();
        }

        /// <summary>
        /// Fügt an den Anfang des Strings solange Leerzeichen
        /// hinzu bis der String die Länge <c>nLength</c> erreicht.
        /// </summary>
        /// <param name="strValue">String, der geändert werden soll</param>
        /// <param name="nLength">Länge des Strings</param>
        /// <returns>Aufgefüllter String. Ist der String länger als <c>nLength</c>,
        /// so wird ein gekürzter String zurückgegeben. </returns>
        public static String PaddingLeft(String strValue, int nLength)
        {
            return PaddingLeft(strValue, nLength, ' ');
        }

        /// <summary>
        /// Bricht eine Zeichenkette nach einer bestimmten Anzahl von Zeichen
        /// um, so dass kein Wort getrennt wird. 
        /// </summary>
        /// <param name="strValue">String</param>
        /// <param name="nLineLength">Länge der Zeile</param>
        /// <returns>Array mit Stringinhalten</returns>
        public static String[] WordWrap(String strValue, int nLineLength)
        {
            List<String> astrLines = new List<string>();

            int nCurrentPos = 0;
            int nStart = 0;

            while (true)
            {
                int nIndex = strValue.IndexOfAny(new char[] { ' ', '\r', '\n' },
                    nCurrentPos);

                if (nIndex == -1)
                {
                    while (strValue.Length - nStart >= nLineLength)
                    {
                        if (nCurrentPos == nStart)
                        {
                            AddLineToArray(astrLines,
                                strValue.Substring(nStart, nLineLength).Trim());
                            nCurrentPos += nLineLength;
                            nStart = nCurrentPos;
                        }
                        else
                        {
                            AddLineToArray(astrLines,
                                strValue.Substring(nStart, nCurrentPos - nStart).Trim());
                            nStart = nCurrentPos;
                        }
                    }
                    astrLines.Add(strValue.Substring(nStart).Trim());
                    break;
                }

                if (nIndex - nStart >= nLineLength)
                {
                    if (nCurrentPos == nStart)
                    {
                        AddLineToArray(astrLines,
                            strValue.Substring(nStart, nLineLength).Trim());
                        nCurrentPos += nLineLength;
                        nStart = nCurrentPos;
                    }
                    else
                    {
                        AddLineToArray(astrLines,
                            strValue.Substring(nStart, nCurrentPos - nStart).Trim());
                        nStart = nCurrentPos;
                    }
                }
                else
                {
                    nCurrentPos = nIndex + 1;
                }
            }

            return astrLines.ToArray();
        }

        /// <summary>
        /// Fügt ein Element zum übergebenen Listenobjekt hinzu, wenn
        /// dieses nicht null oder Leer ist. 
        /// </summary>
        /// <param name="astrList">Liste, die das Element erhalten soll</param>
        /// <param name="strLine">Objekt</param>
        static void AddLineToArray(IList<String> astrList, String strLine)
        {
            if (!String.IsNullOrEmpty(strLine))
            {
                astrList.Add(strLine);
            }
        }

        /// <summary>
        /// Erzeugt mit Hilfe eines Delegaten einen String, dessen einzelne Bestandteile
        /// durch einen Trenner unterteilt sind. 
        /// </summary>
        /// <typeparam name="T">Typ des Objektes aus dem die einzelnen Bestandteile
        /// erzeugt werden sollen</typeparam>
        /// <param name="aoElements">Elemente</param>
        /// <param name="oDelegate">Delegat, der die Bestandteile aus dem Objekttyp <c>T</c> 
        /// erzeugt.</param>
        /// <param name="strSeparator">Trenner, im Regelfall wird dieser ', ' sein. </param>
        /// <returns>Ergebnisstring</returns>
        public static String Join<T>(IEnumerable<T> aoElements, JoinDelegate<T> oDelegate, String strSeparator)
        {
            StringBuilder oResult = new StringBuilder();
            bool bFirst = true;

            foreach (var oElement in aoElements)
            {
                if (!bFirst)
                {
                    oResult.Append(strSeparator);
                }
                oResult.Append(oDelegate(oElement));
                bFirst = false;
            }
            return oResult.ToString();
        }
        
        /// <summary>
        /// Fügt eine Auflistung von Strings mit einem Seperator zusammen
        /// </summary>
        /// <param name="aiList">Liste von Elementen</param>
        /// <param name="strSeparator">Zwischenstück</param>
        /// <returns>Zusammengesetzter String</returns>
        public static string Join(IEnumerable<String> UnitTypes, string strSeperator)
        {
            return Join(UnitTypes, x => x, strSeperator);
        }

        /// <summary>
        /// Entfernt alle Zeichen aus dem Dateinamen, die ungültig sind 
        /// </summary>
        /// <param name="strFilename">Dateiname</param>
        /// <returns>Gesäuberter String</returns>
        public static String RemoveInvalidFilenameChars(String strFilename)
        {
            foreach (var cChar in Path.GetInvalidFileNameChars())
            {
                strFilename = strFilename.Replace(new String(cChar, 1), ((int)cChar).ToString());
            }
            return strFilename;
        }
    }
}
