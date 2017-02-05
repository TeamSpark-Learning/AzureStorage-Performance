using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzPerf.Storage.Table
{
    public class TableCleanup : StoragePerformanceScenarioBase
    {
        protected override async Task InitializeAsync()
        {
        }

        protected override async Task DoWorkAsync()
        {
        }

        protected override async Task CleanupAsync()
        {
            var client = StorageAccount.CreateCloudTableClient();

            var tables = new List<CloudTable>();
            TableContinuationToken token = null;
            do
            {
                var response = await client.ListTablesSegmentedAsync(token);

                tables.AddRange(response.Results);
                token = response.ContinuationToken;
            } while (token != null);

            var tasks = tables.Select(c => c.DeleteAsync());
            await Task.WhenAll(tasks);
        }
    }
}