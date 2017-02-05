using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzPerf.Storage.Blob
{
    public class BlobCleanup : StoragePerformanceScenarioBase
    {
        protected override async Task InitializeAsync()
        {
        }

        protected override async Task DoWorkAsync()
        {
        }

        protected override async Task CleanupAsync()
        {
            var client = StorageAccount.CreateCloudBlobClient();

            var containers = new List<CloudBlobContainer>();
            BlobContinuationToken token = null;
            do
            {
                var response = await client.ListContainersSegmentedAsync(token);

                containers.AddRange(response.Results);
                token = response.ContinuationToken;
            } while (token != null);

            var tasks = containers.Select(c => c.DeleteAsync());
            await Task.WhenAll(tasks);
        }
    }
}