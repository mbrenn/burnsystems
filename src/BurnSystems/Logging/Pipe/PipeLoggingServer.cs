using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace BurnSystems.Logging.Pipe
{
    public class PipeLoggingServer : IDisposable
    {
        private readonly List<NamedPipeServerStream> _stream = new List<NamedPipeServerStream>();

        public void Start(string pipeName)
        {
            try
            {
                var pipe = new NamedPipeServerStream(pipeName, PipeDirection.InOut, 10);
                pipe.WaitForConnection();
                lock (_stream)
                {
                    _stream.Add(pipe);
                }

                Task.Run(() =>
                 {
                     Start(pipeName);
                 });
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        public void SendMessage(LogMessage message)
        {
            lock (_stream)
            {
                var bytes = LogMessageSerializer.ConvertMessage(message);

                if (_stream.Count == 0)
                {
                    // Not connected
                    return;
                }

                foreach (var stream in _stream)
                {
                    try
                    {
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Flush();

                    }
                    catch (IOException)
                    {
                        stream.Dispose();
                        _stream.Remove(stream);
                    }
                }
            }
        }

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    lock (_stream)
                    {
                        foreach (var stream in _stream)
                        {
                            stream.Dispose();
                        }
                        _stream.Clear();
                    }
                }

                _disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
