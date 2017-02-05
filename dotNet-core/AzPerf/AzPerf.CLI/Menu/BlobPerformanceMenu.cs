using System.Threading.Tasks;
using AzPerf.Storage.Blob;

namespace AzPerf.CLI.Menu
{
    public static class BlobPerformanceMenu
    {
        public static void RootMenu()
        {
            var menu = new MenuBuilder("Blob performance");
            menu.Items.Add(new MenuItem("Upload single (block) file via single thread", new UploadSingleBlockFileSingleThread()));
            menu.Items.Add(new MenuItem("Upload single (block) file via multiple threads", new UploadSingleBlockFileMultipleThreads()));
            menu.Items.Add(new MenuItem("Upload single (block) file via multiple threads (SDK)", new UploadSingleBlockFileMultipleThreadsSdk()));
            menu.Items.Add(new MenuItem("Upload multiple (block) files via multiple threads", new UploadMultipleBlockFilesMultipleThreads()));
            menu.Items.Add(new MenuItem("Upload single (page) file via multiple threads (smart)", new UploadSinglePageFileMultipleThreadsSmart()));
            menu.Items.Add(new MenuItem("Download single (page) file via single threads", new DownloadSinglePageFileSingleThread()));
            menu.Items.Add(new MenuItem("Download single (page) file via multiple threads", new DownloadSinglePageFileMultipleThreads()));
            menu.ShowMenu();
        }
    }
}