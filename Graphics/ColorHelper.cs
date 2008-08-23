//-----------------------------------------------------------------------
// <copyright file="ColorHelper.cs" company="Martin Brenn">
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
using System.Drawing;
using System.Globalization;

namespace BurnSystems.Graphics
{
    /// <summary>
    /// This is a small helper class for converting colors to hex and back. 
    /// Further methods will be implemented in the future
    /// </summary>
    public static class ColorHelper
    {
        /// <summary>
        /// Wandelt eine Farbe in einen Hexwert um
        /// </summary>
        /// <param name="color">Color to be converted</param>
        /// <returns>Hexstring of color</returns>
        public static String ColorToHex(Color color)
        {
            return String.Format(
                CultureInfo.InvariantCulture,
                "{0:X2}{1:X2}{2:X2}",
                color.R, color.G, color.B);
        }

        /// <summary>
        /// Converts a hexadecimal string like #FF00FF or FF00FF to a color
        /// </summary>
        /// <param name="hexValue">Hexadecimal string to be converted</param>
        /// <returns>Converted color</returns>
        public static Color HexToColor(String hexValue)
        {
            if (hexValue.Length == 0)
            {
                return Color.White;
            }
            int nStart = hexValue[0] == '#' ? 1 : 0;

            if (hexValue.Length != nStart + 6)
            {
                return Color.White;
            }

            int nR = StringManipulation.HexToInt(hexValue.Substring(nStart, 2));
            int nG = StringManipulation.HexToInt(hexValue.Substring(nStart + 2, 2));
            int nB = StringManipulation.HexToInt(hexValue.Substring(nStart + 4, 2));
            return Color.FromArgb(255, nR, nG, nB);
        }
    }
}
