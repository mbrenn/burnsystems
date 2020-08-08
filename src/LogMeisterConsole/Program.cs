using System;
using System.Threading.Tasks;
using BurnSystems.Logging.Pipe;

namespace LogMeisterConsole
{
    class Program
    {
        static void Main()
        {
            var server = new PipeLoggingClient();
            Task.Run(() => server.Start("BurnSystems.Prime")).Wait();

            Console.ReadKey();
        }
    }
}
