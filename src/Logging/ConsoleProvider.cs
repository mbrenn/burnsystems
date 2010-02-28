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

namespace BurnSystems.Logging
{
    using System;

    /// <summary>
    /// This provider writes all data on the console
    /// </summary>
    public class ConsoleProvider : ILogProvider
    {
        /// <summary>
        /// Initializes a new instance of the ConsoleProvider class.
        /// </summary>
        public ConsoleProvider()
            : this(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ConsoleProvider class.
        /// <c>simpleOutput</c> defines if a colorful output should be used.
        /// </summary>
        /// <param name="simpleOutput">true, wenn nur eine einfache
        /// Ausgabe erzeugt werden soll. </param>
        public ConsoleProvider(bool simpleOutput)
        {
            this.SimpleOutput = simpleOutput;
        }

        /// <summary>
        /// Gets or sets a value indicating whether a simple output without
        /// date and loglevel should be used.
        /// </summary>
        public bool SimpleOutput
        {
            get;
            set;
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
        /// <param name="entry">Entry to be logged</param>
        public void DoLog(LogEntry entry)
        {
            var color = Console.ForegroundColor;

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

            if (this.SimpleOutput)
            {
                if (!string.IsNullOrEmpty(entry.Catagories))
                {
                    Console.Write(entry.Catagories + ": ");
                }

                Console.WriteLine(entry.Message);
            }
            else
            {
                Console.WriteLine(
                    "{0} {1} {2}", 
                    entry.Created, 
                    entry.LogLevel.ToString(), 
                    entry.Message);
            }

            Console.ForegroundColor = color;
        }

        #endregion
    }
}
