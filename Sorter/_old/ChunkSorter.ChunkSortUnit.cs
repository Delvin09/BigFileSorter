using Old.Sorter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Old.Sorter
{
    internal partial class ChunkSorter
    {
        private class ChunkSortUnit
        {
            private const int DefaultBufferSize = 20000;

            private readonly IList<ChunkFile> _chunks;
            private readonly string _outputFilePath;
            private readonly int _bufferSize;

            public ChunkSortUnit(IList<ChunkFile> chunks, string outputFilePath, int bufferSize = DefaultBufferSize)
            {
                this._chunks = chunks;
                this._outputFilePath = outputFilePath;
                this._bufferSize = bufferSize;
            }

            public void Process()
            {
                var buffer = new Queue<Record>(_bufferSize);

                ChunkFile currentChunk = null;
                while (true)
                {
                    bool done = true;
                    foreach (var chunk in _chunks)
                    {
                        done = false;
                        if (currentChunk == null || chunk.Current.CompareTo(currentChunk.Current) < 1)
                        {
                            currentChunk = chunk;
                        }
                    }
                    if (done) break;

                    buffer.Enqueue(currentChunk.Current);
                    if (buffer.Count >= _bufferSize)
                    {
                        File.AppendAllLines(_outputFilePath, buffer.Select(r => r.ToString()));
                        buffer.Clear();
                    }

                    if (!currentChunk.MoveNext())
                    {
                        currentChunk.Dispose();
                        _chunks.Remove(currentChunk);
                        currentChunk = null;
                    }
                }

                if (buffer.Count > 0)
                {
                    File.AppendAllLines(_outputFilePath, buffer.Select(r => r.ToString()));
                    buffer = null;
                }
            }
        }
    }
}