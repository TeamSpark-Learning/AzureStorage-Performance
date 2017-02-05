using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AzPerf.Storage.Helpers;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzPerf.Storage.Blob
{
    public class UploadSingleBlockFileMultipleThreads : StoragePerformanceScenarioBase
    {
        protected string FilePath;
        protected int FileSize;
        protected int ChunkSize;
        protected int ThreadsCount;
        private int _bytesToUpload;
        private int _blockSize;

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
            var blob = BlobContainer.GetBlockBlobReference(Guid.NewGuid().ToString("N"));

            _bytesToUpload = FileSize * 1024 * 1024;
            _blockSize = ChunkSize * 1024 * 1024;

            var i = 0;
            var blockIds = new List<string>();
            var tasks = new List<Task>();

            while (_bytesToUpload > 0)
            {
                blockIds.Add(Base64Encode(i.ToString()));

                var size = Math.Min(_blockSize, _bytesToUpload);
                tasks.Add(UploadChunk(blob, i, size));

                _bytesToUpload -= size;
                i++;
            }

            await Task.WhenAll(tasks);
            await blob.PutBlockListAsync(blockIds);
        }

        private async Task UploadChunk(CloudBlockBlob blob, int number, int size)
        {
            using (var reader = File.OpenRead(FilePath))
            {
                var buffer = new byte[Math.Min(_blockSize, _bytesToUpload)];
                var id = Base64Encode(number.ToString());

                await reader.ReadAsync(buffer, 0, buffer.Length);

                var stream = new MemoryStream(buffer);
                using (var md5 = MD5.Create())
                {
                    var hash = GetMd5Base64HashString(md5, buffer);
                    await blob.PutBlockAsync(id, stream, hash);
                }
            }
        }

        private string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        private string GetMd5Base64HashString(HashAlgorithm md5Hash, byte[] input)
        {
            var hash = md5Hash.ComputeHash(input);

            return Convert.ToBase64String(hash, 0, 16);
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