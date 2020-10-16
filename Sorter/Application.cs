using System;
using System.Diagnostics;

namespace Sorter
{
    internal static class Application
    {
        public static void Run(ApplicationSettings settings)
        {
            PrepareTempDirectory(settings);
            try
            {
                Stopwatch stopwatch;
                using (var chunkProcessor = new ChunkProcessor(settings.InputFile.OpenRead(), settings.BufferSize))
                {
                    Console.WriteLine("Start chunks creating");
                    stopwatch = Stopwatch.StartNew();

                    chunkProcessor.Process();

                    stopwatch.Stop();
                    Console.WriteLine("Chunks created! Time: " + stopwatch.Elapsed);
                }

                Console.WriteLine("Start merge chunks");
                stopwatch = Stopwatch.StartNew();

                var sorter = new ChunkSorter(settings);
                sorter.Sort();

                stopwatch.Stop();
                Console.WriteLine("Chunks merged! Time: " + stopwatch.Elapsed);
            }
            finally
            {
                DeleteTempDirectory(settings);
            }
        }

        private static void PrepareTempDirectory(ApplicationSettings settings)
        {
            DeleteTempDirectory(settings);
            settings.TempDirectory.Create();
        }

        private static void DeleteTempDirectory(ApplicationSettings settings)
        {
            if (settings.TempDirectory.Exists)
            {
                settings.TempDirectory.Delete(true);
            }
        }
    }

}