using System.Threading.Tasks;

namespace BurnSystems.Logging.Pipe
{
    public class PipeLogProvider : ILogProvider
    {
        private readonly PipeLoggingServer _server;

        public PipeLogProvider(string pipeName)
        {
            _server = new PipeLoggingServer();
            Task.Run(() => _server.Start(pipeName));
        }

        public void LogMessage(LogMessage logMessage)
        {
            Task.Run(() =>
            {
                lock (_server)
                {
                    _server.SendMessage(logMessage);
                }
            });
        }
    }
}
