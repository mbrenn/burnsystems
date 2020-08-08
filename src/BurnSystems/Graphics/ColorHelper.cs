using System.Drawing;
using System.Globalization;
using BurnSystems.Test;

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
        public static string ColorToHex(Color color)
        {
            Ensure.IsNotNull(color);

            return string.Format(
                CultureInfo.InvariantCulture,
                "{0:X2}{1:X2}{2:X2}",
                color.R, 
                color.G, 
                color.B);
        }

        /// <summary>
        /// Converts a hexadecimal string like #FF00FF or FF00FF to a color
        /// </summary>
        /// <param name="hexValue">Hexadecimal string to be converted</param>
        /// <returns>Converted color</returns>
        public static Color HexToColor(string hexValue)
        {
            if (hexValue.Length == 0)
            {
                return Color.White;
            }

            var start = hexValue[0] == '#' ? 1 : 0;

            if (hexValue.Length != start + 6)
            {
                return Color.White;
            }

            var nR = hexValue.Substring(start, 2).HexToInt();
            var nG = hexValue.Substring(start + 2, 2).HexToInt();
            var nB = hexValue.Substring(start + 4, 2).HexToInt();
            return Color.FromArgb(255, nR, nG, nB);
        }
    }
}
