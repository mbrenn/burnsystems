//-----------------------------------------------------------------------
// <copyright file="Pair.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Globalization;

    /// <summary>
    /// Stores a pair of two object
    /// </summary>
    /// <typeparam name="TFirst">Datatype of first value</typeparam>
    /// <typeparam name="TSecond">Datatype of second value</typeparam>
    public class Pair<TFirst, TSecond>
    {
        /// <summary>
        /// First value
        /// </summary>
        private TFirst first;

        /// <summary>
        /// Second value
        /// </summary>
        private TSecond second;

        /// <summary>
        /// Creates a new instance and stores the two values
        /// </summary>
        /// <param name="first">First value to be stored</param>
        /// <param name="second">Second value to be stored</param>
        public Pair(TFirst first, TSecond second)
        {
            this.first = first;
            this.second = second;
        }

        /// <summary>
        /// Gets or sets the first value
        /// </summary>
        public TFirst First
        {
            get { return this.first; }
            set { this.first = value; }
        }

        /// <summary>
        /// Gets or sets the second value.
        /// </summary>
        public TSecond Second
        {
            get { return this.second; }
            set { this.second = value; }
        }

        /// <summary>
        /// Converts the pair to a string
        /// </summary>
        /// <returns>The pair converted to a string</returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.CurrentUICulture,
                "{0}, {1}", 
                this.first.ToString(), 
                this.second.ToString());
        }
    }
}
