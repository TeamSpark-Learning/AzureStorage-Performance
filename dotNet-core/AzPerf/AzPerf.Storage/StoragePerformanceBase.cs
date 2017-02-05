using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AzPerf.Storage
{
    public abstract class StoragePerformanceBase
    {
        protected abstract Task InitializeAsync();
        protected abstract Task DoWorkAsync();
        protected abstract Task CleanupAsync();

        public async Task RunAsync()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Initializing...");
                await InitializeAsync();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Initialized.");

                var timer = new Stopwatch();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Working...");
                timer.Start();

                await DoWorkAsync();

                timer.Stop();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Finished.");

                Console.WriteLine("Total time: {0}", timer.Elapsed);
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Cleaning up...");
                await CleanupAsync();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Done.");
            }
        }
    }
}