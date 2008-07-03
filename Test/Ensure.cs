//-----------------------------------------------------------------------
// <copyright file="Ensure.cs" company="Martin Brenn">
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
using BurnSystems.Collections;

namespace BurnSystems.Test
{
    /// <summary>
    /// Mit Hilfe dieser Hilfsklasse kann überprüft werden, ob 
    /// ein bestimmter Zustand eingehalten wird. Im Prinzip entspricht 
    /// diese Klasse der Debug.Assert-Funktionalität, ist nur um einiges
    /// flexibler. 
    /// 
    /// Scheitert eine Abfrage, so wird eine EnsureFailedException geworfen. 
    /// </summary>
    public static class Ensure
    {
        /// <summary>
        /// Wirft die 'EnsureFailedException'-Ausnahme
        /// </summary>
        public static void Fail()
        {
            throw new EnsureFailedException("Fail");
        }

        /// <summary>
        /// Wirft die 'EnsureFailedException'-Ausnahme
        /// </summary>
        public static void Fail(String strFailText)
        {
            throw new EnsureFailedException("Fail: " + strFailText);
        }

        /// <summary>
        /// Versichert, dass das angegebene Objekt null ist. 
        /// </summary>
        /// <param name="value">Zu prüfendes Objekt</param>
        public static void IsNull(object value)
        {
            if (value != null)
            {
                throw new EnsureFailedException("IsNull");
            }
        }

        /// <summary>
        /// Versichert, dass das angegebene Objekt nicht null ist
        /// </summary>
        /// <param name="value">Zu prüfendes Objekt</param>
        public static void IsNotNull(object value)
        {
            if (value == null)
            {
                throw new EnsureFailedException("IsNotNull");
            }
        }

        /// <summary>
        /// Versichert, dass das angegebene Objekt nicht null ist
        /// </summary>
        /// <param name="value">Wert</param>
        /// <param name="errorText">Fehlertext</param>
        public static void IsNotNull(object value, String errorText)
        {
            if (value == null)
            {
                throw new EnsureFailedException("IsNotNull: " + errorText);
            }
        }

        /// <summary>
        /// Prüft, ob der angegebene Wert true ist
        /// </summary>
        /// <param name="value">Zu überprüfende Wert</param>
        public static void IsTrue(bool value)
        {
            if (!value)
            {
                throw new EnsureFailedException("IsTrue");
            }
        } 
        
        /// <summary>
        /// Prüft, ob der angegebene Wert true ist
        /// </summary>
        /// <param name="value">Zu überprüfende Wert</param>
        public static void IsTrue(bool value, String errorText)
        {
            if (!value)
            {
                throw new EnsureFailedException("IsTrue: " + errorText);
            }
        }

        /// <summary>
        /// Prüft, ob der angegebene Wert false ist
        /// </summary>
        /// <param name="value">Zu überprüfende Wert</param>
        public static void IsFalse(bool value)
        {
            if (value)
            {
                throw new EnsureFailedException("IsFalse");
            }
        }

        /// <summary>
        /// Prüft, ob der angegebene Wert false ist
        /// </summary>
        /// <param name="value">Zu überprüfende Wert</param>
        public static void IsFalse(bool value, String errorText)
        {
            if (value)
            {
                throw new EnsureFailedException("IsFalse: " + errorText);
            }
        }

        #region AreEqual
        
