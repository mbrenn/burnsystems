using System;
using System.Threading.Tasks;
using BurnSystems.Logging.Pipe;

namespace LogMeister.Client.Example
{
    class Program
    {
        public static void Main()
        {
            var server = new PipeLoggingClient();
            Task.Run(() => server.Start("BurnSystems.Prime")).ContinueWith(x=>Console.WriteLine("Server closed connection")).Wait();
            Console.ReadKey();
        }
    }
}
