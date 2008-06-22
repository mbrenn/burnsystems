//-----------------------------------------------------------------------
// <copyright file="LogCreator.cs" company="Martin Brenn">
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
using System.Xml;
using System.Globalization;

namespace BurnSystems.Logging
{
    /// <summary>
    /// Dies ist eine Klasse mit dessen Hilfe 
    /// ein Log-Objekt über einen Xml-Knoten erzeugt werden kann. 
    /// </summary>
    public static class LogCreator
    {
        /// <summary>
        /// Erzeugt ein neues Log
        /// </summary>
        /// <param name="xmlNode">Xml-Knoten mit den Informationen</param>
        /// <returns>Ein neuerstelltes Log</returns>
        public static Log CreateLog(XmlNode xmlNode)
        {
            Log oLog = new Log();

            // Setzt das Filterlevel
            if (xmlNode.Attributes["filter"] != null)
            {
                String strFilterlevel
                    = xmlNode.Attributes["filter"].InnerText;
                try
                {
                    LogLevel oLogLevel = (LogLevel)
                        Enum.Parse(typeof(LogLevel), strFilterlevel, true);
                    oLog.FilterLevel = oLogLevel;
                }
                catch (FormatException)
                {
                    throw new InvalidOperationException(
                        String.Format( 
                        CultureInfo.CurrentUICulture,
                        LocalizationBS.Log_UnknownFilter, strFilterlevel));
                }
            } 
            
            AddLogProviders(oLog, xmlNode);

            return oLog;
        }

        /// <summary>
        /// Fügt Logprovider hinzu
        /// </summary>
        /// <param name="xmlNode">Xml-Knoten</param>
        /// <param name="oLog">Log, der erweitert werden soll.</param>
        public static void AddLogProviders(Log oLog, XmlNode xmlNode)
        {
            // Erzeugt die Logprovider
            foreach (XmlNode xmlProvider in xmlNode.SelectNodes("./logprovider"))
            {
                String strType = xmlProvider.Attributes["type"].InnerText;

                switch (strType)
                {
                    case "console":
                        oLog.AddLogProvider(new ConsoleProvider());
                        break;
                    case "file":
                        String strPath =
                            xmlProvider.Attributes["path"].InnerText;
                        oLog.AddLogProvider(new FileProvider(strPath));
                        break;
                }
            }
        }
    }
}
