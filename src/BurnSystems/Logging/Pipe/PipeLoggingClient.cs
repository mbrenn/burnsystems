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

            using (var sr = new StreamReader(_pipe))
            {
                string temp;
                while((temp = await sr.ReadLineAsync()) != null)
                {
                    Console.WriteLine(temp);
                }
            }

            Console.WriteLine("Reading done");
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
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
