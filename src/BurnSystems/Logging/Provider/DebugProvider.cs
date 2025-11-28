using System.Diagnostics;
using System.Globalization;

namespace BurnSystems.Logging.Provider
{
    public class DebugProvider: ILogProvider
    {
        private static readonly Lock SyncObject = new();
        
        public void LogMessage(LogMessage logMessage)
        {
            lock (SyncObject)
            {
                var timePassed = DateTime.Now - TheLog.TimeCreated;
                Debug.WriteLine(
                    $"{timePassed.TotalSeconds.ToString("n3", CultureInfo.InvariantCulture)}: {logMessage}");
            }
        }
    }
}