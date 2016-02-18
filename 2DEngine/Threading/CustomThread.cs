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
        public bool IsAlive { get; private set; }

        private Thread Thread { get; set; }

        public event OnThreadTaskComplete OnThreadTaskComplete;

        public CustomThread(ThreadStart functionToRun)
        {
            Thread = new Thread(functionToRun);
            Thread.Start();

            IsAlive = true;
        }

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
