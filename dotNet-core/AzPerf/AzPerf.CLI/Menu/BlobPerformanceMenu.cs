using System.Threading.Tasks;
using AzPerf.Storage.Blob;

namespace AzPerf.CLI.Menu
{
    public static class BlobPerformanceMenu
    {
        public static void RootMenu()
        {
            var menu = new MenuBuilder("Blob performance");
            menu.Items.Add(new MenuItem("Upload single (block) file via single thread", UploadSingleBlockFileSingleThread));
            menu.Items.Add(new MenuItem("Upload single (block) file via multiple threads", UploadSingleBlockFileMultipleThreads));
            menu.Items.Add(new MenuItem("Upload multiple (block) files via multiple threads", UploadMultipleBlockFilesMultipleThreads));
            menu.Items.Add(new MenuItem("Upload single (page) file via multiple threads (smart)", UploadSinglePageFileMultipleThreadsSmart));
            menu.Items.Add(new MenuItem("Download single (page) file via multiple threads", DownloadSinglePageFileMultipleThreads));
            menu.ShowMenu();
        }

        public static void UploadSingleBlockFileSingleThread()
        {
            var scenario = new UploadSingleBlockFileSingleThread();
            var task = Task.Run(scenario.RunAsync);
            task.Wait();
        }

        public static void UploadSingleBlockFileMultipleThreads()
        {

        }

        public static void UploadMultipleBlockFilesMultipleThreads()
        {
            var scenario = new UploadMultipleBlockFilesMultipleThreads();
            var task = Task.Run(scenario.RunAsync);
            task.Wait();
        }

        public static void UploadSinglePageFileMultipleThreadsSmart()
        {

        }

        public static void DownloadSinglePageFileMultipleThreads()
        {

        }
    }
}