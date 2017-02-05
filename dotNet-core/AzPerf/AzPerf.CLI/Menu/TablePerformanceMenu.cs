using System;

namespace AzPerf.CLI.Menu
{
    public static class TablePerformanceMenu
    {
        public static void RootMenu()
        {
            var menu = new MenuBuilder("Table performance");
            menu.Items.Add(new MenuItem("Insert single entity", InsertSingleEntity));
            menu.Items.Add(new MenuItem("Insert multiple entities (batch)", InsertMultipleEntity));
            menu.Items.Add(new MenuItem("Select all entities via single thread", SelectEntitiesSingleThread));
            menu.Items.Add(new MenuItem("Select all entities via multiple threads", SelectEntitiesByPartitions));
            menu.Items.Add(new MenuItem("Select all entities via multiple threads (smart)", SelectEntitiesByPartitionsSmart));
            menu.ShowMenu();
        }

        public static void InsertSingleEntity()
        {
        }

        public static void InsertMultipleEntity()
        {
        }

        public static void SelectEntitiesSingleThread()
        {
        }

        public static void SelectEntitiesByPartitions()
        {
        }

        public static void SelectEntitiesByPartitionsSmart()
        {
        }
    }
}