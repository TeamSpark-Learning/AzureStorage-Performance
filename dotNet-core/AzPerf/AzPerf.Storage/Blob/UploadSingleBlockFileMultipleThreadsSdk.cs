using System;
using System.IO;
using System.Threading.Tasks;
using AzPerf.Storage.Helpers;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzPerf.Storage.Blob
{
    public class UploadSingleBlockFileMultipleThreadsSdk : StoragePerformanceScenarioBase
    {
        protected string FilePath;
        protected int FileSize;
        protected int ChunkSize;
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

        protected async Task InitializeChunkSizeAsync()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Chose chunk size in MB: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            var input = Console.ReadLine();

            int size;
            if (!int.TryParse(input, out size))
            {
                await InitializeChunkSizeAsync();
                return;
            }

            if (size < 1)
            {
                await InitializeChunkSizeAsync();
                return;
            }

            ChunkSize = size;
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

//            ThreadPool.QueueUserWorkItem()
        }

        protected async Task InitializeFileAsync()
        {
            FilePath = Path.GetTempFileName();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(FilePath);

            await FileHelper.CreateFileWithRandomContentAsync(FilePath, FileSize);
        }

        protected override async Task InitializeAsync()
        {
            await InitializeFileSizeAsync();
            await InitializeChunkSizeAsync();
            await InitializeThreadsCountAsync();
            await InitializeFileAsync();
        }

        protected override async Task DoWorkAsync()
        {
            var options = new BlobRequestOptions();

            options.ParallelOperationThreadCount = ThreadsCount;
            options.SingleBlobUploadThresholdInBytes = ChunkSize * 1024 * 1024;

            var blob = BlobContainer.GetBlockBlobReference(Guid.NewGuid().ToString("N"));
            await blob.UploadFromFileAsync(FilePath, null, options, null);

//            var blob = BlobContainer.GetBlockBlobReference(Guid.NewGuid().ToString("N"));
//            var blockIds = new List<string>();
//            using (var reader = File.OpenRead(FilePath))
//            {
//                var blockSize = FileSize / 10;
//                for (var i = 0; i < 10; i++)
//                {
//                    var buffer = new byte[FileSize / 10];
//                    await reader.ReadAsync(buffer, i * blockSize, buffer.Length);
//
//                    var stream = new MemoryStream(buffer);
//                    var id = Base64Encode(i.ToString());
//                    using (var md5 = MD5.Create())
//                    {
//                        var hash = GetMd5HashString(md5, buffer);
//                        Console.WriteLine(hash);
//                        await blob.PutBlockAsync(id, stream, Base64Encode(hash));
//                    }
//                    blockIds.Add(id);
//                }
//
//                await blob.PutBlockListAsync(blockIds);
//            }
        }
//
//        private string Base64Encode(string plainText) {
//            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
//            return Convert.ToBase64String(plainTextBytes);
//        }
//
//        private string GetMd5HashString(HashAlgorithm md5Hash, byte[] input)
//        {
//            var hash = md5Hash.ComputeHash(input);
//            var hashString = new StringBuilder();
//
//            foreach (var b in hash)
//            {
//                hashString.Append(b.ToString("x2"));
//            }
//
//            return hashString.ToString();
//        }

        protected override async Task CleanupAsync()
        {
            if (!string.IsNullOrEmpty(FilePath) && File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
    }
}