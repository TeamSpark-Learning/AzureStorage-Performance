using System;
using AzPerf.CLI.Menu;

namespace AzPerf.CLI
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.Clear();
            RootMenu();
            Console.WriteLine("Thank you for your cooperation. Bye.");
        }

        public static void RootMenu()
        {
            var menu = new MenuBuilder("Welcome to Azure Storage Performance demo app!");
            menu.Items.Add(new MenuItem("Blob performance", BlobPerformance.RootMenu));
            menu.Items.Add(new MenuItem("Table performance", TablePerformance.RootMenu));
            menu.ShowMenu();
        }
    }
}