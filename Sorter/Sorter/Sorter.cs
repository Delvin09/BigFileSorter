using Sorter.Chunks;
using Sorter.Misc;
using Sorter.Records;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sorter
{
    internal class Sorter
    {
        private readonly ApplicationSettings _settings;

        public Sorter(ApplicationSettings settings)
        {
            this._settings = settings;
        }

        public void Sort()
        {
            var outputFilePath = _settings.OutputFile.FullName;
            List<FileInfo> files = GetChunks();

            Console.WriteLine("Chunks Size is equals: " + (files.Sum(f => f.Length) == _settings.InputFile.Length));
            Console.WriteLine("Input Size " + _settings.InputFile.Length);
            Console.WriteLine("OutPt Size " + files.Sum(f => f.Length));

            if (files.Count == 0)
            {
                Console.WriteLine("No files found to sort");
            }
            else if (files.Count == 1)
            {
                files[0].MoveTo(outputFilePath);
            }
            else
            {
                //MergeByTreeQueue(files, outputFilePath);
              
                var priorityQueue = new PriorityQueue<ChunkFile, Record>(files.Count + 1);
                foreach (var file in files.Select(file => new ChunkFile(file, _settings.BufferSize / files.Count)))
                {
                    priorityQueue.Enqueue(file, file.Current);
                }

                using (var writer = new StreamWriter(outputFilePath))
                {
                    while (priorityQueue.Count > 0)
                    {
                        var file = priorityQueue.Dequeue();

                        writer.WriteLine(file.Current.Span);

                        if (file.MoveNext())
                            priorityQueue.Enqueue(file, file.Current);
                        else
                            file.Dispose();
                    }
                }
            }
        }

        private void MergeByTreeQueue(List<FileInfo> files, string outputFilePath)
        {
            var priorityQueue = new ChunksQueue(files.Select(f => new ChunkFile(f, _settings.BufferSize / files.Count)));
            using (var writer = new StreamWriter(outputFilePath))
            {
                while (priorityQueue.Count > 0)
                {
                    var file = priorityQueue.Peek();
                    writer.WriteLine(file.Current.Span);

                    if (!priorityQueue.MoveNext())
                    {
                        file.Dispose();
                    }
                }
            }
        }

        private List<FileInfo> GetChunks() => _settings.TempDirectory.EnumerateFiles("*.txt", SearchOption.TopDirectoryOnly).ToList();
    }
}