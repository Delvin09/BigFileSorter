using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Old.Sorter
{
    internal class ChunkFile : IEnumerator<Record>
    {
        private readonly StreamReader _streamReader;
        private readonly FileInfo _file;
        private readonly bool _withCache;
        private readonly Queue<Record> _cache;

        public ChunkFile(FileInfo file, bool withCache = true)
        {
            _file = file;
            _withCache = withCache;
            if (_withCache) _cache = new Queue<Record>(1000);

            _streamReader = file.OpenText();

            Current = new Record(_streamReader.ReadLine());
            if (_withCache) FillCache();
        }

        public Record Current { get; private set; }

        object IEnumerator.Current => Current;

        public FileInfo File => _file;

        public void Dispose()
        {
            _streamReader.Dispose();
            _file.Delete();
        }

        public bool MoveNext()
        {
            if (_withCache)
            {
                if (_cache.Count == 0)
                {
                    FillCache();
                    if (_cache.Count == 0 && _streamReader.EndOfStream)
                        return false;
                }

                var line = _cache.Dequeue();
                Current = line;
                return true;
            }
            else
            {
                if (_streamReader.EndOfStream) return false;
                var line = _streamReader.ReadLine();
                Current = new Record(line);
                return true;
            }

        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        private void FillCache()
        {
            for (int i = 0; i < 1000 && !_streamReader.EndOfStream; i++)
            {
                var line = _streamReader.ReadLine();
                _cache.Enqueue(new Record(line));
            }
        }
    }
}