//-----------------------------------------------------------------------
// <copyright file="ConsoleProvider.cs" company="Martin Brenn">
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

namespace BurnSystems.Logging
{
    /// <summary>
    /// This provider writes all data on the console
    /// </summary>
    public class ConsoleProvider : ILogProvider
    {
        /// <summary>
        /// Flag, ob nur eine einfache Ausgabe ohne Uhrzeiten 
        /// und Schwere des Ereignisses ausgegeben werden sollen
        /// </summary>
        bool _SimpleOutput = false;

        /// <summary>
        /// Flag, ob nur eine einfache Ausgabe ohne Uhrzeiten 
        /// und Schwere des Ereignisses ausgegeben werden sollen
        /// </summary>
        public bool SimpleOutput
        {
            get { return _SimpleOutput; }
            set { _SimpleOutput = value; }
        }

        /// <summary>
        /// Erzeugt eine neue Instanz
        /// </summary>
        public ConsoleProvider()
            : this ( false )
        {
        }

        /// <summary>
        /// Erzeugt eine neue Instanz und legt fest ob nur
        /// eine einfache Ausgabe erzeugt werden soll
        /// </summary>
        /// <param name="bSimpleOutput">true, wenn nur eine einfache
        /// Ausgabe erzeugt werden soll. </param>
        public ConsoleProvider(bool bSimpleOutput)
        {
            _SimpleOutput = bSimpleOutput;
        }


        #region ILogProvider Members

        /// <summary>
        /// Nothing is done
        /// </summary>
        public void Start()
        {
            
        }

        /// <summary>
        /// Nothing is done
        /// </summary>
        public void Shutdown()
        {
            
        }

        /// <summary>
        /// Writes the logentry to console
        /// </summary>
        /// <param name="entry">Entry</param>
        public void DoLog(LogEntry entry)
        {
            var eColor = Console.ForegroundColor;

            switch (entry.LogLevel)
            {
                case LogLevel.Message:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogLevel.Fail:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Critical:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Fatal:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }

            if (_SimpleOutput)
            {
                Console.WriteLine(entry.Message);
            }
            else
            {
                Console.WriteLine("{0} {1} {2}", 
                    entry.Created, 
                    entry.LogLevel.ToString(), 
                    entry.Message);

            }

            Console.ForegroundColor = eColor;
        }

        #endregion
    }
}
