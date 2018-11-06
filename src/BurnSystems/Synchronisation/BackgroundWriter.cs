using System;
using System.Threading.Tasks;

namespace BurnSystems.Synchronisation
{
    /// <summary>
    /// Gets or sets a background worker that needs to be derived to support the specific task of the user
    /// </summary>
    public class BackgroundWriter
    {
        /// <summary>
        /// Gets or sets the time that the background worker will wait after the last
        /// adding of content
        /// </summary>
        public TimeSpan WriteWaitTime { get; set; } = TimeSpan.FromSeconds(2);

        /// <summary>
        /// Gets or sets the time that the background worker will do the maximum waiting time
        /// before the content is stored. 
        /// </summary>
        public TimeSpan WriteMaxTime = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Gets or sets the threshold that will be accepted before data is stored. 
        /// </summary>
        public TimeSpan Threshold = TimeSpan.FromSeconds(0.1);

        /// <summary>
        /// Stores the value when the last update was performed
        /// </summary>
        private DateTime _lastUpdate;

        /// <summary>
        /// STores the value when the last storing was performed
        /// </summary>
        private DateTime _lastStore;

        /// <summary>
        /// Gets whether the task is running
        /// </summary>
        private bool _taskRunning;

        /// <summary>
        /// Gets whether the information is stored
        /// </summary>
        private bool _stored = true;

        /// <summary>
        /// Just the sync object
        /// </summary>
        private readonly object _syncObject = new object();

        /// <summary>
        /// Gets te information when the object was created
        /// </summary>
        private readonly DateTime _started;

        /// <summary>
        /// Initializes a new instance of the BackgroundWriter
        /// </summary>
        public BackgroundWriter()
        {
            _lastUpdate = DateTime.Now;
            _lastStore = DateTime.Now;
            _started = DateTime.Now;
        }

        /// <summary>
        /// Gets the information when the next storage might happen.
        /// The class will wait this waiting time. If a new setting might occur
        /// during the waiting time, an additional waiting time might be added
        /// </summary>
        /// <returns></returns>
        private TimeSpan GetNextTick()
        {
            lock (_syncObject)
            {
                var now = DateTime.Now;
                var elapsedUpdate = now - _lastUpdate;
                var elapsedStore = now - _lastStore;

                var leftUpdate = WriteWaitTime - elapsedUpdate;
                var leftStore = WriteMaxTime - elapsedStore;

                if (leftUpdate < leftStore)
                {
                    return leftUpdate;
                }
                else
                {
                    return leftStore;
                }
            }
        }

        /// <summary>
        /// The derived class needs to call the method, when a value to be stored was
        /// added into the class. The waiting time will started and StoreContent will be
        /// called after the waiting time
        /// </summary>
        protected void UpdateContent()
        {
            lock (_syncObject)
            {
                _lastUpdate = DateTime.Now;
                _stored = false;

                if (!_taskRunning)
                {
                    _taskRunning = true;
                    _lastStore = DateTime.Now;
                    Task.Run(() => BackgroundTask());
                }
            }
        }

        /// <summary>
        /// This method needs to be overridden to support the writing of the changed content.
        /// </summary>
        protected virtual void StoreContent()
        {
            lock (_syncObject)
            {
                Console.WriteLine($"Store: {(DateTime.Now - _started).TotalSeconds:n3} since start. {(DateTime.Now - _lastStore).TotalSeconds:n3}");
            }
        }

        /// <summary>
        /// Defines the task that is running in the background to support the writing.s
        /// </summary>
        private async void BackgroundTask()
        {
            while (!_stored)
            {
                var tickTime = GetNextTick();
                Console.WriteLine($"Left time: {tickTime.TotalSeconds:n3}");

                if (tickTime.TotalSeconds <= Threshold.TotalSeconds)
                {
                    _taskRunning = false;
                    lock (_syncObject)
                    {
                        _stored = true;
                        StoreContent();
                        _lastStore = DateTime.Now;
                    }
                }
                else
                {
                    await Task.Delay(tickTime);
                }
            }
        }
    }
}
