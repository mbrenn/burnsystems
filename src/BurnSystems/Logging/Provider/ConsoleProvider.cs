using System;
using System.Globalization;

namespace BurnSystems.Logging.Provider
{
    public class ConsoleProvider : ILogProvider
    {
        /// <summary>
        /// Defines the colors of the console
        /// </summary>
        private readonly ConsoleColor[] _consoleColors = {
            ConsoleColor.DarkGray,
            ConsoleColor.Gray,
            ConsoleColor.Green,
            ConsoleColor.Yellow,
            ConsoleColor.Red,
            ConsoleColor.Magenta
        };

        /// <summary>
        /// Defines just the sync object
        /// </summary>
        private static readonly object SyncObject = new object();

        public void LogMessage(LogMessage logMessage)
        {
            lock (SyncObject)
            {
                var timePassed = DateTime.Now - TheLog.TimeCreated;
                var old = Console.ForegroundColor;
                Console.ForegroundColor = _consoleColors[(int) logMessage.LogLevel - 1];
                Console.WriteLine($"{timePassed.TotalSeconds.ToString("n3", CultureInfo.InvariantCulture)}: {logMessage}");
                Console.ForegroundColor = old;
            }
        }
    }
}