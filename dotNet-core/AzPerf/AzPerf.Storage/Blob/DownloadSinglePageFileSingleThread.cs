using System.Threading.Tasks;

namespace AzPerf.Storage.Blob
{
    public class DownloadSinglePageFileSingleThread : StoragePerformanceBase
    {
        protected override async Task InitializeAsync()
        {
            throw new System.NotImplementedException();
        }

        protected override async Task DoWorkAsync()
        {
            await Task.Delay(500);
        }

        protected override async Task CleanupAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}