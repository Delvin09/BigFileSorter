using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sorter
{
    internal class ChunkSorter
    {
        private readonly ApplicationSettings _settings;

        public ChunkSorter(ApplicationSettings settings)
        {
            this._settings = settings;
        }

        public void Sort()
        {
            var files = _settings.TempDirectory.EnumerateFiles("*.txt", SearchOption.TopDirectoryOnly)
                .Select(file => new ChunkFile(file))
                .ToList();

            var result = new Queue<Record>(_settings.BufferSize);
            var outputFilePath = _settings.OutputFile.FullName;

            ChunkFile next = null;
            while (true)
            {
                bool done = true;
                foreach (var mergeFile in files)
                {
                    done = false;
                    if (next == null || mergeFile.Current.CompareTo(next.Current) < 1)
                    {
                        next = mergeFile;
                    }
                }
                if (done) break;
                result.Enqueue(next.Current);
                if (result.Count >= _settings.BufferSize)
                {
                    File.AppendAllLines(outputFilePath, result.Select(r => r.ToString()));
                    result.Clear();
                }

                if (!next.MoveNext())
                {
                    next.Dispose();
                    files.Remove(next);
                    next = null;
                }
            }

            if (result.Count > 0)
            {
                File.AppendAllLines(outputFilePath, result.Select(r => r.ToString()));
                result = null;
            }
        }
    }

}