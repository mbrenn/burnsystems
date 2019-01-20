using System;
using System.IO;    
using System.IO.Pipes;
using System.Threading.Tasks;

namespace BurnSystems.Logging.Pipe
{
    public class PipeLoggingClient : IDisposable
    {
        NamedPipeClientStream _pipe;

        public async Task Start(string pipeName)
        {
            _pipe = new NamedPipeClientStream(".", pipeName);
            Console.WriteLine("Connecting");
            await _pipe.ConnectAsync();
            Console.WriteLine("Connected");

            while (true)
            {
                var result = await LogMessageSerializer.ParseMessage(_pipe);
                if (result == null)
                {
                    break;
                }

                Console.WriteLine(result.ToString());
            }

            Console.WriteLine("Reading done");
        }

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }
        
        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
