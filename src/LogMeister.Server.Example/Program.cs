using BurnSystems.Logging;
using BurnSystems.Logging.Pipe;
using BurnSystems.Logging.Provider;

namespace LogMeister.Server.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            TheLog.AddProvider(new ConsoleProvider(), LogLevel.Trace);
            TheLog.AddProvider(new PipeLogProvider("BurnSystems.Prime"));
            TheLog.FilterThreshold = LogLevel.Trace;
                       
            for (var n = 100000000; n < 200000000; n++)
            {
                if (IsPrime(n))
                {
                    TheLog.Info($"{n} is prime");
                }
            }
        }

        public static bool IsPrime(long n)
        {
            for (var x = 2; x < n; x++)
            {
                if (n % x == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
