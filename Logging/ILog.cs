//-----------------------------------------------------------------------
// <copyright file="ILog.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

using System;
namespace BurnSystems.Logging
{
    /// <summary>
    /// Ein sehr schmales Interface, das eine Logging-Schnittstelle zur
    /// Verfügung stellt. Über dieses Interface können keine neuen Log-Provider
    /// zur Verfügung gestellt werden. 
    /// </summary>
    public interface ILog : IDisposable
    {
        /// <summary>
        /// Fügt einen neuen Logeintrag zur Datenbank hinzu
        /// </summary>
        /// <param name="oEntry">Logeintrag</param>
        void LogEntry(LogEntry oEntry);
    }
}
