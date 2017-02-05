using System;
using System.IO;
using System.Threading.Tasks;

namespace AzPerf.Storage.Blob
{
    public class UploadSingleBlockFileSingleThread : StoragePerformanceBase
    {
        protected string filePath;

        protected override async Task InitializeAsync()
        {
            Console.Write("Chose file size in MB: ");
            var input = Console.ReadLine();

            int size;
            if (int.TryParse(input, out size))
            {
                if (size < 1)
                {
                    await InitializeAsync();
                }
                else
                {
                    filePath = Path.GetTempFileName();
                    var rnd = new Random();

                    using (var writer = File.Create(filePath))
                    {
                        for (var i = 0; i < size; i++)
                        {
                            var buffer = new byte[1024 * 1024];
                            rnd.NextBytes(buffer);

                            await writer.WriteAsync(buffer, i * 1024 * 1024, buffer.Length);
                        }
                    }
                }
            }
            else
            {
                await InitializeAsync();
            }
        }

        protected override async Task DoWorkAsync()
        {
            await Task.Delay(500);
        }

        protected override async Task CleanupAsync()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}