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

using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace BurnSystems
{
    /// <summary>
    /// Eine Klasse, die ein paar Hilfsfunktionen für die mathematischen 
    /// Routinen zur Verfügung stellt. 
    /// </summary>
    public static class MathHelper
    {
        #region Threadsafe Implementation of random

        /// <summary>
        /// Threadsichere Klasse von Random
        /// </summary>
        class RandomThreadSafe : Random
        {
            object _SyncObject = new object();

            public override double NextDouble()
            {
                lock (_SyncObject)
                {
                    return base.NextDouble();
                }
            }
            public override int Next()
            {
                lock (_SyncObject)
                {
                    return base.Next();
                }
            }
            public override int Next(int maxValue)
            {
                lock (_SyncObject)
                {
                    return base.Next(maxValue);
                }
            }

            public override int Next(int minValue, int maxValue)
            {
                lock (_SyncObject)
                {
                    return base.Next(minValue, maxValue);
                }
            }
            public override void NextBytes(byte[] buffer)
            {
                lock (_SyncObject)
                {
                    base.NextBytes(buffer);
                }
            }
        }

        #endregion

        /// <summary>
        /// Eine Zufallsvariable
        /// </summary>
        static Random _Random = new RandomThreadSafe();

        public static Random Random
        {
            get { return _Random; }
        }

        /// <summary>
        /// Statischer Kontsruktor
        /// </summary>
        static MathHelper()
        {

        }

        /// <summary>
        /// Gibt das kleinere der beiden Timespans zurück
        /// </summary>
        /// <param name="o1">Das Timespan 1</param>
        /// <param name="o2">Das Timespan 2</param>
        /// <returns>Die kleinere Zeitspanne</returns>
        public static TimeSpan Min(TimeSpan o1, TimeSpan o2)
        {
            if (o1.TotalMilliseconds > o2.TotalMilliseconds)
            {
                return o2;
            }
            else
            {
                return o1;
            }
        }

        /// <summary>
        /// Gibt das kleinere der beiden Timespans zurück
        /// </summary>
        /// <param name="o1">Das Timespan 1</param>
        /// <param name="o2">Das Timespan 2</param>
        /// <returns>Die kleinere Zeitspanne</returns>
        public static TimeSpan Max(TimeSpan o1, TimeSpan o2)
        {
            if (o1.TotalMilliseconds < o2.TotalMilliseconds)
            {
                return o2;
            }
            else
            {
                return o1;
            }
        }

        /// <summary>
        /// Gibt den früheren der beiden Zeitpunkte zurück
        /// </summary>
        /// <param name="o1">Der erste Zeitpunkt 1</param>
        /// <param name="o2">Der zweite Zeitpunkt 2</param>
        /// <returns>Die früheren Zeitpunkt</returns>
        public static DateTime Min(DateTime o1, DateTime o2)
        {
            if (o1.Ticks > o2.Ticks)
            {
                return o2;
            }
            else
            {
                return o1;
            }
        }

        /// <summary>
        /// Gibt den spüteren der beiden Zeitpunkte zurück
        /// </summary>
        /// <param name="o1">Der erste Zeitpunkt 1</param>
        /// <param name="o2">Der zweite Zeitpunkt 2</param>
        /// <returns>Die spüteren Zeitpunkt</returns>
        public static DateTime Max(DateTime o1, DateTime o2)
        {
            if (o1.Ticks < o2.Ticks)
            {
                return o2;
            }
            else
            {
                return o1;
            }
        }

        /// <summary>
        /// Ermittelt das kleinste Objekt, das in der Auflistung übergeben wurde
        /// </summary>
        /// <typeparam name="T">Auflistung von Objekten</typeparam>
        /// <param name="aoObjects">Objekte, die zu vergleichen sind</param>
        /// <returns>Das kleisnte</returns>
        public static T Min<T>(IEnumerable<T> aoObjects) where T : IComparable<T>
        {
            bool bFirst = true;
            T oReturn = default(T);

            foreach (var oObject in aoObjects)
            {
                if (bFirst)
                {
                    oReturn = oObject;
                    bFirst = false;
                }
                else
                {
                    oReturn = (oReturn.CompareTo(oObject) == -1) ? oReturn : oObject;
                }
            }

            return oReturn;
        }

        /// <summary>
        /// Ermittelt das grüüte Objekt, das in der Auflistung übergeben wurde
        /// </summary>
        /// <typeparam name="T">Auflistung von Objekten</typeparam>
        /// <param name="aoObjects">Objekte, die zu vergleichen sind</param>
        /// <returns>Das kleisnte</returns>
        public static T Max<T>(IEnumerable<T> aoObjects) where T : IComparable<T>
        {
            bool bFirst = true;
            T oReturn = default(T);

            foreach (var oObject in aoObjects)
            {
                if (bFirst)
                {
                    oReturn = oObject;
                    bFirst = false;
                }
                else
                {
                    oReturn = (oReturn.CompareTo(oObject) == 1) ? oReturn : oObject;
                }
            }

            return oReturn;
        }

        /// <summary>
        /// Gibt eine zufüllige Zahl zurück
        /// </summary>
        /// <param name="dAverage">Erwarteter Durchschnitt</param>
        /// <param name="dVariance">Erwartete Varianz</param>
        /// <returns>Ermittelte Zufallszahl</returns>
        public static double GetNextGaussian(double dAverage, double dVariance)
        {
            while (true)
            {
                double d1 = Random.NextDouble();
                double d2 = Random.NextDouble();

                // Algorithmus aus http://de.wikipedia.org/wiki/Normalverteilung
                double dV = (2 * d1 - 1) * (2 * d1 - 1) + (2 * d2 - 1) * (2 * d2 - 1);
                if (dV >= 1)
                {
                    continue;
                }

                double dRandom = (2 * d1 - 1) * Math.Sqrt(-2 * Math.Log(dV) / dV);
                dRandom *= Math.Sqrt(dVariance);
                return dRandom + dAverage;                
            }
        }



        /// <summary>
        /// Formatiert einen Timespan so, dass er dem Format
        /// HH:MM:SS entspricht
        /// </summary>
        /// <param name="oTimeSpan">Zu formatierender Timespan</param>
        /// <returns>String mit dem oben genannten Format</returns>
        public static String FormatTimeSpan(TimeSpan oTimeSpan)
        {
            if (oTimeSpan > TimeSpan.MaxValue - TimeSpan.FromSeconds(2) ||
                oTimeSpan < TimeSpan.MinValue + TimeSpan.FromSeconds(2))
            {
                return LocalizationBS.TimeSpan_MaxValue;
            }
            int nTotalSeconds = (int) Math.Round(oTimeSpan.TotalSeconds);
            int nSeconds = nTotalSeconds % 60;
            int nMinutes = (nTotalSeconds / 60) % 60;
            int nHours = nTotalSeconds / 3600;

            return String.Format(CultureInfo.InvariantCulture,
                "{0}:{1}:{2}",
                MakeTwoDigits(nHours),
                MakeTwoDigits(nMinutes),
                MakeTwoDigits(nSeconds));
        }

        /// <summary>
        /// Gibt eine zwei- oder mehrstellige Ziffer.
        /// </summary>
        /// <param name="nDigit">Ziffer, die umgewandelt werden soll.</param>
        /// <returns>Zahl im Format XX.</returns>
        static String MakeTwoDigits(int nDigit)
        {
            if (nDigit < 10)
            {
                return String.Format(CultureInfo.InvariantCulture, "0{0}", nDigit);
            }
            return nDigit.ToString(CultureInfo.InvariantCulture);
        }
    }
}
