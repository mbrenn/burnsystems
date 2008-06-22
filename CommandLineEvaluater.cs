//-----------------------------------------------------------------------
// <copyright file="CommandLineEvaluater.cs" company="Martin Brenn">
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

namespace BurnSystems
{
    /// <summary>
    /// Wertet die Kommandozeilen aus
    /// </summary>
    public class CommandLineEvaluater
    {
        /// <summary>
        /// Nichtbenannte Argument
        /// </summary>
        List<String> _UnnamedArguments =
            new List<string>();

        /// <summary>
        /// Benannte Argumente
        /// </summary>
        NiceDictionary<String, String> _NamedArguments =
            new NiceDictionary<string, string>();

        /// <summary>
        /// Liste aller unbenannten Argumente
        /// </summary>
        public List<String> UnnamedArguments
        {
            get { return _UnnamedArguments; }
        }

        /// <summary>
        /// Liste aller benannten Argumente
        /// </summary>
        public NiceDictionary<String, String> NamedArguments
        {
            get { return _NamedArguments; }
        }

        /// <summary>
        /// Erzeugt eine neue Struktur
        /// </summary>
        /// <param name="astrArguments"></param>
        public CommandLineEvaluater(String[] astrArguments)
        {
            foreach (String strArgument in astrArguments)
            {
                if (strArgument.Length == 0)
                {
                    continue;
                }

                if (strArgument[0] == '-')
                {
                    int nPos = strArgument.IndexOf('=');
                    if (nPos == -1)
                    {
                        _NamedArguments[strArgument] = "1";
                    }
                    else
                    {
                        _NamedArguments[strArgument.Substring(1, nPos - 1)] =
                            strArgument.Substring(nPos + 1);
                    }
                    continue;
                }
                else
                {
                    _UnnamedArguments.Add(strArgument);
                }
            }
        }
    }
}
