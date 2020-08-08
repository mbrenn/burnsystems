using System.Threading;
using BurnSystems.Logging.Provider;

namespace BurnSystems.Logging.Console
{
    class Program
    {
        static void Main()
        {
            TheLog.AddProvider(new ConsoleProvider(), LogLevel.Trace);
            TheLog.AddProvider(new FileProvider("test.log", true), LogLevel.Info);
            TheLog.AddProvider(new EventTracingProvider(), LogLevel.Trace);
            TheLog.Trace("Not added");
            Thread.Sleep(100);
            TheLog.Debug("This is a debug message");
            Thread.Sleep(80);
            TheLog.Info("We have an info message.");
            Thread.Sleep(80);
            TheLog.Warn("We have a warn");
            Thread.Sleep(80);
            TheLog.Error("Error Occured....");
            Thread.Sleep(80);
            TheLog.Fatal("We have to quit the application due to loss of O².");
            Thread.Sleep(80);
            new WorkingMan().Work();

            System.Console.WriteLine("Press key");
            System.Console.ReadKey();
        }
    }
}
