using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzPerf.Storage
{
    public abstract class StoragePerformanceScenarioBase
    {
        protected abstract Task InitializeAsync();
        protected abstract Task DoWorkAsync();
        protected abstract Task CleanupAsync();

        protected CloudStorageAccount StorageAccount { get; private set; }
        protected CloudBlobContainer BlobContainer { get; private set; }
        protected CloudTable Table { get; private set; }

        protected async Task ConnectAzureStorageAsync()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("myconfigs.json");

            var configuration = builder.Build();

            var accountName = configuration["AccountName"];
            var accountKey = configuration["AccountKey"];

            var cred = new StorageCredentials(accountName, accountKey);
            StorageAccount = new CloudStorageAccount(cred, false);

            var name = DateTime.UtcNow.ToString("yyyyMMddhhmmss");

            BlobContainer = StorageAccount
                .CreateCloudBlobClient()
                .GetContainerReference(string.Format("fldr{0}", name));

            Table = StorageAccount
                .CreateCloudTableClient()
                .GetTableReference(string.Format("tbl{0}", name));

            await Task.WhenAll(
                BlobContainer.CreateIfNotExistsAsync(),
                Table.CreateIfNotExistsAsync());
        }

        public async Task RunAsync()
        {
            await ConnectAzureStorageAsync();

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
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                throw;
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