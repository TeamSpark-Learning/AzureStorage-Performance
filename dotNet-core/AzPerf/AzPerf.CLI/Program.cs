using System;
using AzPerf.CLI.Menu;

namespace AzPerf.CLI
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var color = Console.ForegroundColor;

            Console.Clear();
            RootMenu();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Thank you for your cooperation. Bye.");

            Console.ForegroundColor = color;
        }

        public static void RootMenu()
        {
            var menu = new MenuBuilder("Welcome to Azure Storage Performance demo app!");
            menu.Items.Add(new MenuItem("Blob performance", BlobPerformanceMenu.RootMenu));
            menu.Items.Add(new MenuItem("Table performance", TablePerformanceMenu.RootMenu));
            menu.ShowMenu();
        }
    }
}