using System;
using System.Threading;

namespace _2DEngine
{
    /// <summary>
    /// A delegate we use to call a function when our thread has finished executing.
    /// </summary>
    public delegate void OnThreadTaskComplete();

    /// <summary>
    /// A class which wraps a thread to provide custom behaviour.
    /// </summary>
    public class CustomThread
    {
        /// <summary>
        /// A bool to indicate whether this thread is a alive.
        /// </summary>
        public bool IsAlive { get; private set; }

        /// <summary>
        /// The actual thread that this class wraps around
        /// </summary>
        private Thread Thread { get; set; }

        /// <summary>
        /// The event that will execute when this thread's main task is completed.
        /// </summary>
        public event OnThreadTaskComplete OnThreadTaskComplete;

        public CustomThread(ThreadStart functionToRun)
        {
            Thread = new Thread(functionToRun);
            Thread.Start();

            IsAlive = true;
        }

        /// <summary>
        /// Checks to see if the thread has finished it's main work and calls it's finished callback if necessary.
        /// </summary>
        public void Update()
        {
            if (!Thread.IsAlive)
            {
                IsAlive = false;
                if (OnThreadTaskComplete != null)
                {
                    OnThreadTaskComplete();
                }
            }
        }
    }
}
