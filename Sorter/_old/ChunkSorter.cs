using Sorter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Old.Sorter
{
    internal partial class ChunkSorter
    {
        private readonly ApplicationSettings _settings;

        public ChunkSorter(ApplicationSettings settings)
        {
            this._settings = settings;
        }

        public void Sort()
        {
            var outputFilePath = _settings.OutputFile.FullName;
            List<ChunkFile> files = GetChunks();

            if (files.Count == 0)
            {
                Console.WriteLine("No files found to sort");
            }
            else if (files.Count == 1)
            {
                files.First().File.MoveTo(outputFilePath);
            }
            else
            {
                while (files.Count > 1)
                {
                    if (Environment.ProcessorCount > 1 && files.Count >= Environment.ProcessorCount * 2)
                    {
                        var unitCount = Environment.ProcessorCount;
                        var fileForProcess = files.Count / unitCount;
                        var appendix = files.Count % unitCount;

                        var units = new ChunkSortUnit[unitCount];
                        for (int i = 0; i < unitCount; i++)
                        {
                            var filesToUnit = files.Skip(i * fileForProcess).Take(i == unitCount - 1 ? fileForProcess + appendix : fileForProcess).ToList();
                            units[i] = new ChunkSortUnit(filesToUnit, GenerateOutputFileName(filesToUnit, _settings.TempDirectory.FullName), _settings.BufferSize / unitCount);
                        }

                        Parallel.Invoke(units.Select(u => new Action(u.Process)).ToArray());
                    }
                    else
                    {
                        var unit = new ChunkSortUnit(files, outputFilePath, _settings.BufferSize);
                        unit.Process();
                        break;
                    }

                    files = GetChunks();
                }
            }
        }

        private List<ChunkFile> GetChunks()
            => _settings.TempDirectory.EnumerateFiles("*.txt", SearchOption.TopDirectoryOnly)
                .Select(file => new ChunkFile(file))
                .ToList();

        private string GenerateOutputFileName(IList<ChunkFile> chunks, string directory)
            => Path.Combine(directory, string.Join('_', chunks.Select(c => Path.GetFileNameWithoutExtension(c.File.Name))) + ".txt");
    }
}