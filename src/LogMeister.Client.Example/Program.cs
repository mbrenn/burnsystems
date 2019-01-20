using System;
using System.Threading.Tasks;
using BurnSystems.Logging.Pipe;

namespace LogMeister.Client.Example
{
    class Program
    {
        public static void Main(string[] args)
        {
            var server = new PipeLoggingClient();
            Task.Run(() => server.Start("BurnSystems.Prime")).Wait();

            Console.ReadKey();
        }
    }
}
