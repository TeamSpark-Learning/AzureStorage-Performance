using System;
using System.IO;
using System.Threading.Tasks;
using AzPerf.Storage.Helpers;

namespace AzPerf.Storage.Blob
{
    public class UploadSingleBlockFileMultipleThreads : StoragePerformanceScenarioBase
    {
        protected string FilePath;
        protected int FileSize;
        protected int ThreadsCount;

        protected async Task InitializeFileSizeAsync()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Chose file size in MB: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            var input = Console.ReadLine();

            int size;
            if (!int.TryParse(input, out size))
            {
                await InitializeFileSizeAsync();
                return;
            }

            if (size < 1)
            {
                await InitializeFileSizeAsync();
                return;
            }

            FileSize = size;
        }

        protected async Task InitializeThreadsCountAsync()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("How many threads to use: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            var input = Console.ReadLine();

            int count;
            if (!int.TryParse(input, out count))
            {
                await InitializeThreadsCountAsync();
                return;
            }

            if (count < 0)
            {
                await InitializeThreadsCountAsync();
                return;
            }

            ThreadsCount = count;
        }

        protected async Task InitializeFileAsync()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Chose each file size in MB: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            var input = Console.ReadLine();

            int size;
            if (!int.TryParse(input, out size))
            {
                await InitializeFileAsync();
                return;
            }

            if (size < 0)
            {
                await InitializeFileAsync();
                return;
            }

            FilePath = Path.GetTempFileName();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(FilePath);

            await FileHelper.CreateFileWithRandomContentAsync(FilePath, size);
        }

        protected override async Task InitializeAsync()
        {
            await InitializeFileSizeAsync();
            await InitializeThreadsCountAsync();
            await InitializeFileAsync();
        }

        protected override async Task DoWorkAsync()
        {
            await Task.Delay(500);
        }

        protected override async Task CleanupAsync()
        {
            if (!string.IsNullOrEmpty(FilePath) && File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
    }
}