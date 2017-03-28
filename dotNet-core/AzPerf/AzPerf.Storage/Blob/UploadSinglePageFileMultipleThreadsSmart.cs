using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AzPerf.Storage.Helpers;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzPerf.Storage.Blob
{
    public class UploadSinglePageFileMultipleThreadsSmart : StoragePerformanceScenarioBase
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
            var fileInfo = new FileInfo(FilePath);

            var blob = BlobContainer.GetPageBlobReference(Guid.NewGuid().ToString("N"));
            await blob.CreateAsync(fileInfo.Length);

            var tasks = new List<Task>();

            using (var fileStream = new FileStream(FilePath, FileMode.Open))
            {
                var buffer = new byte[512];

                while (fileStream.Position < fileStream.Length)
                {
                    await fileStream.ReadAsync(buffer, 0, 512);
                    if (buffer.Any(b => b != 0))
                    {
                        tasks.Add(UploadPageAsync(blob, buffer, fileStream.Position));
                    }
                }

                await Task.WhenAll(tasks);
            }
        }

        private async Task UploadPageAsync(CloudPageBlob blob, byte[] pageBuffer, long position)
        {
            var memStream = new MemoryStream(pageBuffer);
            await blob.WritePagesAsync(memStream, position, null);
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