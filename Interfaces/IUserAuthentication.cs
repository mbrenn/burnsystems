//-----------------------------------------------------------------------
// <copyright file="IUserAuthentication.cs" company="Martin Brenn">
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

namespace BurnSystems.Interfaces
{
    /// <summary>
    /// Dieses sehr einfache Interface muss von allen Klassen implementiert
    /// werden, die auf irgendeine Art und Weise eine Benutzer->Kennwort-
    /// Zuordnung zur Verfügung stellen
    /// </summary>
    public interface IUserAuthentication
    {
        /// <summary>
        /// überprüft ob der Benutzername und das Kennwort zueinander
        /// zusammen passen
        /// </summary>
        /// <param name="userName">Benutzername</param>
        /// <param name="password">Kennwort</param>
        /// <param name="encryted">Flag, ob das Kennwort schon 
        /// verschlüsselt ist</param>
        /// <returns>true, wenn dies der Fall ist</returns>
        bool AreCredentialsOk(String userName, String password, bool encryted);
    }
}
