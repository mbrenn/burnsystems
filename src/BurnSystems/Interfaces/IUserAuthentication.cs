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
        /// <param name="userName">Username of credentials</param>
        /// <param name="password">Encrypted or unencrypted password</param>
        /// <param name="encryted">Flag, ob das Kennwort schon 
        /// verschlüsselt ist</param>
        /// <returns>true, wenn dies der Fall ist</returns>
        bool AreCredentialsOk(string userName, string password, bool encryted);
    }
}
