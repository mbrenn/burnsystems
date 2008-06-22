//-----------------------------------------------------------------------
// <copyright file="FloatRectangle.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace BurnSystems.Graphics
{
    /// <summary>
    /// Eine Rechteckstruktur, bei dem Breite und Höhe in einer Fließkommazahl
    /// mit doppelter Genauigkeit gespeichert wird. 
    /// </summary>
    public class FloatRectangle
    {
        /// <summary>
        /// Linke Kante
        /// </summary>
        double _Left;

        /// <summary>
        /// Obere Kante
        /// </summary>
        double _Top;

        /// <summary>
        /// Breite des Rechtecks
        /// </summary>
        double _Width;

        /// <summary>
        /// Höhe des Rechtecks
        /// </summary>
        double _Height;

        /// <summary>
        /// Linker Wert
        /// </summary>
        public double Left
        {
            get { return _Left; }
            set { _Left = value; }
        }

        /// <summary>
        /// Oberer Wert
        /// </summary>
        public double Top
        {
            get { return _Top; }
            set { _Top = value; }
        }

        public double Right
        {
            get { return _Left + _Width; }
            set { _Width = value - _Left; }
        }

        /// <summary>
        /// Untere und obere Ecke
        /// </summary>
        public double Bottom
        {
            get { return _Top + _Height; }
            set { _Height = value - _Top; }
        }

        /// <summary>
        /// Breite des Rechtecks
        /// </summary>
        public double Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        /// <summary>
        /// Höhe des Rechtecks
        /// </summary>
        public double Height
        {
            get { return _Height; }
            set { _Height = value; }
        }

        /// <summary>
        /// Erstellt ein neues Rechteck
        /// </summary>
        public FloatRectangle()
        {
        }

        /// <summary>
        /// Erstellt ein neues Rechteck
        /// </summary>
        /// <param name="dWidth">Breite</param>
        /// <param name="dHeight">Höhe</param>
        public FloatRectangle(double dWidth, double dHeight)
        {
            _Left = 0;
            _Top = 0;
            _Width = dWidth;
            _Height = dHeight;
        }

        /// <summary>
        /// Erstellt ein neues Rechteck
        /// </summary>
        /// <param name="dLeft">Links</param>
        /// <param name="dTop">Oben</param>
        /// <param name="dWidth">Breite</param>
        /// <param name="dHeight">Höhe</param>
        public FloatRectangle(double dLeft, double dTop, double dWidth, double dHeight)
        {
            _Left = dLeft;
            _Top = dTop;
            _Width = dWidth;
            _Height = dHeight;
        }
    }
}
