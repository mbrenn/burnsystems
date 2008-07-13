//-----------------------------------------------------------------------
// <copyright file="MathHelper.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Globalization;
    using BurnSystems.Test;

    /// <summary>
    /// Eine Klasse, die ein paar Hilfsfunktionen für die mathematischen 
    /// Routinen zur Verfügung stellt. 
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Eine Zufallsvariable
        /// </summary>
        private static Random random = new RandomThreadSafe();

        /// <summary>
        /// Statischer Kontsruktor
        /// </summary>
        static MathHelper()
        {
        }

        /// <summary>
        /// Gets a threadsafe random instance
        /// </summary>
        public static Random Random
        {
            get { return random; }
        }

        /// <summary>
        /// Gibt das kleinere der beiden Timespans zurück
        /// </summary>
        /// <param name="timeSpan1">Das Timespan 1</param>
        /// <param name="timeSpan2">Das Timespan 2</param>
        /// <returns>Die kleinere Zeitspanne</returns>
        public static TimeSpan Min(TimeSpan timeSpan1, TimeSpan timeSpan2)
        {
            if (timeSpan1.TotalMilliseconds > timeSpan2.TotalMilliseconds)
            {
                return timeSpan2;
            }
            else
            {
                return timeSpan1;
            }
        }

        /// <summary>
        /// Gibt das kleinere der beiden Timespans zurück
        /// </summary>
        /// <param name="timeSpan1">Das Timespan 1</param>
        /// <param name="timeSpan2">Das Timespan 2</param>
        /// <returns>Die kleinere Zeitspanne</returns>
        public static TimeSpan Max(TimeSpan timeSpan1, TimeSpan timeSpan2)
        {
            if (timeSpan1.TotalMilliseconds < timeSpan2.TotalMilliseconds)
            {
                return timeSpan2;
            }
            else
            {
                return timeSpan1;
            }
        }

        /// <summary>
        /// Gibt den früheren der beiden Zeitpunkte zurück
        /// </summary>
        /// <param name="dateTime1">Der erste Zeitpunkt 1</param>
        /// <param name="dateTime2">Der zweite Zeitpunkt 2</param>
        /// <returns>Die früheren Zeitpunkt</returns>
        public static DateTime Min(DateTime dateTime1, DateTime dateTime2)
        {
            if (dateTime1.Ticks > dateTime2.Ticks)
            {
                return dateTime2;
            }
            else
            {
                return dateTime1;
            }
        }

        /// <summary>
        /// Gibt den spüteren der beiden Zeitpunkte zurück
        /// </summary>
        /// <param name="dateTime1">Der erste Zeitpunkt 1</param>
        /// <param name="dateTime2">Der zweite Zeitpunkt 2</param>
        /// <returns>Die spüteren Zeitpunkt</returns>
        public static DateTime Max(DateTime dateTime1, DateTime dateTime2)
        {
            if (dateTime1.Ticks < dateTime2.Ticks)
            {
                return dateTime2;
            }
            else
            {
                return dateTime1;
            }
        }

        /// <summary>
        /// Ermittelt das kleinste Objekt, das in der Auflistung übergeben wurde
        /// </summary>
        /// <typeparam name="T">Auflistung von Objekten</typeparam>
        /// <param name="objects">Objekte, die zu vergleichen sind</param>
        /// <returns>Das kleinste Objekt in der Auflistung</returns>
        public static T Min<T>(IEnumerable<T> objects) where T : IComparable<T>
        {
            bool first = true;
            T result = default(T);

            foreach (var obj in objects)
            {
                if (first)
                {
                    result = obj;
                    first = false;
                }
                else
                {
                    result = (result.CompareTo(obj) == -1) ? result : obj;
                }
            }

            return result;
        }

        /// <summary>
        /// Ermittelt das grüüte Objekt, das in der Auflistung übergeben wurde
        /// </summary>
        /// <typeparam name="T">Auflistung von Objekten</typeparam>
        /// <param name="objects">Objekte, die zu vergleichen sind</param>
        /// <returns>The biggest object in enumeration</returns>
        public static T Max<T>(IEnumerable<T> objects) where T : IComparable<T>
        {
            bool first = true;
            T result = default(T);

            foreach (var obj in objects)
            {
                if (first)
                {
                    result = obj;
                    first = false;
                }
                else
                {
                    result = (result.CompareTo(obj) == 1) ? result : obj;
                }
            }

            return result;
        }

        /// <summary>
        /// Gibt eine zufüllige Zahl zurück
        /// </summary>
        /// <param name="average">Erwarteter Durchschnitt</param>
        /// <param name="variance">Erwartete Varianz</param>
        /// <returns>Ermittelte Zufallszahl</returns>
        public static double GetNextGaussian(double average, double variance)
        {
            while (true)
            {
                Ensure.IsGreaterOrEqual(variance, 0.0);
                double d1 = Random.NextDouble();
                double d2 = Random.NextDouble();

                // Algorithmus aus http://de.wikipedia.org/wiki/Normalverteilung
                double dV = (2 * d1 - 1) * (2 * d1 - 1) + (2 * d2 - 1) * (2 * d2 - 1);
                if (dV >= 1)
                {
                    continue;
                }

                double randomNumber = (2 * d1 - 1) * Math.Sqrt(-2 * Math.Log(dV) / dV);
                randomNumber *= Math.Sqrt(variance);
                return randomNumber + average;                
            }
        }
        
        /// <summary>
        /// Formatiert einen Timespan so, dass er dem Format
        /// HH:MM:SS entspricht
        /// </summary>
        /// <param name="timeSpan">Zu formatierender Timespan</param>
        /// <returns>String mit dem oben genannten Format</returns>
        public static string FormatTimeSpan(TimeSpan timeSpan)
        {
            if (timeSpan > TimeSpan.MaxValue - TimeSpan.FromSeconds(2) ||
                timeSpan < TimeSpan.MinValue + TimeSpan.FromSeconds(2))
            {
                return LocalizationBS.TimeSpan_MaxValue;
            }

            int totalSeconds = (int) Math.Round(timeSpan.TotalSeconds);
            int seconds = totalSeconds % 60;
            int minutes = (totalSeconds / 60) % 60;
            int hours = totalSeconds / 3600;

            return String.Format(
                CultureInfo.InvariantCulture,
                "{0}:{1}:{2}",
                MakeTwoDigits(hours),
                MakeTwoDigits(minutes),
                MakeTwoDigits(seconds));
        }

        /// <summary>
        /// Gibt eine zwei- oder mehrstellige Ziffer.
        /// </summary>
        /// <param name="digit">Ziffer, die umgewandelt werden soll.</param>
        /// <returns>Zahl im Format XX.</returns>
        private static string MakeTwoDigits(int digit)
        {
            if (digit < 10)
            {
                return string.Format(CultureInfo.InvariantCulture, "0{0}", digit);
            }

            return digit.ToString(CultureInfo.InvariantCulture);
        }
        
        #region Threadsafe Implementation of random

        /// <summary>
        /// Threadsichere Klasse von Random
        /// </summary>
        private class RandomThreadSafe : Random
        {
            /// <summary>
            /// Object for synchronisation
            /// </summary>
            private object syncObject = new object();

            /// <summary>
            /// Calls Random.NextDouble()
            /// </summary>
            /// <returns>Result of Call</returns>
            public override double NextDouble()
            {
                lock (this.syncObject)
                {
                    return base.NextDouble();
                }
            }

            /// <summary>
            /// Calls Random.Next()
            /// </summary>
            /// <returns>Result of Call</returns>
            public override int Next()
            {
                lock (this.syncObject)
                {
                    return base.Next();
                }
            }

            /// <summary>
            /// Calls Random.Next()
            /// </summary>
            /// <param name="maxValue">Parameter for Random.Next</param>
            /// <returns>Result of Call</returns>
            public override int Next(int maxValue)
            {
                lock (this.syncObject)
                {
                    return base.Next(maxValue);
                }
            }

            /// <summary>
            /// Calls Random.Next()            
            /// </summary>
            /// <param name="minValue">First Parameter for Random.Next</param>
            /// <param name="maxValue">Second Parameter for Random.Next</param>
            /// <returns>Result of Call</returns>
            public override int Next(int minValue, int maxValue)
            {
                lock (this.syncObject)
                {
                    return base.Next(minValue, maxValue);
                }
            }

            /// <summary>
            /// Calls Random.NextBytes()
            /// </summary>
            /// <param name="buffer">Parameter for buffer</param>
            public override void NextBytes(byte[] buffer)
            {
                lock (this.syncObject)
                {
                    base.NextBytes(buffer);
                }
            }
        }

        #endregion
    }
}
