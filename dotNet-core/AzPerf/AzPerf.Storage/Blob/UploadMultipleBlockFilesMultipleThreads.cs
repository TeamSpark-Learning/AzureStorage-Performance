﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AzPerf.Storage.Helpers;

namespace AzPerf.Storage.Blob
{
    public class UploadMultipleBlockFilesMultipleThreads : StoragePerformanceScenarioBase
    {
        protected int FilesCount { get; set; }
        protected int ThreadsCount { get; set; }
        protected int FileSizeInMb { get; set; }
        protected ConcurrentBag<string> Files = new ConcurrentBag<string>();

        protected async Task InitializeFilesCountAsync()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("How many files to use: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            var input = Console.ReadLine();

            int count;
            if (!int.TryParse(input, out count))
            {
                await InitializeFilesCountAsync();
                return;
            }

            if (count < 1)
            {
                await InitializeFilesCountAsync();
                return;
            }

            FilesCount = count;
        }

        protected async Task InitializeThreadsCountAsync()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("How many threads to use: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            var input = Console.ReadLine();

            int count;
            if (!int.TryParse(input, out count))
            {
                await InitializeThreadsCountAsync();
                return;
            }

            if (count < 0)
            {
                await InitializeThreadsCountAsync();
                return;
            }

            ThreadsCount = count;
        }

        protected async Task InitializeFilesAsync()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Chose each file size in MB: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            var input = Console.ReadLine();

            int size;
            if (!int.TryParse(input, out size))
            {
                await InitializeFilesAsync();
                return;
            }

            if (size < 0)
            {
                await InitializeFilesAsync();
                return;
            }

            FileSizeInMb = size;

            var tasks = new List<Task>();
            for (var i = 0; i < FilesCount; i++)
            {
                tasks.Add(CreateFile());
            }

            await Task.WhenAll(tasks);
        }

        private async Task CreateFile()
        {
            var filePath = Path.GetTempFileName();

            await FileHelper.CreateFileWithRandomContentAsync(filePath, FileSizeInMb);
            Files.Add(filePath);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(filePath);
        }

        protected override async Task InitializeAsync()
        {
            await InitializeThreadsCountAsync();
            await InitializeFilesCountAsync();
            await InitializeFilesAsync();
        }

        protected override async Task DoWorkAsync()
        {
            await Task.Delay(500);
        }

        protected override async Task CleanupAsync()
        {
            foreach (var file in Files)
            {
                if (!string.IsNullOrEmpty(file) && File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }
    }
}