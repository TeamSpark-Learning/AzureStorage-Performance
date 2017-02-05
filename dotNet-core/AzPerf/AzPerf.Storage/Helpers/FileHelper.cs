using System;
using System.IO;
using System.Threading.Tasks;

namespace AzPerf.Storage.Helpers
{
    public static class FileHelper
    {
        public static async Task CreateFileWithRandomContentAsync(string filePath, int sizeInMb)
        {
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
    }
}