using System;
using System.IO;
using System.Threading.Tasks;
using AzPerf.Storage.Helpers;

namespace AzPerf.Storage.Blob
{
    public class UploadSinglePageFileMultipleThreadsSmart : StoragePerformanceBase
    {
        protected string FilePath;
        protected int FileSize;
        protected int EmptyPercentage;

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

        protected async Task InitializeEmptyPercentageAsync()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Chose file empty percentage: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            var input = Console.ReadLine();

            int percentage;
            if (!int.TryParse(input, out percentage))
            {
                await InitializeEmptyPercentageAsync();
                return;
            }

            if (percentage < 0 || percentage > 100)
            {
                await InitializeEmptyPercentageAsync();
                return;
            }

            EmptyPercentage = percentage;
        }

        protected async Task InitializeFileAsync()
        {
            FilePath = Path.GetTempFileName();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(FilePath);

            await FileHelper.CreateFileWithEmptyPagesAsync(FilePath, FileSize, EmptyPercentage);
        }

        protected override async Task InitializeAsync()
        {
            await InitializeFileSizeAsync();
            await InitializeEmptyPercentageAsync();
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