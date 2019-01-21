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
            await _pipe.ConnectAsync();

            try
            {
                while (true)
                {
                    var result = await LogMessageSerializer.ParseMessage(_pipe);
                    if (result == null)
                    {
                        // Message is not known
                        continue;
                    }

                    Console.WriteLine(result.ToString());
                }
            }
            catch (EndOfStreamException)
            {
            }
        }

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _pipe?.Dispose();
                    _pipe = null;
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
