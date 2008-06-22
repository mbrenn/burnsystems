//-----------------------------------------------------------------------
// <copyright file="ListHelper.cs" company="Martin Brenn">
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

namespace BurnSystems.Collections
{
    /// <summary>
    /// Portiert den Delegaten <c>Action</c> aus System.Core zurück.
    /// </summary>
    public delegate void Function();

    /// <summary>
    /// Portiert den Delegat <c>Func</c> aus System.Core zurück
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="oParameter"></param>
    /// <returns></returns>
    public delegate TResult Function<TResult>();

    /// <summary>
    /// Portiert den Delegat <c>Func</c> aus System.Core zurück
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="oParameter"></param>
    /// <returns></returns>
    public delegate TResult Function<TResult, T>(T oParameter);

    /// <summary>
    /// Portiert den Delegat <c>Func</c> aus System.Core zurück
    /// </summary>
    /// <typeparam name="T1">Typ 1</typeparam>
    /// <typeparam name="T2">Typ 2</typeparam>    
    /// <returns></returns>
    public delegate TResult Function<TResult, T1, T2>(T1 o1, T2 o2);

    /// <summary>
    /// Diese Listenhelferklasse stellt ein paar Funktionen für die
    /// unterschiedlichen Listenklassen zur Verfügung. 
    /// </summary>
    public static class ListHelper
    {
        /// <summary>
        /// Es wird jedes Element in der Aufzählung durchgegangen und das Delegat
        /// mit diesem Element aufgerufen
        /// </summary>
        /// <typeparam name="T">Typ des Delegaten</typeparam>
        /// <param name="iList">Aufzählung mit den Elementen</param>
        /// <param name="oDelegate">Delegat, der aufgerufen wird</param>
        public static void ForEach<T>(IEnumerable<T> iList, Action<T> oDelegate)
        {
            foreach (T oObject in iList)
            {
                oDelegate(oObject);
            }
        }

