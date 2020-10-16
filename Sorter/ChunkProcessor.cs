using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sorter
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
                while (!reader.EndOfStream)
                {
                    File.WriteAllLines(@$"Temp\{num}.txt", GetRecords(reader).AsParallel().OrderBy(r => r).Select(r => r.ToString()));
                    
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write(new string(' ', Console.BufferWidth));
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write($"Splitting in progress: {_inuptStream.Position} of {_inuptStream.Length} ({(_inuptStream.Position / (float)_inuptStream.Length) * 100:P2}), chuncks created: {num}");

                    num++;
                }

                Console.WriteLine();
            }
        }

        public void Dispose()
        {
            _inuptStream.Close();
        }

        private IEnumerable<Record> GetRecords(StreamReader reader)
        {
            for (int i = 0; i < _chunkSize; i++)
            {
                if (reader.EndOfStream)
                    yield break;

                yield return new Record(reader.ReadLine());
            }
        }
    }

}