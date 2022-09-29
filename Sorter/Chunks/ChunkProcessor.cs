using Sorter.Misc;
using System;
using System.IO;
using System.Linq;

namespace Sorter.Chunks
{
    internal class ChunkProcessor : IDisposable
    {
        private readonly Stream _inuptStream;
        private readonly int _chunkSize;

        public ChunkProcessor(Stream inuptStream, int chunkSize)
        {
            this._inuptStream = inuptStream;
            this._chunkSize = chunkSize;
        }

        public void Process()
        {
            using (var reader = new StreamReader(_inuptStream))
            {
                int num = 0;

                UpdateProgress(num);

                var recordsReader = new LineReader(reader, _chunkSize);
                while (!recordsReader.IsEOF)
                {
                    var records = recordsReader.ReadNext()
                        .ToArray()
                        .AsParallel()
                        .OrderBy(r => r);

                    using (var writer = new StreamWriter(@$"Temp\{num}.txt"))
                    {
                        foreach (var r in records)
                            writer.WriteLine(r.Span);
                    }

                    num++;

                    UpdateProgress(num);
                }

                Console.WriteLine();
            }
        }

        private void UpdateProgress(int chunkNum)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write($"Splitting in progress: {_inuptStream.Position} of {_inuptStream.Length} ({(_inuptStream.Position / (float)_inuptStream.Length):P2}), chuncks created: {chunkNum}");
        }

        public void Dispose()
        {
            _inuptStream.Close();
        }
    }
}