        /// <summary>
        /// Überprüft, ob auf ein bestimmtes Element in der Liste das 
        /// Prädikat zutrifft
        /// </summary>
        /// <typeparam name="T">Typ der Elemente in der Liste</typeparam>
        /// <param name="iList">Liste</param>
        /// <param name="oPredicate">Prädikat, auf das jedes einzelne
        /// Element getestet wird. </param>
        /// <returns>true, wenn eines der Elemente zutrifft</returns>
        public static bool Exists<T>(IEnumerable<T> iList, Predicate<T> oPredicate)
        {
            foreach (var oElement in iList)
            {
                if (oPredicate(oElement))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gibt das erste Element zurück, dass das Prädikat erfüllt
        /// </summary>
        /// <typeparam name="T">Typ</typeparam>
        /// <param name="iList">List</param>
        /// <param name="oPredicate">Zu erfüllendes Prädikat</param>
        /// <returns>Gefundenes Objekt</returns>
        public static T Find<T>(IEnumerable<T> iList, Predicate<T> oPredicate)
        {
            foreach (T oObject in iList)
            {
                if (oPredicate(oObject))
                {
                    return oObject;
                }
            }
            return default(T);
        }

        /// <summary>
        /// Gibt eine Aufzählung aller Inhalte zurück, die das übergebene
        /// Prädikat erfüllen
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="iList">Liste mit den Elementen, die zu überprüfen sind</param>
        /// <param name="oPredicate">Gefordertes Prädikat</param>
        /// <returns>Aufzählung mit passenden Elementen</returns>
        public static IEnumerable<T> FindAll<T>(IEnumerable<T> iList, Predicate<T> oPredicate)
        {
            foreach (var oObject in iList)
            {
                if (oPredicate(oObject))
                {
                    yield return oObject;
                }
            }
        }

        /// <summary>
        /// Konvertiert ein aufzählbares Objekt eines bestimmten Types
        /// mit Hilfe eines Delegaten in einen anderen Typ.
        /// </summary>
        /// <typeparam name="TSource">Typ der Quellobjekte</typeparam>
        /// <typeparam name="TResult">Typ der Zielobjekt</typeparam>
        /// <param name="aoSource">Array mit den Quellelementen</param>
        /// <param name="oConverter">Konverter mit dem Resultat</param>
        /// <returns>Liste mit dem konvertierten Elementen</returns>
        public static IList<TResult> Convert<TSource, TResult>
            (IEnumerable<TSource> aoSource, Converter<TSource, TResult> oConverter)
        {
            var aoResult = new List<TResult>();

            foreach (var oSource in aoSource)
            {
                aoResult.Add(oConverter(oSource));
            }

            return aoResult;
        }

        /// <summary>
        /// Konvertiert ein aufzählbares Objekt eines bestimmten Types
        /// mit Hilfe eines Delegaten in einen anderen Typ.
        /// </summary>
        /// <typeparam name="TSource">Typ der Quellobjekte</typeparam>
        /// <typeparam name="TResult">Typ der Zielobjekt</typeparam>
        /// <param name="aoSource">Array mit den Quellelementen</param>
        /// <param name="oConverter">Konverter mit dem Resultat</param>
        /// <returns>Array mit dem konvertierten Elementen</returns>
        public static TResult[] ConvertToArray<TSource, TResult>
            (IEnumerable<TSource> aoSource, Converter<TSource, TResult> oConverter)
        {
            var aoResult = new List<TResult>();

            foreach (var oSource in aoSource)
            {
                aoResult.Add(oConverter(oSource));
            }

            return aoResult.ToArray();
        }

        /// <summary>
        /// Identifiziert Duplikate in der Liste und gibt nur die eindeutigen
        /// Elemente zurück. Dazu wird die Funktion 'Equals' genutzt. 
        /// </summary>
        /// <typeparam name="T">Typ der zu überprüfenden Objekte</typeparam>
        /// <param name="oSource">Quelle</param>
        /// <returns>Eindeutige Objekte</returns>
        public static IEnumerable<T> Distinct<T>
            (IEnumerable<T> oSource)
        {
            var aoFound = new List<T>();

            foreach (var oObject in oSource)
            {
                if (aoFound.IndexOf(oObject) == -1)
                {
                    // Noch nicht in der Liste, hinzufügen und zurückgeben
                    yield return oObject;
                    aoFound.Add(oObject);
                }
            }
        }

        /// <summary>
        /// Gibt die Anzahl der Elemente zurück, die ein bestimmtes
        /// Prädikat erfüllen
        /// </summary>
        /// <typeparam name="T">Typ der Elemente in der Aufzählung</typeparam>
        /// <param name="oSource">Elemente, die zu prüfen sind</param>
        /// <param name="oPredicate">Das zu erfüllende Prädikat</param>
        /// <returns>Anzahl der gefundenen Elemente</returns>
        public static int Count<T>(IEnumerable<T> oSource, Predicate<T> oPredicate)
        {
            var nReturn = 0;
            foreach (var oElement in oSource)
            {
                if (oPredicate(oElement))
                {
                    nReturn++;
                }
            }
            return nReturn;
        }

        /// <summary>
        /// Gibt die Summe der konvertierten Objekte zurück
        /// </summary>
        /// <typeparam name="T">Angefragter Typ</typeparam>
        /// <param name="oSource">Quellobjekt</param>
        /// <param name="oConverter">Konvertierungsdelegat</param>
        /// <returns>Summe der Rückgabewerte</returns>
        public static long Sum<T>(IEnumerable<T> oSource, Converter<T, long> oConverter)
        {
            var nReturn = 0L;

            foreach (var oElement in oSource)
            {
                nReturn += oConverter(oElement);
            }
            return nReturn;
        }

        /// <summary>
        /// Überprüft mit Hilfe des übergebenen Prädikats, ob sich das Objekt
        /// schon in der Liste befindet. Ist dies nicht der Fall, so
        /// wird das Objekt hinzugefügt
        /// </summary>
        /// <typeparam name="T">Typ des Objektes</typeparam>
        /// <param name="oList">Liste, zu der das Objekt hinzugefügt
        /// werden soll. </param>
        /// <param name="oToBeAdded">Objekt, das hinzugefügt werden soll.</param>
        /// <param name="oTest">Prädikat, mit dessen Hilfe getestet wird.</param>
        public static void AddIfNotExists<T>(IList<T> oList, T oToBeAdded, Function<bool, T, T> oTest)
        {
            foreach (var oObject in oList)
            {
                if (oTest(oToBeAdded, oObject))
                {
                    return;
                }
            }
            oList.Add(oToBeAdded);
        }

        /// <summary>
        /// Sortiert eine Liste mit Hilfe des übergebenen Comparers
        /// </summary>
        /// <typeparam name="T">Typ der in der Liste enthaltenen Elemente</typeparam>
        /// <param name="oList">Zu sortierende Liste</param>
        /// <param name="oComparer">Compaerer</param>
        public static IEnumerable<T> Sort<T>(IEnumerable<T> iList, Comparison<T> oComparer)
        {
            var oList = new List<T>(iList);
            oList.Sort(oComparer);
            return oList;
        }

        /// <summary>
        /// Entfernt alle Elemente aus der Liste <c>aiList</c>, die das 
        /// Prädikat <c>oPredicate</c> erfüllen.
        /// </summary>
        /// <typeparam name="T">Typspezifizierer</typeparam>
        /// <param name="aiList">Liste, die verändert werden soll</param>
        /// <param name="oPredicate">Prädikat</param>
        /// <returns>Anzahl der entfernten Elemente</returns>
        public static int Remove<T>(IList<T> aiList, Predicate<T> oPredicate)
        {
            var oList = new List<T>(aiList);
            var nPosition = 0;
            int nRemoved = 0;
            foreach (var oItem in oList)
            {
                if (oPredicate(oItem))
                {
                    aiList.RemoveAt(nPosition);
                    nRemoved++;
                }
                else
                {
                    nPosition++;
                }
            }
            return nRemoved;
        }

        /// <summary>
        /// Returns the position of the first occurance of <c>aoNeedle</c>
        /// in the Array <c>aoHayStick</c>. The method <c>Equals</c> is used
        /// for testing. 
        /// </summary>
        /// <typeparam name="T">Type of the elements.</typeparam>
        /// <param name="aoHayStick">Haystick</param>
        /// <param name="aoNeedle">Searched Needle</param>
        /// <param name="nStartPosition">StartPosition</param>
        /// <returns></returns>
        public static int IndexOf<T>(T[] aoHayStick, T[] aoNeedle, int nStartPosition)
        {
            // Do standard error checking here.
            if (aoHayStick == null)
            {
                throw new ArgumentNullException("aoHayStick");
            }
            if (aoNeedle == null)
            {
                throw new ArgumentNullException("aoNeedle");
            }

            // Found?
            bool bFound = false;

            // Cycle through each byte of the searched.  Do not search past
            // searched.Length - find.Length bytes, since it's impossible
            // for the value to be found at that point.
            for (var nIndex = nStartPosition; nIndex <= aoHayStick.Length - aoNeedle.Length; ++nIndex)
            {
                // Assume the values matched.
                bFound = true;

                // Search in the values to be found.
                for (var nSubIndex = 0L; nSubIndex < aoNeedle.Length; ++nSubIndex)
                {
                    // Check the value in the searched array vs the value
                    // in the find array.
                    if (!aoNeedle[nSubIndex].Equals(aoHayStick[nIndex + nSubIndex]))
                    {
                        // The values did not match.
                        bFound = false;

                        // Break out of the loop.
                        break;
                    }
                }

                // If the values matched, return the index.
                if (bFound)
                {
                    // Return the index.
                    return nIndex;
                }
            }

            // None of the values matched, return -1.
            return -1;
        }

        /// <summary>
        /// Stabile Sortierung mit Hilfe des Insertionsorts
        /// </summary>
        /// <typeparam name="T">Typ der zu sortierenden Elemente</typeparam>
        /// <param name="oList">Zu sortierende Liste</param>
        /// <param name="dComparison">Sortierfunktion</param>
        public static void InsertionSort<T>(IList<T> oList, Comparison<T> dComparison)
        {
            if (oList == null)
                throw new ArgumentNullException("list");
            if (dComparison == null)
                throw new ArgumentNullException("comparison");

            var count = oList.Count;
            for (int j = 1; j < count; j++)
            {
                var key = oList[j];

                var i = j - 1;
                for (; i >= 0 && dComparison(oList[i], key) > 0; i--)
                {
                    oList[i + 1] = oList[i];
                }
                oList[i + 1] = key;
            }
        }

        /// <summary>
        /// Kopiert ein zweidimensionales Array
        /// </summary>
        /// <typeparam name="T">Typ der Elemente in dem Array</typeparam>
        /// <param name="aoArray">Zu kopierendes Array</param>
        /// <returns>Kopiertes Array</returns>
        public static T[,] Copy<T>(T[,] aoArray)
        {
            int nHeight = aoArray.GetLength(0);
            int nWidth = aoArray.GetLength(1);

            var aoReturn = new T[nHeight, nWidth];
            for (int n = 0; n < nHeight; n++)
            {
                for (int m = 0; m < nWidth; m++)
                {
                    aoReturn[n, m] = aoArray[n, m];
                }
            }
            return aoReturn;
        }
    }
}
