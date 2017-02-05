using System;
using System.Collections.Generic;

namespace AzPerf.CLI.Menu
{
    public class MenuBuilder
    {
        public string Title { get; private set; }
        public List<MenuItem> Items { get; private set; }

        public MenuBuilder(string title = null)
        {
            Title = title;
            Items = new List<MenuItem>();
        }

        public void ShowMenu()
        {
            if (!string.IsNullOrEmpty(Title))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(Title);
            }
            for (var i = 0; i < Items.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("{0}. ", i);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(Items[i].Title);
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("q. ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Quit");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Make your choice: ");

            Console.ForegroundColor = ConsoleColor.Magenta;
            var input = Console.ReadLine();

            if (input == "q")
            {
                return;
            }

            int choice;
            if (int.TryParse(input, out choice))
            {
                if (choice < 0 || choice > Items.Count - 1)
                {
                    ShowMenu();
                }
                else
                {
                    Items[choice].Action();
                }
            }
            else
            {
                ShowMenu();
            }
        }
    }

    public class MenuItem
    {
        public string Title { get; private set; }
        public Action Action { get; private set; }

        public MenuItem(string title, Action action)
        {
            Title = title;
            Action = action;
        }
    }
}