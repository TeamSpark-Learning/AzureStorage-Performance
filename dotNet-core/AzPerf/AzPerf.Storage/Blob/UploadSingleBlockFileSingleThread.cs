using System;
using System.IO;
using System.Threading.Tasks;
using AzPerf.Storage.Helpers;

namespace AzPerf.Storage.Blob
{
    public class UploadSingleBlockFileSingleThread : StoragePerformanceScenarioBase
    {
        protected string FilePath;

        protected async Task InitializeFileAsync()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Chose file size in MB: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            var input = Console.ReadLine();

            int size;
            if (!int.TryParse(input, out size))
            {
                await InitializeFileAsync();
                return;
            }

            if (size < 1)
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
            await InitializeFileAsync();
        }

        protected override async Task DoWorkAsync()
        {
            var blob = BlobContainer.GetBlockBlobReference(Guid.NewGuid().ToString("N"));
            await blob.UploadFromFileAsync(FilePath);
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