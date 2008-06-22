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
        /// <param name="oValue">Zu prüfendes Objekt</param>
        public static void IsNull(object oValue)
        {
            if (oValue != null)
            {
                throw new EnsureFailedException("IsNull");
            }
        }

        /// <summary>
        /// Versichert, dass das angegebene Objekt nicht null ist
        /// </summary>
        /// <param name="oValue">Zu prüfendes Objekt</param>
        public static void IsNotNull(object oValue)
        {
            if (oValue == null)
            {
                throw new EnsureFailedException("IsNotNull");
            }
        }

        /// <summary>
        /// Versichert, dass das angegebene Objekt nicht null ist
        /// </summary>
        /// <param name="oValue">Wert</param>
        /// <param name="strErrorText">Fehlertext</param>
        public static void IsNotNull(object oValue, String strErrorText)
        {
            if (oValue == null)
            {
                throw new EnsureFailedException("IsNotNull: " + strErrorText);
            }
        }

        /// <summary>
        /// Prüft, ob der angegebene Wert true ist
        /// </summary>
        /// <param name="bValue">Zu überprüfende Wert</param>
        public static void IsTrue(bool bValue)
        {
            if (!bValue)
            {
                throw new EnsureFailedException("IsTrue");
            }
        } 
        
        /// <summary>
        /// Prüft, ob der angegebene Wert true ist
        /// </summary>
        /// <param name="bValue">Zu überprüfende Wert</param>
        public static void IsTrue(bool bValue, String strText)
        {
            if (!bValue)
            {
                throw new EnsureFailedException("IsTrue: " + strText);
            }
        }

        /// <summary>
        /// Prüft, ob der angegebene Wert false ist
        /// </summary>
        /// <param name="bValue">Zu überprüfende Wert</param>
        public static void IsFalse(bool bValue)
        {
            if (bValue)
            {
                throw new EnsureFailedException("IsFalse");
            }
        }

        /// <summary>
        /// Prüft, ob der angegebene Wert false ist
        /// </summary>
        /// <param name="bValue">Zu überprüfende Wert</param>
        public static void IsFalse(bool bValue, String strText)
        {
            if (bValue)
            {
                throw new EnsureFailedException("IsFalse: " + strText);
            }
        }

        #region AreEqual
        
        /// <summary>
        /// Überprüft, ob die beiden Objekte gleich sind. Zum Vergleich
        /// wird der ==-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void AreEqual<T>(T oValue, T oReference) 
        {
            if (oValue.Equals(oReference))
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} == {1}", oValue.ToString(), oReference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob die beiden Objekte gleich sind. Zum Vergleich
        /// wird der ==-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void AreEqual<T>(T oValue, T oReference, String strText)
        {
            if (oValue.Equals(oReference))
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} == {1}: {2}", oValue.ToString(), oReference.ToString(), strText));
            }
        }

        #endregion

        #region AreNotEqual

        

        /// <summary>
        /// Überprüft, ob die beiden Objekte ungleich sind. Zum Vergleich
        /// wird der !=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void AreNotEqual<T>(T oValue, T oReference)
        {
            if (!oValue.Equals(oReference))
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} != {1}", oValue.ToString(), oReference.ToString()));
            }
        }

        #endregion

        #region IsGreater

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer der Referenz ist. Zum Vergleich
        /// wird der >-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsGreater(short oValue, short oReference)
        {
            if (oValue > oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} > {1}", oValue.ToString(), oReference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer der Referenz ist. Zum Vergleich
        /// wird der >-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsGreater(int oValue, int oReference)
        {
            if (oValue > oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} > {1}", oValue.ToString(), oReference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer der Referenz ist. Zum Vergleich
        /// wird der >-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsGreater(long oValue, long oReference)
        {
            if (oValue > oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} > {1}", oValue.ToString(), oReference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer der Referenz ist. Zum Vergleich
        /// wird der >-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsGreater(float oValue, float oReference)
        {
            if (oValue > oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} > {1}", oValue.ToString(), oReference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer der Referenz ist. Zum Vergleich
        /// wird der >-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsGreater(double oValue, double oReference)
        {
            if (oValue > oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} > {1}", oValue.ToString(), oReference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer der Referenz ist. Zum Vergleich
        /// wird der >-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsGreater(decimal oValue, decimal oReference)
        {
            if (oValue > oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} > {1}", oValue.ToString(), oReference.ToString()));
            }
        }

        #endregion

        #region IsGreaterOrEqual

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsGreaterOrEqual(short oValue, short oReference)
        {
            if (oValue >= oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} >= {1}", oValue.ToString(), oReference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsGreaterOrEqual(int oValue, int oReference)
        {
            if (oValue >= oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} >= {1}", oValue.ToString(), oReference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsGreaterOrEqual(long oValue, long oReference)
        {
            if (oValue >= oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} >= {1}", oValue.ToString(), oReference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsGreaterOrEqual(float oValue, float oReference)
        {
            if (oValue >= oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} >= {1}", oValue.ToString(), oReference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsGreaterOrEqual(double oValue, double oReference)
        {
            if (oValue >= oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} >= {1}", oValue.ToString(), oReference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsGreaterOrEqual(decimal oValue, decimal oReference)
        {
            if (oValue >= oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} >= {1}", oValue.ToString(), oReference.ToString()));
            }
        }

        #endregion

        #region IsSmaller

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsSmaller(short oValue, short oReference)
        {
            if (oValue < oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} < {1}", oValue.ToString(), oReference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsSmaller(int oValue, int oReference)
        {
            if (oValue < oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} < {1}", oValue.ToString(), oReference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsSmaller(long oValue, long oReference)
        {
            if (oValue < oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} < {1}", oValue.ToString(), oReference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsSmaller(float oValue, float oReference)
        {
            if (oValue < oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} < {1}", oValue.ToString(), oReference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsSmaller(double oValue, double oReference)
        {
            if (oValue < oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} < {1}", oValue.ToString(), oReference.ToString()));
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsSmaller(decimal oValue, decimal oReference)
        {
            if (oValue < oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} < {1}", oValue.ToString(), oReference.ToString()));
            }
        }

        #endregion

        #region IsSmallerOrEqual

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsSmallerOrEqual(short oValue, short oReference)
        {
            if (oValue <= oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} <= {1}", oValue.ToString(), oReference.ToString()));  
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsSmallerOrEqual(int oValue, int oReference)
        {
            if (oValue <= oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} <= {1}", oValue.ToString(), oReference.ToString()));  
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsSmallerOrEqual(long oValue, long oReference)
        {
            if (oValue <= oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} <= {1}", oValue.ToString(), oReference.ToString()));  
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsSmallerOrEqual(float oValue, float oReference)
        {
            if (oValue <= oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} <= {1}", oValue.ToString(), oReference.ToString()));  
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsSmallerOrEqual(double oValue, double oReference)
        {
            if (oValue <= oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} <= {1}", oValue.ToString(), oReference.ToString()));  
            }
        }

        /// <summary>
        /// Überprüft, ob das übergebene Objekt größer oder gleich der Referenz ist. 
        /// Zum Vergleich wird der >=-Operator genutzt.
        /// </summary>
        /// <typeparam name="T">Typ der zu vergleichenden Objekte</typeparam>
        /// <param name="oValue">Wert, der geprüft werden soll. </param>
        /// <param name="oReference">Wert, zu dem <c>oValue</c> gleich sein soll.</param>
        public static void IsSmallerOrEqual(decimal oValue, decimal oReference)
        {
            if (oValue <= oReference)
            {
                return;
            }
            else
            {
                throw new EnsureFailedException(
                    String.Format(
                        "{0} <= {1}", oValue.ToString(), oReference.ToString()));  
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
