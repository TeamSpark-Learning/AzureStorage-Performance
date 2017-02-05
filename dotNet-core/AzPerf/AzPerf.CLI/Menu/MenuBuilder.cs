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
                Console.WriteLine(Title);
            }
            for (var i = 0; i < Items.Count; i++)
            {
                Console.WriteLine("{0}. {1}", i, Items[i].Title);
            }

            Console.WriteLine("q. Quit");
            Console.Write("Make your choice: ");

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