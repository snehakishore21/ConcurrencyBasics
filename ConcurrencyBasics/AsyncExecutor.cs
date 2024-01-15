using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyBasics
{
    public class AsyncExecutor
    {
        public void Execute(Action callback)
        {
            // Simulate asynchronous work
            Task.Run(()=>
            {
                Thread.Sleep(2000); // Simulating asynchronous work
                Console.WriteLine("Asynchronous work completed");
                callback?.Invoke();
            });
        }
    }

    public class SynchronousExecutor : AsyncExecutor
    {
        private readonly AutoResetEvent executionCompleteEvent = new AutoResetEvent(false);

        public new void Execute(Action callback)
        {
            lock (executionCompleteEvent)
            {
                // Wrap the original callback with a wrapper that signals completion
                base.Execute(() =>
                {
                    callback?.Invoke();
                    executionCompleteEvent.Set();
                });

                // Wait for the asynchronous execution to complete
                executionCompleteEvent.WaitOne();
                Console.WriteLine("Synchronous execution completed");
            }
        }
    }
    internal class AsyncExecutorFunction
    {
        public static void AsyncExecutorMainFunc()
        {
            SynchronousExecutor synchronousExecutor = new SynchronousExecutor();

            // Example usage
            synchronousExecutor.Execute(() => Console.WriteLine("Callback invoked"));
        }
    }
}