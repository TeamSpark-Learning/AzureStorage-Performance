using System;
using System.IO;
using System.Threading.Tasks;

namespace AzPerf.Storage.Helpers
{
    public static class FileHelper
    {
        public static async Task CreateFileWithRandomContentAsync(string filePath, int sizeInMb)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException(nameof(filePath));
            }

            if (sizeInMb < 0)
            {
                throw new ArgumentException(nameof(sizeInMb));
            }

            var rnd = new Random();
            using (var writer = File.Create(filePath))
            {
                for (var i = 0; i < sizeInMb; i++)
                {
                    var buffer = new byte[1024 * 1024];
                    rnd.NextBytes(buffer);

                    await writer.WriteAsync(buffer, 0, buffer.Length);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sizeInMb"></param>
        /// <param name="emptyPercentage">from 0 to 100</param>
        /// <returns></returns>
        public static async Task CreateFileWithEmptyPagesAsync(string filePath, int sizeInMb, int emptyPercentage)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException(nameof(filePath));
            }

            if (sizeInMb < 0)
            {
                throw new ArgumentException(nameof(sizeInMb));
            }

            if (emptyPercentage < 0 || emptyPercentage > 100)
            {
                throw new ArgumentException(nameof(emptyPercentage));
            }

            int pageSizeInBytes = 512;
            int sizeInPages = sizeInMb * 1024 * 1024 / pageSizeInBytes;

            decimal emptyCount = sizeInPages * ((decimal)emptyPercentage / 100);
            int emptyStep = (int)(sizeInPages / emptyCount);

            var rnd = new Random();
            using (var writer = File.Create(filePath))
            {
                for (var i = 0; i < sizeInPages; i++)
                {
                    var buffer = new byte[pageSizeInBytes];

                    if (i % emptyStep != 0)
                    {
                        rnd.NextBytes(buffer);
                    }

                    await writer.WriteAsync(buffer, 0, buffer.Length);
                }
            }
        }
    }
}