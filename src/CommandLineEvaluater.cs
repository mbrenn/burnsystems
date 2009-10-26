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

namespace BurnSystems
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using BurnSystems.Collections;

    /// <summary>
    /// Wertet die Kommandozeilen aus
    /// </summary>
    public class CommandLineEvaluater
    {
        /// <summary>
        /// Nichtbenannte Argument
        /// </summary>
        private List<string> unnamedArguments =
            new List<string>();

        /// <summary>
        /// Benannte Argumente
        /// </summary>
        private NiceDictionary<string, string> namedArguments =
            new NiceDictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the CommandLineEvaluater class.
        /// </summary>
        /// <param name="arguments">List of program arguments</param>
        public CommandLineEvaluater(string[] arguments)
        {
            foreach (var argument in arguments)
            {
                if (argument.Length == 0)
                {
                    continue;
                }

                if (argument[0] == '-')
                {
                    int pos = argument.IndexOf('=');
                    if (pos == -1)
                    {
                        this.namedArguments[argument] = "1";
                    }
                    else
                    {
                        this.namedArguments[argument.Substring(1, pos - 1)] =
                            argument.Substring(pos + 1);
                    }

                    continue;
                }
                else
                {
                    this.unnamedArguments.Add(argument);
                }
            }
        }

        /// <summary>
        /// Gets a list of unnamed arguments
        /// </summary>
        public List<string> UnnamedArguments
        {
            get { return this.unnamedArguments; }
        }

        /// <summary>
        /// Gets a dictionary of named arguments
        /// </summary>
        public NiceDictionary<string, string> NamedArguments
        {
            get { return this.namedArguments; }
        }
    }
}
