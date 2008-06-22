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
        /// <param name="oColor"></param>
        /// <returns></returns>
        public static String ColorToHex(Color oColor)
        {
            return String.Format(
                CultureInfo.InvariantCulture,
                "{0:X2}{1:X2}{2:X2}",
                oColor.R, oColor.G, oColor.B);
        }

        /// <summary>
        /// Converts a hexadecimal string like #FF00FF or FF00FF to a color
        /// </summary>
        /// <param name="strHexString"></param>
        /// <returns></returns>
        public static Color HexToColor(String strHexValue)
        {
            if ( strHexValue.Length == 0 )
            {
                return Color.White;
            }
            int nStart = strHexValue[0] == '#' ? 1 : 0;

            if ( strHexValue.Length != nStart + 6 )
            {
                return Color.White;
            }

            int nR = StringManipulation.HexToInt(strHexValue.Substring(nStart, 2));
            int nG = StringManipulation.HexToInt(strHexValue.Substring(nStart + 2, 2));
            int nB = StringManipulation.HexToInt(strHexValue.Substring(nStart + 4, 2));
            return Color.FromArgb(255, nR, nG, nB);
        }
    }
}