        /// <summary>
        /// Überprüft, ob die beiden Objekte gleich sind. Zum Vergleich
        /// wird der ==-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void AreEqual<T>(T value, T reference) 
        {
            if (value.Equals(reference))
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} == {1}", value.ToString(), reference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob die beiden Objekte gleich sind. Zum Vergleich
        /// wird der ==-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void AreEqual<T>(T value, T reference, String strText)
        {
            if (value.Equals(reference))
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} == {1}: {2}", value.ToString(), reference.ToString(), strText));
            }
        }

        #endregion

        #region AreNotEqual

        

        /// <summary>
        /// Überprüft, ob die beiden Objekte ungleich sind. Zum Vergleich
        /// wird der !=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void AreNotEqual<T>(T value, T reference)
        {
            if (!value.Equals(reference))
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} != {1}", value.ToString(), reference.ToString()));
            }
        }

        #endregion

        #region IsGreater

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer der Referenz ist. Zum Vergleich
        /// wird der >-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreater(short value, short reference)
        {
            if (value > reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} > {1}", value.ToString(), reference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer der Referenz ist. Zum Vergleich
        /// wird der >-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreater(int value, int reference)
        {
            if (value > reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} > {1}", value.ToString(), reference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer der Referenz ist. Zum Vergleich
        /// wird der >-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreater(long value, long reference)
        {
            if (value > reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} > {1}", value.ToString(), reference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer der Referenz ist. Zum Vergleich
        /// wird der >-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreater(float value, float reference)
        {
            if (value > reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} > {1}", value.ToString(), reference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer der Referenz ist. Zum Vergleich
        /// wird der >-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreater(double value, double reference)
        {
            if (value > reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} > {1}", value.ToString(), reference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer der Referenz ist. Zum Vergleich
        /// wird der >-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreater(decimal value, decimal reference)
        {
            if (value > reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} > {1}", value.ToString(), reference.ToString()));
            }
        }

        #endregion

        #region IsGreaterOrEqual

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreaterOrEqual(short value, short reference)
        {
            if (value >= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} >= {1}", value.ToString(), reference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreaterOrEqual(int value, int reference)
        {
            if (value >= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} >= {1}", value.ToString(), reference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreaterOrEqual(long value, long reference)
        {
            if (value >= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} >= {1}", value.ToString(), reference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreaterOrEqual(float value, float reference)
        {
            if (value >= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} >= {1}", value.ToString(), reference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreaterOrEqual(double value, double reference)
        {
            if (value >= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} >= {1}", value.ToString(), reference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsGreaterOrEqual(decimal value, decimal reference)
        {
            if (value >= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} >= {1}", value.ToString(), reference.ToString()));
            }
        }

        #endregion

        #region IsSmaller

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmaller(short value, short reference)
        {
            if (value < reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} < {1}", value.ToString(), reference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmaller(int value, int reference)
        {
            if (value < reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} < {1}", value.ToString(), reference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmaller(long value, long reference)
        {
            if (value < reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} < {1}", value.ToString(), reference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmaller(float value, float reference)
        {
            if (value < reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} < {1}", value.ToString(), reference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmaller(double value, double reference)
        {
            if (value < reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} < {1}", value.ToString(), reference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmaller(decimal value, decimal reference)
        {
            if (value < reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} < {1}", value.ToString(), reference.ToString()));
            }
        }

        #endregion

        #region IsSmallerOrEqual

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmallerOrEqual(short value, short reference)
        {
            if (value <= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} <= {1}", value.ToString(), reference.ToString()));  
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmallerOrEqual(int value, int reference)
        {
            if (value <= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} <= {1}", value.ToString(), reference.ToString()));  
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmallerOrEqual(long value, long reference)
        {
            if (value <= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} <= {1}", value.ToString(), reference.ToString()));  
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmallerOrEqual(float value, float reference)
        {
            if (value <= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} <= {1}", value.ToString(), reference.ToString()));  
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmallerOrEqual(double value, double reference)
        {
            if (value <= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} <= {1}", value.ToString(), reference.ToString()));  
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="value">Wert, der geprüft werden soll. </param>
        /// <param name="reference">Wert, zu dem <c>value</c> gleich sein soll.</param>
        public static void IsSmallerOrEqual(decimal value, decimal reference)
        {
            if (value <= reference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} <= {1}", value.ToString(), reference.ToString()));  
            }
        }

        #endregion

        /// <summary>
        /// Versichert, dass eine Ausnahme geworfen wird
        /// </summary>
        /// <typeparam name="T">Typ der zu werfenden Ausnahme</typeparam>
        public static void ThrowsException<T>(Function dFunction) where T : Exception
        {
            try
            {
                dFunction();
            }
            catch (T)
            {
                // Alles OK
                return;
            }
            throw new EnsureFailedException("ThrowsException");
        }
    }
}
